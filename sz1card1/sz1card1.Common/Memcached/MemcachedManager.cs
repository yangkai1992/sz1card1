using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using sz1card1.Common.Memcached.Session;

namespace sz1card1.Common.Memcached
{
    public class MemcachedManager : IDisposable
    {
        private MemcachedClient memcachedClient;

        public MemcachedManager(string sectionName)
        {
            sectionName = string.Format("sz1card1.memcached/{0}", sectionName);
            memcachedClient = new MemcachedClient(sectionName);
        }

        public IEnumerable<T> GetItems<T>(string key)
        {
            List<T> items = new List<T>();
            IEnumerable list = GetItems(key);
            foreach (var item in list)
            {
                if (item != null && (item is T))
                {
                    items.Add((T)item);
                }
            }
            return items;
        }

        public IEnumerable GetItems(string key)
        {
            IEnumerable<string> items = GetKeys();
            ArrayList results = new ArrayList();
            if (items == null)
            {
                return results;
            }
            foreach (string item in items)
            {
                object container = memcachedClient.Get(item);
                if (container is DataContainer)
                {
                    DataContainer dataContainer = (DataContainer)container;
                    if (dataContainer.ItemCollection != null && dataContainer.ItemCollection[key] != null)
                    {
                        results.Add(dataContainer.ItemCollection[key]);
                    }
                }
                else if (container is SessionStateDataContainer)
                {
                    SessionStateDataContainer dataContainer = (SessionStateDataContainer)container;
                    if (dataContainer.SessionStateItemCollection != null && dataContainer.SessionStateItemCollection[key] != null)
                    {
                        results.Add(dataContainer.SessionStateItemCollection[key]);
                    }
                }
            }
            return results;
        }

        public void SetItem(string itemName, string key, object value)
        {
            object container = memcachedClient.Get(itemName);
            if (container is DataContainer)
            {
                DataContainer dataContainer = (DataContainer)container;
                if (dataContainer.ItemCollection != null)
                {
                    dataContainer.ItemCollection[key] = value;
                    memcachedClient.Store(StoreMode.Set, itemName, dataContainer, TimeSpan.FromMinutes(dataContainer.Timeout));
                }
            }
            else if (container is SessionStateDataContainer)
            {
                SessionStateDataContainer dataContainer = (SessionStateDataContainer)container;
                if (dataContainer.SessionStateItemCollection != null)
                {
                    dataContainer.SessionStateItemCollection[key] = value;
                    memcachedClient.Store(StoreMode.Set, itemName, dataContainer, TimeSpan.FromMinutes(dataContainer.Timeout));
                }
            }
        }

        public void RemoveItem(string itemName, string key)
        {
            object container = memcachedClient.Get(itemName);
            if (container is DataContainer)
            {
                DataContainer dataContainer = (DataContainer)container;
                if (dataContainer.ItemCollection != null)
                {
                    dataContainer.ItemCollection.Remove(key);
                    memcachedClient.Store(StoreMode.Set, itemName, dataContainer, TimeSpan.FromMinutes(dataContainer.Timeout));
                }
            }
            else if (container is SessionStateDataContainer)
            {
                SessionStateDataContainer dataContainer = (SessionStateDataContainer)container;
                if (dataContainer.SessionStateItemCollection != null)
                {
                    dataContainer.SessionStateItemCollection.Remove(key);
                    memcachedClient.Store(StoreMode.Set, itemName, dataContainer, TimeSpan.FromMinutes(dataContainer.Timeout));
                }
            }
        }

        public void SetItems(string key, object value)
        {
            IEnumerable<string> items = GetKeys();
            foreach (string item in items)
            {
                SetItem(item, key, value);
            }
        }

        public void RemoveItems(string key)
        {
            IEnumerable<string> items = GetKeys();
            foreach (string item in items)
            {
                RemoveItem(item, key);
            }
        }

        private IEnumerable<string> GetKeys()
        {
            if (memcachedClient == null)
            {
                return null;
            }
            return memcachedClient.GetItemsKey();
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
