using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace sz1card1.Common.Distributed
{
    public class RedisDataState:IDataState
    {
        private bool timeoutChanged;
        protected int timeout;//单位：分钟
        protected string identity;
        protected string sectionName;
        protected RedisStoreProvider provider;

        public RedisDataState()
        {
            this.sectionName = this.GetType().Name;
            provider = new RedisStoreProvider(sectionName);
        }

        public RedisDataState(string sectionName)
        {
            this.sectionName = sectionName;
            provider = new RedisStoreProvider(this.sectionName);
        }

        public virtual string Identity
        {
            get { return identity; }
        }

        public virtual int Timeout
        {
            get { return timeout; }
            set
            {
                if (timeout != value)
                {
                    timeout = value;
                    timeoutChanged = true;
                }
            }
        }

        public virtual void Initialize()
        {
            timeoutChanged = true;
            ResetTimeout();
        }

        public virtual object this[string name]
        {
            get
            {
                return provider.HashGetItem(identity, name);
            }
            set
            {
                provider.HashSetItem(identity, name, value);
            }
        }

        public virtual object this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual void ResetTimeout()
        {
            if (timeoutChanged)
            {
                provider.SetHashTimeout(identity, timeout);
                timeoutChanged = false;
            }
            provider.SetHashExpire(identity, timeout);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return provider.HashGetAllItem(Identity).GetEnumerator(); ;
        }

        public void Remove(string name)
        {
            provider.HashRemove(Identity, name);
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


        //#region IDisposable 成员

        //public virtual void Dispose()
        //{
        //    if (provider != null)
        //    {
        //        provider.Dispose();
        //    }
        //}

        //#endregion
    }
}
