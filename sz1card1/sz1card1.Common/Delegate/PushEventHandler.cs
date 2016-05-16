using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Delegate
{
    /// <summary>
    /// 服务器推消息处理事件
    /// </summary>
    /// <param name="action">操作</param>
    /// <param name="body">参数</param>
    public delegate void PushEventHandler(string action,object[] body);
}
