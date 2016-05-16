using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace sz1card1.Common
{
    /// <summary>
    /// 分布式上下文提供程序构建工厂
    /// </summary>
    public class DistributedContextProviderFactory : IDistributedContextProviderFactory
    {
        private readonly Type _providerType;

        public DistributedContextProviderFactory(string distributedContextProvider)
        {
            if (distributedContextProvider == null) throw new ArgumentNullException("distributedContextProvider");

            _providerType = Type.GetType(distributedContextProvider, true, true);
        }

        public DistributedContextProviderFactory()
            : this(ConfigurationManager.AppSettings["DistributedContextProvider"])
        {
        }

        public IDistributedContext CreateInstance()
        {
            return Activator.CreateInstance(_providerType) as IDistributedContext;
        }
    }
}
