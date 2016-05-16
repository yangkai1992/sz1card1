using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Delegate
{
    /// <summary>
    /// 异常处理事件
    /// </summary>
    /// <param name="ex">异常</param>
    /// <param name="obj">其他数据</param>
    public delegate void ExceptionEventHandler(Exception ex,object obj);
}
