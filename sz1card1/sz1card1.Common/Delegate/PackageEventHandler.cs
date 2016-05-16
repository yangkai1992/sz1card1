using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sz1card1.Common.Iso8583Communication;

namespace sz1card1.Common.Delegate
{
    /// <summary>
    /// 数据包处理事件
    /// </summary>
    /// <param name="requestPackage">请求数据包</param>
    /// <returns>响应数据包</returns>
    public delegate Iso8583Package PackageEventHandler(Iso8583Package requestPackage);
}
