using ProxyServer.HTTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProxyServer.Server
{
    class CacheItem
    {
        public HTTPResponse Response { get; }
        public DateTime CacheDate { get; }
        public int ExperationTimeInSeconds;

        public CacheItem(HTTPResponse response, HTTPRequest request)
        {
            Response = response;
            CacheDate = DateTime.UtcNow;
            ExperationTimeInSeconds = 0;

            if(request.ContainsHeader("Cache-Control") && request.GetHeader("Cache-Control").Value.Contains("max-age="))
            {
                int.TryParse(request.GetHeader("Cache-Control").Value.Split('=')[1], out ExperationTimeInSeconds);
            }
        }

        public bool IsNotExpired(int TimeOutInSeconds)
        {
            if(ExperationTimeInSeconds > 0)
            {
                return (CacheTimeInSeconds() <= ExperationTimeInSeconds) && (CacheTimeInSeconds() <= TimeOutInSeconds);
            }
            return CacheTimeInSeconds() <= TimeOutInSeconds;
        }

        public double CacheTimeInSeconds() => (DateTime.UtcNow - CacheDate).TotalSeconds;
    }
}
