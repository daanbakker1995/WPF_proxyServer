using ProxyServer.HTTP;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ProxyServer.Server
{
    class Cache
    {
        private ConcurrentDictionary<string, CacheItem> cachedItems; // A threadsafe dictionary

        public Cache()
        {
            cachedItems = new ConcurrentDictionary<string, CacheItem>();
        }

        /// <summary>
        /// Add a item to cache
        /// </summary>
        /// <param name="request">HTTPRequest</param>
        /// <param name="response"></param>
        public void AddItem(HTTPRequest request, HTTPResponse response)
        {
            // If not stored
            if(!IsItemCached(request.StartLine))
            {
                // Create new cache item
                CacheItem item = new CacheItem(response, request);
                // Try to add item to cache
                try
                {
                    cachedItems.TryAdd(request.StartLine, item);
                }catch(ArgumentNullException e)
                {
                    throw new ArgumentNullException("Item not stored, key is null. Feedback:"+e.Message);
                }
                catch(OverflowException e)
                {
                    throw new OverflowException("Item not stored, The dictionary already contains the maximum number of elements. Feedback:"+e.Message);
                }
            }
        }

        /// <summary>
        /// Get item from cache. Returns null if not found
        /// </summary>
        /// <param name="statusLine"></param>
        /// <returns>CacheItem if item is found; otherwise, null</returns>
        public CacheItem GetItem(string statusLine)
        {
            try
            {
                if (cachedItems.TryGetValue(statusLine, out CacheItem cacheItem)) return cacheItem;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        /// <summary>
        /// Delete item from cache
        /// </summary>
        /// <param name="statusLine"></param>
        public void DeleteItem(string statusLine)
        {
            try
            {
                cachedItems.TryRemove(statusLine, out CacheItem item);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Check if item is cached
        /// </summary>
        /// <param name="StatusLine"></param>
        /// <returns>true if item is cached; otherwise, false</returns>
        public bool IsItemCached(string StatusLine)
        {
            CacheItem item = GetItem(StatusLine);
            return (item != null);
        }
    }
}
