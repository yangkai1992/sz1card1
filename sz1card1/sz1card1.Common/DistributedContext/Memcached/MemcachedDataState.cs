using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace sz1card1.Common.Distributed
{
    /// <summary>
    /// 缓存数据对象
    /// </summary>
    public abstract class MemcachedDataState : IDisposable,IDataState
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
            set { throw new NotImplementedException(); }
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

        public virtual object this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual void ResetTimeout()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Remove(string name)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            provider.RemoveItem(Identity);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void Persistent()
        {
        }

        public object SyncRoot { get; private set; }
        public bool IsSynchronized { get; private set; }
        public bool Dirty { get; set; }

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
