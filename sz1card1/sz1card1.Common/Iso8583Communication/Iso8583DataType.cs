using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Iso8583Communication
{
    /// <summary>
    /// 表示 ISO 8583 包的数据类型
    /// </summary>
    public enum Iso8583DataType
    {
        /// <summary>
        /// 二进制位
        /// </summary>
        B,
        /// <summary>
        /// BCD编码(每字节两位数字)，多余部分填零。若表示金额，无小数点符号，最后两位表示角分
        /// </summary>
        BCD,
        /// <summary>
        /// ASCII码，每字节一个，多余部分填空格
        /// </summary>
        AS,
        /// <summary>
        /// GBK码,每字节一个，多余部分填空格
        /// </summary>
        GBK
    }
}
