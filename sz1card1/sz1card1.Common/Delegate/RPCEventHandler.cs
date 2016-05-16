using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sz1card1.Common.Communication;

namespace sz1card1.Common.Delegate
{
    /// <summary>
    /// 远程方法处理事件
    /// </summary>
    /// <param name="action">操作</param>
    /// <param name="body">参数</param>
    /// <returns>结果</returns>
    public delegate object[] RPCEventHandler(string sessionId, string action, object[] body);
}
