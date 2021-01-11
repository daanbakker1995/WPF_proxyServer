using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyServer.HTTP
{
    class HTTPResponse : HTTPMessage
    {
        public HTTPResponse(string StartLine, List<HTTPHeader> headerItems, byte[] body, byte[] messageInBytes) : base(StartLine, headerItems, body, messageInBytes) { }

        /// <summary>
        /// Convert bytes to a readable response
        /// </summary>
        public static HTTPResponse TryParse(byte[] responseBytes)
        {
            string responseString = Encoding.UTF8.GetString(responseBytes);
            List<string> responseLines = SplitHeader(responseString);

            string firstLine = responseLines[0];
            List<HTTPHeader> headers = GetHeaders(responseLines);
            byte[] body = ReadBody(responseString);

            if (headers.Count() > 0) return new HTTPResponse(firstLine, headers, body, responseBytes);

            return null;
        }

        /// <summary>
        /// A placeholder HTTPResponse for to filter contents
        /// </summary>
        public static HTTPResponse GetFilterResponse()
        {
            string StartLine = "HTTP/1.1 200 Ok";
            List<HTTPHeader> HTTPHeaders = new List<HTTPHeader>
            {
                new HTTPHeader("Connection", "close"),
                new HTTPHeader("Content-Type", "image/svg+xml"),
                new HTTPHeader("Date", DateTime.Now.ToUniversalTime().ToString("r"))
            };
            byte[] body = Encoding.UTF8.GetBytes("<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 1987.8 1170.5'><rect fill='#ddd' width='1987.8' height='1170.5'/><text fill='rgba(0,0,0,0.5)' font-family='sans-serif' font-size='30' dy='10.5' font-weight='bold' x='50%' y='50%' text-anchor='middle'>Placeholder</text></svg>");                
            return new HTTPResponse(StartLine, HTTPHeaders, body, new byte[0]);
        }

        /// <summary>
        /// Returns HTTPResponse 407: Proxy Authentication Required
        /// <para> Equivalent to HTTP status 407. 
        /// ProxyAuthenticationRequired indicates that the requested proxy requires authentication.
        /// The Proxy-authenticate header contains the details of how to perform the authentication.</para>
        /// </summary>
        public static HTTPResponse Get407Response()
        {
            string StartLine = "HTTP/1.1 407 Proxy Authentication Required ";
            List<HTTPHeader> HTTPHeaders = new List<HTTPHeader>
            {
                new HTTPHeader("Connection", "close"),
                new HTTPHeader("Content-Type", "text/html; charset=utf-8"),
                new HTTPHeader("Date", DateTime.Now.ToUniversalTime().ToString("r"))
            };
            byte[] body = Encoding.UTF8.GetBytes("<html><h1>The server understood the request but refuses to authorize it. Please authorize and try again.</h1></html>");

            return new HTTPResponse(StartLine, HTTPHeaders, body, new byte[0]);
        }
    }
}
