using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Memcached
{
    /// <summary>
    /// 应用程序缓存数据对象
    /// </summary>
    public class MemcachedApplicationDataState : MemcachedDataState
    {
        public MemcachedApplicationDataState()
            : base()
        {
            timeout = 240;
            identity = "application".ToDes();
        }

        public override object this[string name]
        {
            get
            {
                string key = string.Format("{0}_{1}", identity, name);
                DataCollection dataCollection = provider.GetItem(key);
                if (dataCollection == null)
                {
                    return null;
                }
                ResetItemTimeout(key);
                return dataCollection[key];
            }
            set
            {
                string key = string.Format("{0}_{1}", identity, name);
                DataCollection dataCollection = new DataCollection(timeout);
                dataCollection[key] = value;
                provider.SetItem(key, dataCollection);
                lastOperationTime = DateTime.Now;
            }
        }
    }
}
