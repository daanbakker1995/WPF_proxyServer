using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyServer.HTTP
{
    public class HTTPRequest : HTTPMessage
    {
        public HTTPRequest(string StartLine, List<HTTPHeader> headerItems, byte[] body, byte[] messageInBytes) : base(StartLine, headerItems, body, messageInBytes) { }

        public static HTTPRequest TryParse(byte[] requestBytes)
        {
            string responseString = Encoding.UTF8.GetString(requestBytes);
            List<string> responseLines = SplitHeader(responseString);

            string StartLine = responseLines[0];
            List<HTTPHeader> headers = GetHeaders(responseLines);
            byte[] body = ReadBody(responseString);

            if (headers.Count() > 0) return new HTTPRequest(StartLine, headers, body, requestBytes);

            return null;
        }
        /// <summary>
        /// Remove User-Agent from header
        /// </summary>
        public void HideUserAgent()
        {
            if (ContainsHeader("User-Agent"))
                RemoveHeader("User-Agent");
        }

        /// <summary>
        /// Returns the host of the request
        /// </summary>
        public string GetHost()
        {
            if (ContainsHeader("Host")) return GetHeader("Host").Value;
            return StartLine.Split(' ')[1];
        }

        /// <summary>
        ///  Check if the request contains cache-control
        /// </summary>
        public bool IsCacheable()
        {
            // If request has a 'Cache-Control: private' header, request is not cacheable.
            if (ContainsHeader("Cache-Control") && GetHeader("Cache-Control").Value == "private") return false;

            return true;
        }

        /// <summary>
        /// Returns if request is png
        /// </summary>
        /// <returns></returns>
        public bool HasContentToFilter()
        {
            string[] fileTypesToFilter = new string[] { ".png" };
            return fileTypesToFilter.Any(fileType => StartLine.Split(' ')[1].ToLower().Contains(fileType));
        }
    }
}
