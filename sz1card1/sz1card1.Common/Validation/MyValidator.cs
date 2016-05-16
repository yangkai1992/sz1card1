using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace sz1card1.Common
{
    public static class MyValidator
    {
        /// <summary>
        /// 是否是numberic类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static bool IsGuid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            else
                return Regex.IsMatch(value, RegularExp.GUID);
        }
        public static bool IsMobile(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            else
                return Regex.IsMatch(value, RegularExp.Mobile);
        }
    }
}
