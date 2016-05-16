using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// 短信运营商类型
    /// </summary>
    public enum SmsOperatorType
    {
        /// <summary>
        /// 不分运营商
        /// </summary>
        None = 0,

        /// <summary>
        /// 移动
        /// </summary>
        Mobile = 1,

        /// <summary>
        /// 联通
        /// </summary>
        Unicom = 2,

        /// <summary>
        /// 电信
        /// </summary>
        Telecom = 3
    }
}
