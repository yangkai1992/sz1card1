using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// 分页模式
    /// </summary>
    public enum PageMode
    {
        /// <summary>
        /// 查询字符串模式
        /// </summary>
        QueryString,

        /// <summary>
        /// 回传模式
        /// </summary>
        Postback,

        /// <summary>
        /// 异步模式
        /// </summary>
        Ajax
    }
}
