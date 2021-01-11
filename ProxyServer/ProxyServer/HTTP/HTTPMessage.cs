using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProxyServer.HTTP
{
    public class HTTPMessage
    {
        public string StartLine;
        public List<HTTPHeader> HeaderItems;
        public byte[] Body;
        public byte[] MessageInBytes;

        public HTTPMessage(string startLine, List<HTTPHeader> headerItems, byte[] body, byte[] messageInBytes)
        {
            StartLine = startLine;
            HeaderItems = headerItems;
            Body = body;
            MessageInBytes = messageInBytes;
        }

        /// <summary>
        /// Split headerstring to header items
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<string> SplitHeader(string message)
        {
            string[] result = message.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            return new List<string>(result);
        }

        /// <summary>
        /// Return message in bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            // Convert all data to byte arrays
            byte[] StartLineInBytes = Encoding.UTF8.GetBytes(StartLine);
            string headersString = string.Join("\r\n", HeaderItems.Select(header => header.ToString));
            byte[] headersBytes = Encoding.UTF8.GetBytes(headersString);
            // Make a newline byte array
            byte[] newLine = Encoding.UTF8.GetBytes(Environment.NewLine);

            // Make List of bytes from message with converted data
            List<byte> message = new List<byte>();
            message.AddRange(StartLineInBytes);
            message.AddRange(newLine);
            message.AddRange(headersBytes);
            message.AddRange(newLine);
            message.AddRange(newLine);
            message.AddRange(Body);

            return message.ToArray(); // Return array
        }

        /// <summary>
        ///  Read the body in the http message
        /// </summary>
        protected static byte[] ReadBody(string messageString)
        {
            // create a array from messageString 
            string[] bodyStringArray = messageString.Split(
                new[] { "\r\n", "\r", "\n" }, // Delimiters
                StringSplitOptions.None
            );

            byte[] bodyBytes = Encoding.UTF8.GetBytes(bodyStringArray[bodyStringArray.Length - 1]); // make byte array of string array

            return bodyBytes; // return body in bytes
        }

        /// <summary>
        /// returns a list of headers from the Message
        /// </summary>
        /// <param name="messageLines"></param>
        /// <returns></returns>
        public static List<HTTPHeader> GetHeaders(List<string> messageLines)
        {
            messageLines.RemoveAt(0); // remove StartLine

            List<HTTPHeader> headers = new List<HTTPHeader>(); // make list of headers
            foreach (string line in messageLines) 
            {
                if (line.Equals("")) // if line is empty break loop
                {
                    break;
                }

                int seperator = line.IndexOf(":");  // define seperator
                if (seperator == -1) continue; // if no seperator found, continue to the next item.

                try
                {
                    string key = line.Substring(0, seperator).Trim(); // set key
                    string value = line.Substring(seperator + 1).Trim(); // set value

                    headers.Add(new HTTPHeader(key, value)); // add a new HTTPHeader to the list
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }
            }
            return headers; // return result
        }

        /// <summary>
        /// Returns a single header based on key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HTTPHeader GetHeader(string key) => HeaderItems.Where(header => header.Key.ToLower() == key.ToLower()).FirstOrDefault();

        /// <summary>
        /// Checks if Message contains header
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsHeader(string key) => HeaderItems.Where(header => header.Key.ToLower() == key.ToLower()).Count() == 1;

        /// <summary>
        /// Add a header to the Message
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddHeader(string key, string value) => HeaderItems.Add(new HTTPHeader(key, value));

        /// <summary>
        /// Remove a header from HeaderItems
        /// </summary>
        /// <param name="key"></param>
        public void RemoveHeader(string key) => HeaderItems.Remove(GetHeader(key));

        /// <summary>
        /// Get All header items as a string
        /// </summary>
        /// <returns></returns>
        public string GetHeadersToString() => string.Join("\r\n", HeaderItems.Select(header=>header.ToString));

        /// <summary>
        /// Get Message Details as a string
        /// </summary>
        /// <returns></returns>
        public string GetDetails()
        {   
            string MessageDetails = "";
            if (ContainsHeader("User-Agent"))
            {
                MessageDetails = GetHeader("User-Agnet").Value;
            }
            MessageDetails += "\r\n";
            MessageDetails += Encoding.UTF8.GetString(Body, 0, Body.Length);
            return MessageDetails;
        }
        /// <summary>
        /// To string of message header
        /// </summary>
        public new string ToString => $"{StartLine}\r\n{GetHeadersToString()}";
    }
}
