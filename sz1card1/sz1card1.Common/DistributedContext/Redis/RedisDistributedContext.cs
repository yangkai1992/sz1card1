using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.Remoting.Messaging;

namespace sz1card1.Common.Distributed
{
    public class RedisDistributedContext:IDistributedContext
    {
        private string contextId;
        private IDataState session;
        private IDataState winIdentity;
        private IDataState application;

        public RedisDistributedContext()
        {
        }

        private RedisDistributedContext(string contextId)
        {
            this.contextId = contextId;
        }

        public IDistributedContext Current
        {
            get
            {
                string contextId = getIdentity();
                var distributedContext = (IDistributedContext)CallContext.GetData(contextId);
                if (distributedContext == null)
                {
                    distributedContext=new RedisDistributedContext(contextId);
                    CallContext.SetData(contextId, distributedContext);
                }
                return distributedContext;
            }
        }

        public string ContextID
        {
            get
            {
                return contextId;
            }
        }

        private static string getIdentity()
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
        public IDataState Session
        {
            get
            {
                if (session == null)
                {
                    session = new RedisSessionDataState();
                }
                return session;
            }
        }

        /// <summary>
        /// Windows缓存对象
        /// </summary>
        public IDataState WinIdentity
        {
            get
            {
                if (winIdentity == null)
                {
                    winIdentity = new RedisWinIdentityDataState();
                }
                return winIdentity;
            }
        }

        /// <summary>
        /// 应用程序缓存对象
        /// </summary>
        public IDataState Application
        {
            get
            {
                if (application == null)
                {
                    application = new RedisApplicationDataState();
                }
                return application;
            }
        }
    }
}
