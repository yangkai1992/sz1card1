using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;

namespace sz1card1.Common
{
    /// <summary>
    /// 二进制类型扩展方法
    /// </summary>
    public static class BinaryExtender
    {
        /// <summary>
        /// 转换成16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(this Binary binary)
        {
            string res = "0x";
            if (binary != null)
            {
                byte[] bytes = binary.ToArray();
                for (int i = 0; i < bytes.Length; i++)
                {
                    res += bytes[i].ToString("X2");
                }
            }
            return res;
        }

        /// <summary>
        /// 转换成Base64编码的字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64String(this Binary binary)
        {
            return HttpUtility.UrlEncode(Convert.ToBase64String(binary.ToArray()));
        }
    }
}
