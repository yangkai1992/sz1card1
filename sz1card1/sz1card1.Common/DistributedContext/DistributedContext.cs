using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Web;
using sz1card1.Common.Distributed;

namespace sz1card1.Common
{
    /// <summary>
    /// 分布式Context
    /// </summary>
    public class DistributedContext 
    {
        private static IDistributedContext _provier;
        private static IDistributedContext provier
        {
            get
            {
                if (_provier == null)
                {
                    _provier = new RedisDistributedContext();
                }
                return _provier;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="factory">依赖解析器创建工厂</param>
        public static void InitializeWith(IDistributedContextProviderFactory factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");

            _provier = factory.CreateInstance();
        }

        public static void RegisterContext(string identity)
        {
            CallContext.SetData("identity", identity);
        }

        public static void UnRegisterContext(string identity)
        {
            if (CallContext.GetData("identity") != null)
            {
                Current.Session.Persistent();
                CallContext.SetData("identity", null);
            }
        }

        public static IDistributedContext Current
        {
            get
            {
                return provier.Current;
            }
        }
    }
}
