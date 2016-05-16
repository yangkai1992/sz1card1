using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    /// <summary>
    /// 分布式上下文提供程序构建工厂
    /// </summary>
    public interface IDistributedContextProviderFactory
    {
        /// <summary>
        /// 创建分布式上下文提供程序实例
        /// </summary>
        /// <returns>分布式上下文提供程序实例</returns>
        IDistributedContext CreateInstance();
    }
}
