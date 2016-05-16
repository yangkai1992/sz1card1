using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Memcached
{
    /// <summary>
    /// 缓存数据对象
    /// </summary>
    public abstract class MemcachedDataState : IDisposable
    {
        protected int timeout;//单位：分钟
        protected string identity;
        protected string sectionName;
        protected DateTime lastOperationTime = DateTime.Now;
        protected static TimeSpan refreshTimeSpan = new TimeSpan(0, 5, 0);
        protected MemcachedStoreProvider provider;

        public MemcachedDataState()
        {
            this.sectionName = this.GetType().Name;
            provider = new MemcachedStoreProvider(sectionName);
        }

        public virtual string Identity
        {
            get { return identity; }
        }

        public virtual int Timeout
        {
            get { return timeout; }
        }

        public virtual DateTime LastOperationTime
        {
            get { return lastOperationTime; }
        }

        public abstract object this[string name]
        {
            get;
            set;
        }

        protected void ResetItemTimeout(string key)
        {
            if (DateTime.Now - lastOperationTime > refreshTimeSpan)
            {
                provider.ResetItemTimeout(key);
                lastOperationTime = DateTime.Now;
            }
        }

        #region IDisposable 成员

        public virtual void Dispose()
        {
            if (provider != null)
            {
                provider.Dispose();
            }
        }

        #endregion
    }
}
