using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace sz1card1.Common.Distributed
{
    public class MemcachedStoreProvider : IDisposable
    {
        private MemcachedClient memcachedClient;
        private static Dictionary<string, MemcachedClient> memcachedClients;

        static MemcachedStoreProvider()
        {
            memcachedClients = new Dictionary<string, MemcachedClient>();
        }

        public MemcachedStoreProvider(string sectionName)
        {
            if (!memcachedClients.Keys.Contains(sectionName))
            {
                lock (memcachedClients)
                {
                    if (!memcachedClients.Keys.Contains(sectionName))
                    {
                        string sectionNames = string.Format("sz1card1.memcached/{0}", sectionName);
                        memcachedClients.Add(sectionName, new MemcachedClient(sectionNames));
                    }
                }
            }
            memcachedClient = memcachedClients[sectionName];
        }

        public DataCollection GetItem(string key)
        {
            DataContainer dataContainer = memcachedClient.Get<DataContainer>(key);
            if (dataContainer == null)
            {
                return null;
            }
            return dataContainer.ItemCollection;
        }

        public void SetItem(string key, DataCollection dataCollection)
        {
            DataContainer container = new DataContainer(dataCollection, dataCollection.Timeout);
            bool storeResult = memcachedClient.Store(StoreMode.Set, key, container, TimeSpan.FromMinutes(container.Timeout));
            if (!storeResult)
            {
                throw new Exception("缓存服务器连接失败");
            }
        }

        public void ResetItemTimeout(string key)
        {
            DataContainer dataContainer = memcachedClient.Get<DataContainer>(key);
            if (dataContainer != null)
            {
                memcachedClient.Store(StoreMode.Set, key, dataContainer, TimeSpan.FromMinutes(dataContainer.Timeout));
            }
        }

        public void RemoveItem(string key)
        {
            memcachedClient.Remove(key);
        }

        #region IDisposable 成员

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/> implementation.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // The bulk of the clean-up code is implemented in this method
            if (disposing)
            {
                // free managed resources
                if (memcachedClient != null)
                {
                    memcachedClient.Dispose();
                    memcachedClient = null;
                }
            }
        }
        #endregion
    }
}
