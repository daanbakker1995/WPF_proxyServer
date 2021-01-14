using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProxyServer.HTTP;
using ProxyServer.Server;

namespace ProxyServer
{
    class Proxy
    {
        public TcpListener TCPListener;
        public Cache cache;

        // Server settings
        public IPAddress IPAdress;
        public int PortNumber;
        public int CacheTimeOut;


        public int BufferSize;
        public bool ServerStarted = false;

        private bool FilterContent = false;
        private bool UseCaching = false;
        private bool AuthRequired = false;
        private bool HideUserAgent = false;
        private bool LogRequestHeaders = false;
        private bool LogResponseHeaders = false;

        //Callback methods 
        private Action<string> AddMessageToLog;
        private Action<string> AddMessageToHTTPDetails;

        public Proxy(string IPAdress, 
            int port, 
            int buffer, 
            int cacheTimeOut,
            Action<string> AddMessageToLog,
            Action<string> AddMessageToHTTPDetails, 
            bool hideUserAgent, 
            bool filterContent, 
            bool logRequestHeaders, 
            bool logResponseHeaders, 
            bool useCaching, 
            bool useBasicAuth)
        {
            cache = new Cache();
            this.IPAdress = IPAddress.Parse(IPAdress);
            PortNumber = port;
            BufferSize = buffer;
            CacheTimeOut = cacheTimeOut;
            this.AddMessageToHTTPDetails = AddMessageToHTTPDetails;
            this.AddMessageToLog = AddMessageToLog;

            FilterContent = filterContent;
            UseCaching = useCaching;
            AuthRequired = useBasicAuth;
            HideUserAgent = hideUserAgent;
            LogRequestHeaders = logRequestHeaders;
            LogResponseHeaders = logResponseHeaders;
        }

        public void StartProxy()
        {
            try
            {
                TCPListener = new TcpListener(IPAdress, PortNumber);
                TCPListener.Start();
                ServerStarted = true;
                Task.Run(() => ConnectWithClients());
            }
            catch (Exception e)
            {
                AddMessageToLog($"Fout bij starten server: {e.Message}");
            }

        }

        private async Task ConnectWithClients()
        {
            AddMessageToLog("Luisteren naar requests");
            // Check if server is not closed
            while (ServerStarted)
            {
                try
                {
                    // Listen for new client to connect
                    TcpClient client = await TCPListener.AcceptTcpClientAsync();
                    // Handle receiving data
                    await Task.Run(() => HandleIncomingData(client));
                }
                catch(Exception e)
                { // Add exception to chat if server started and an exception caught
                    if (ServerStarted) AddMessageToLog("Fout bij accepteren client" + e.Message);
                }
            }
        }

        private async void HandleIncomingData(TcpClient client)
        {
            // Get stream from cient
            using NetworkStream stream = client.GetStream();
            // Receive request from stream
            HTTPRequest request = await ReceiveIncomingData(stream);
            // Create new HTTPResponse
            HTTPResponse response = null;

            // Request reveived
            if (request != null)
            {
                try
                {
                    // Protect identity
                    if (HideUserAgent) request.HideUserAgent();
                    // If insight to Request traffic is requested, send headers to UI HTTPDetails
                    if (LogRequestHeaders) AddMessageToHTTPDetails("NIEUW-REQUEST: " + request.GetHeadersToString());
                    // If authenication is not needed or approved
                    if (IsAuthenticated(request))
                    {
                        // if contentfilter is needed and contains content to be filtered
                        if (FilterContent && request.HasContentToFilter())
                        {
                            // Update respone with placeholder HTTPResponse
                            response = HTTPResponse.GetFilterResponse();
                            byte[] responseInBytes = response.ToBytes();
                            // Write response to stream
                            await stream.WriteAsync(responseInBytes, 0, responseInBytes.Length);
                            // If response headers are asked for, send headers to UI chat log
                            if (LogResponseHeaders && response != null) AddMessageToHTTPDetails("RESPONE-HEADERS: "+response.GetHeadersToString());

                            return;
                        }

                        // If caching active, content is cacheable and if in cache storage
                        if (UseCaching && request.IsCacheable() && cache.IsItemCached(request.StartLine))
                        {
                            // Get item from cache
                            CacheItem item = cache.GetItem(request.StartLine);

                            // If item is valid, return item as response
                            if (item.IsNotExpired(CacheTimeOut))
                            {
                                // Update response with Cache HTTPResponse
                                response = item.Response;
                                string startLine = response.StartLine;
                                // Update UI
                                AddMessageToLog("Uit cache: "+ startLine);
                                AddMessageToLog("Resterende tijd: "+item.CacheTimeInSeconds());
                                // Write response to stream
                                byte[] responseInBytes = response.ToBytes();
                                await stream.WriteAsync(responseInBytes, 0, responseInBytes.Length);

                                // If response headers are asked for, send headers to UI chat log
                                if (LogResponseHeaders && response != null) AddMessageToHTTPDetails("RESPONE-HEADERS: " + response.GetHeadersToString());

                                return;
                            }
                            else // Response expired
                            {
                                cache.DeleteItem(request.StartLine);
                                AddMessageToLog("Verlopen, verwijderde item: " + request.StartLine);
                            }
                        }

                        // No response has been set
                        if (response == null)
                        {
                            response = await SendHTTPResponseToClientAndReturn(request, stream);

                            // If request is cacheable and is not stored in cache
                            if (response != null && UseCaching && request.IsCacheable() && !cache.IsItemCached(request.StartLine))
                            {
                                // Add item to Cache
                                try
                                {
                                    cache.AddItem(request, response);
                                    AddMessageToLog("Toegevoegd aan cache "+request.StartLine);
                                }
                                catch (ArgumentNullException e)
                                {
                                    AddMessageToLog("ArgumentNullException opgetreden: "+e.Message);
                                }
                                catch (OverflowException e)
                                {
                                    AddMessageToLog("OverflowException opgetreden: " + e.Message);
                                }
                            }
                            else // Item not cacheable
                            {
                                if (UseCaching)
                                {
                                    AddMessageToLog("Niet mogelijk toe te voegen aan cache: " + request.StartLine);
                                }
                            }
                        }

                    }
                    else // Not authenticated response
                    {
                        response = HTTPResponse.Get407Response(); 
                    }
                    if (response != null && LogResponseHeaders) AddMessageToHTTPDetails("RESPONE-INFORMATIE: " + response.ToString); // Log response headers if needed
                }
                catch (Exception e)
                {
                    AddMessageToLog("Fout bij afhandelen inkomende data: \n" + e.Message);
                }
            }
        }

        private async Task<HTTPResponse> SendHTTPResponseToClientAndReturn(HTTPRequest request, NetworkStream stream)
        {
            // Create client for target host
            string host = request.GetHost();
            TcpClient Client = new TcpClient();
            Client.Connect(host, 80);
            
            AddMessageToLog("HOST: "+host);
            using NetworkStream ClientStream = Client.GetStream();
            // Send Request to host
            byte[] requestInBytes = request.ToBytes();
            await ClientStream.WriteAsync(requestInBytes, 0, requestInBytes.Length);
            // Create memory stream
            using MemoryStream memoryStream = new MemoryStream();
            try
            {
                byte[] buffer = new byte[BufferSize];
                // While can read host stream
                while (true)
                { 
                    int bytesReceived = await ClientStream.ReadAsync(buffer, 0, buffer.Length); // Read host stream
                    if (bytesReceived == 0) break;
                    await memoryStream.WriteAsync(buffer, 0, bytesReceived);// Place in memory
                    await stream.WriteAsync(buffer, 0, bytesReceived);  // Write to stream
                }
                // Get response in bytes from memory
                byte[] responseBytes = memoryStream.ToArray();
                // Create HTTP Response
                HTTPResponse response = HTTPResponse.TryParse(responseBytes);
                return response;
            }
            catch (ArgumentNullException)
            {
                AddMessageToLog("Error bij lezen stream: buffer is null.");
                return null;
            }
            catch (NotSupportedException)
            {
                AddMessageToLog("Error bij lezen stream: De stream ondersteund geen schrijfrechten.");
                return null;
            }
            catch (ObjectDisposedException)
            {
                AddMessageToLog("Error bij lezen stream: De stream is verbroken .");
                return null;
            }
            catch (InvalidOperationException)
            {
                AddMessageToLog("Error bij lezen stream: De stream is momenteel in gebruik door een voorgaande schrijf operatie.");
                return null;
            }
            catch (SocketException)
            {
                AddMessageToLog("Error bij lezen stream: De stream is momenteel in gebruik door een voorgaande schrijf operatie.");
                return null;
            }
            catch (Exception e)
            {
                AddMessageToLog("Fout bij versturen response: " + e.GetType());
                return null;
            }
        }

        /// <summary>
        /// <para>Checks if a user needs to be authenticated and if the request contains Admin header.</para>
        /// Returns: true if no authentication is required or is auhtenticated, false if authentication is required and the request doesn't containt Admin header.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsAuthenticated(HTTPRequest request)
        {
            if (AuthRequired)
            {
                if (request.ContainsHeader("Proxy-Authorization"))
                {
                    HTTPHeader header = request.GetHeader("Proxy-Authorization");
                    if (header.Value == "admin") return true;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Receives data from stream. Returns as HTTPRequest.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private async Task<HTTPRequest> ReceiveIncomingData(NetworkStream stream)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] requestBufferArray = new byte[BufferSize];
                    int bytesReceived;

                    if (stream.CanRead)
                    {
                        if (stream.DataAvailable)
                        {
                            do
                            {
                                bytesReceived = await stream.ReadAsync(requestBufferArray, 0, requestBufferArray.Length);
                                await memoryStream.WriteAsync(requestBufferArray, 0, bytesReceived);
                            } while (stream.DataAvailable);
                        }
                    }
                    byte[] requestBytesArray = memoryStream.ToArray();
                    HTTPRequest request = HTTPRequest.TryParse(requestBytesArray);

                    return request;
                }
            }
            catch (Exception e)
            {
                AddMessageToLog("Fout bij ontvangen informatie "+e.Message);
                return null;
            }
        }

        /// <summary>
        /// Checks if TCPListener pendeing, else stop listening.
        /// </summary>
        public void StopProxy()
        {
            while (true)
            {
                if(!TCPListener.Pending())
                {
                    TCPListener.Stop();
                    ServerStarted = !ServerStarted;
                    AddMessageToLog("Proxy server gestopt.");
                    break;
                }
                else
                {
                    AddMessageToLog("Bezig met afhandelen request/response. Probeer opnieuw.");
                    continue;
                }
            }
        }

        public void ClearCache()
        {
            cache = new Cache();
        }
    }
}
