using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Delegate
{
    /// <summary>
    /// 远程方法调用后的回调
    /// </summary>
    /// <param name="obj">关联对象</param>
    /// <param name="result">结果</param>
    public delegate void RPCCallback(object obj,object[] result);
}
