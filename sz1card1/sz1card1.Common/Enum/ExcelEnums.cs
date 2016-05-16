using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// 单元格的数据类型
    /// </summary>
    public enum ColDataType
    {
        /// <summary>
        /// 文本型
        /// </summary>
        String = 21,

        /// <summary>
        /// 数值型
        /// </summary>
        Number = 22,

        /// <summary>
        /// 日期型
        /// </summary>
        DateTime = 24,
    }

    /// <summary>
    /// 格式化类型
    /// </summary>
    public enum ExcelFormat
    {
        /// <summary>
        /// 二进制
        /// </summary>
        Binary,

        /// <summary>
        /// Xml格式
        /// </summary>
        Xml
    }
}
