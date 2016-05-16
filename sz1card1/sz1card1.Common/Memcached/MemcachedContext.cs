using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Linq;
using System.Text;
using System.Web;
using sz1card1.Common.Memcached;

namespace sz1card1.Common
{
    internal class MemcachedContext : IDisposable
    {
        private string contextId;
        private MemcachedDataState memcachedSession;
        private MemcachedDataState memcachedWinIdentity;
        private MemcachedDataState memcachedApplication;

        private static Dictionary<string, MemcachedContext> memcachedContexts;
        static MemcachedContext()
        {
            memcachedContexts = new Dictionary<string, MemcachedContext>();
        }

        private MemcachedContext(string contextId)
        {
            this.contextId = contextId;
        }

        public static void RegisterContext(string identity)
        {
            CallContext.SetData("identity", identity);
        }

        public static void UnRegisterContext(string identity)
        {

        }

        public static MemcachedContext Current
        {
            get
            {
                string contextId = GetContextId();
                if (!memcachedContexts.ContainsKey(contextId))
                {
                    lock (memcachedContexts)
                    {
                        if (!memcachedContexts.ContainsKey(contextId))
                        {
                            memcachedContexts.Add(contextId, new MemcachedContext(contextId));
                        }
                    }
                }
                return memcachedContexts[contextId];
            }
        }

        public string ContextID
        {
            get
            {
                return contextId;
            }
        }

        private static string GetContextId()
        {
            string contextId;
            if (HttpContext.Current == null)
            {
                if (CallContext.GetData("identity") == null)
                {
                    throw new NullReferenceException("没有找到ContextID,请在程序开始时调用RegisterContext方法");
                }
                contextId = CallContext.GetData("identity").ToString();
            }
            else
            {
                contextId = HttpContext.Current.Session.SessionID;
            }
            return contextId;
        }

        /// <summary>
        /// Session缓存对象
        /// </summary>
        public MemcachedDataState MemcachedSession
        {
            get
            {
                if (memcachedSession == null)
                {
                    memcachedSession = new MemcachedSessionDataState();
                }
                return memcachedSession;
            }
        }

        /// <summary>
        /// Windows缓存对象
        /// </summary>
        public MemcachedDataState MemcachedWinIdentity
        {
            get
            {
                memcachedWinIdentity = new MemcachedWinIdentityDataState();
                return memcachedWinIdentity;
            }
        }

        /// <summary>
        /// 应用程序缓存对象
        /// </summary>
        public MemcachedDataState MemcachedApplication
        {
            get
            {
                if (memcachedApplication == null)
                {
                    memcachedApplication = new MemcachedApplicationDataState();
                }
                return memcachedApplication;
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            lock (memcachedContexts)
            {
                if (memcachedContexts.ContainsKey(contextId))
                {
                    MemcachedContext context = memcachedContexts[contextId];
                    memcachedContexts.Remove(contextId);
                    context.Dispose();
                }
                if (this.memcachedSession != null)
                    this.memcachedSession.Dispose();
                if (this.memcachedWinIdentity != null)
                    this.memcachedWinIdentity.Dispose();
                if (this.memcachedApplication != null)
                    this.memcachedApplication.Dispose();
            }
        }

        #endregion
    }
}
