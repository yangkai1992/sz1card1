using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Iso8583Communication
{
    public enum Iso8583Property
    {
        /// <summary>
        /// 右靠，左边补0或空格（视数据类型而定）
        /// </summary>
        N,
        /// <summary>
        /// 左靠，右边补0或空格（视数据类型而定）
        /// </summary>
        ANS
    }
}
