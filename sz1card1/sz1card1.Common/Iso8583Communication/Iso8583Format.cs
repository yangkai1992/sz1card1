﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Iso8583Communication
{
    /// <summary>
    /// 表示 ISO 8583 包的字段格式
    /// </summary>
    public enum Iso8583Format
    {
        /// <summary>
        /// 普通定长域
        /// </summary>
        None,
        /// <summary>
        /// 可变长域的长度值（一位），占用1个字符。
        /// </summary>
        LVAR,
        /// <summary>
        /// 可变长域的长度值（二位），占用2个字符。
        /// </summary>
        LLVAR,
        /// <summary>
        /// 可变长域的长度值（三位），占用3个字符。
        /// </summary>
        LLLVAR,
        /// <summary>
        /// 年月日
        /// </summary>
        YYYYMMDD,
        /// <summary>
        /// 年月
        /// </summary>
        YYMM,
        /// <summary>
        /// 年
        /// </summary>
        YYYY,
        /// <summary>
        /// 月日
        /// </summary>
        MMDD,
        /// <summary>
        /// 时分秒
        /// </summary>
        hhmmss,
        /// <summary>
        /// 月日时分秒
        /// </summary>
        MMDDhhmmss,
    }
}
