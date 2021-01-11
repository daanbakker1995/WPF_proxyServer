using System;
using System.Collections.Generic;
using System.Text;

namespace ProxyServer.HTTP
{
    public class HTTPHeader
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public HTTPHeader(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public new string ToString => $"{Key}: {Value}";
    }
}
