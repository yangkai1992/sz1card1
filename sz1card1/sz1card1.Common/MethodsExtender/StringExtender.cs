///功能说明：提供将字符串加密、解密等扩展方法,转化成指定格式
///注意事项：
///修改记录：
///2009-06-17 [by pq] 新增对字符串数据转化为指定格式xml
///2009-06-25 [by pq] 新增字符串的DES加密解密
///2009-11-27 [by pq] 新增将汉字转化成Unicode编码和Unicode的反转化
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;

namespace sz1card1.Common
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtender
    {
        private static string desKey = "20090625";


        //编码后的字符集
        private static string base64EncodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        //对应ASICC字符的位置
        private static int[] base64DecodeChars = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1, -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1 };

        /// <summary>
        /// 将字符串进行MD5加密
        /// </summary>
        /// <param name="oldString">原字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string ToMD5(this string oldString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(oldString, "MD5");
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="pToEncrypt">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string ToDes(this string pToEncrypt)
        {
            return Utility.ToDes(pToEncrypt, desKey);
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="pToEncrypt">待加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string ToDes(this string pToEncrypt, string key)
        {
            return Utility.ToDes(pToEncrypt, key);
        }
        /// <summary>
        /// 全角字符及标点转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }
        public static string GuidsToWhereString(this string guids)
        {
            string[] strGuids = guids.Split(',');
            string returnStr = "";
            foreach (string str in strGuids)
            {
                if (!string.IsNullOrEmpty(str))
                    returnStr = returnStr + ",'" + str + "'";
            }
            return returnStr==""?returnStr:returnStr.Substring(1, returnStr.Length - 1);
        }
        public static string ToJsString(this string input)
        {
            StringBuilder sb = new StringBuilder();
            char[] CharJson = input.ToCharArray();
            if (CharJson.Length > 0)
            {
                foreach (char c in CharJson)
                {
                    switch (c)
                    {
                        case '\\':
                            sb.Append("\\\\");
                            break;
                        case '\"':
                            sb.Append("\\\"");
                            break;
                        case '/':
                            sb.Append("\\/");
                            break;
                        case '\b':
                            sb.Append("\\b");
                            break;
                        case '\f':
                            sb.Append("\\f");
                            break;
                        case '\n':
                            sb.Append("\\n");
                            break;
                        case '\r':
                            sb.Append("\\r");
                            break;
                        case '\t':
                            sb.Append("\\t");
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="pToDecrypt">待解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string FromDes(this string pToDecrypt)
        {
            return Utility.FromDes(pToDecrypt, desKey);
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="pToDecrypt">待解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string FromDes(this string pToDecrypt, string key)
        {
            return Utility.FromDes(pToDecrypt, key);
        }

        /// <summary>
        /// 将字符串数组转化为指定格式的xml字符串
        /// </summary>
        /// <param name="array">字符串数组</param>
        /// <param name="xpath">xpath路径(如:/ChainStores/ChainStore/@Id)</param>
        /// <returns>XML字符串</returns>
        public static string ToXML(this string[] array, string xpath)
        {
            Regex re = new Regex(@"/?(\w+)/(\w+)/@(\w+)");
            Match match = re.Match(xpath);
            if (!match.Success)
            {
                throw new ArgumentException("xpath");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                string root = match.Result("$1");
                string node = match.Result("$2");
                string attribute = match.Result("$3");
                if (array.Length == 0)
                {
                    return "<" + root + "/>";
                }
                sb.AppendLine(string.Format("<{0}>", root));
                foreach (string temp in array)
                {
                    sb.AppendLine(string.Format("<{0} {1}=\"{2}\"/>", node, attribute, temp));
                }
                sb.AppendLine(string.Format("</{0}>", root));
                return sb.ToString();
            }
        }

        /// <summary>
        /// 将字符串转化为Unicode编码格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnicode(this string str)
        {
			string result = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
				result = str;
				Regex rxChinaCharacter = new Regex("[\u4e00-\u9fa5]+");
				if (rxChinaCharacter.IsMatch(str)) {
					result = string.Empty;
					byte[] bytes = Encoding.Unicode.GetBytes(str);
					string temp1 = "", temp2 = "";
					for (int i = 0; i < bytes.Length; i++) {
						if (i % 2 == 0) {
							temp1 = Convert.ToString(bytes[i], 16);
							if (temp1.Length < 2) {
								temp1 = "0" + temp1;
							}
						} else {
							temp2 = Convert.ToString(bytes[i], 16);
							if (temp2.Length < 2) {
								temp2 = "0" + temp2;
							}
							result += "\\u" + temp2 + temp1;
						}
					}
				}
            }
            return result;
        }

        /// <summary>
        /// 将Unicode编码转化为字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromUnicode(this string str)
        {
            MatchCollection mc = Regex.Matches(str, "(\\\\u([\\w]{4}))");
            if (mc != null && mc.Count > 0)
            {
                foreach (Match m2 in mc)
                {
                    string v = m2.Value;
                    string word = v.Substring(2);
                    byte[] codes = new byte[2];
                    int code = Convert.ToInt32(word.Substring(0, 2), 16);
                    int code2 = Convert.ToInt32(word.Substring(2), 16);
                    codes[0] = (byte)code2;
                    codes[1] = (byte)code;
                    str = str.Replace(v, Encoding.Unicode.GetString(codes));
                }
            }
            return str;
        }

        /// <summary>
        /// 将字符Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encoder(this string str)
        {
            string Out = "";
            int i = 0, len = str.Length;
            char c1, c2, c3;
            while (i < len)
            {
                c1 = Convert.ToChar(str[i++] & 0xff);
                if (i == len)
                {
                    Out += base64EncodeChars[c1 >> 2];
                    Out += base64EncodeChars[(c1 & 0x3) << 4];
                    Out += "==";
                    break;
                }
                c2 = str[i++];
                if (i == len)
                {
                    Out += base64EncodeChars[c1 >> 2];
                    Out += base64EncodeChars[((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4)];
                    Out += base64EncodeChars[(c2 & 0xF) << 2];
                    Out += "=";
                    break;
                }
                c3 = str[i++];
                Out += base64EncodeChars[c1 >> 2];
                Out += base64EncodeChars[((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4)];
                Out += base64EncodeChars[((c2 & 0xF) << 2) | ((c3 & 0xC0) >> 6)];
                Out += base64EncodeChars[c3 & 0x3F];
            }
            return Out;
        }

        /// <summary>
        /// escape解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscape(this string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if (Uri.IsHexEncoding(str, i))
                {
                    sb.Append(Uri.HexUnescape(str, ref i));
                }
                else
                {
                    sb.Append(str[i]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将字符Base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decode(this string str)
        {
            int c1, c2, c3, c4;
            int i, len;
            string Out;
            len = str.Length;
            i = 0; Out = "";
            while (i < len)
            {
                do
                {
                    c1 = base64DecodeChars[str[i++] & 0xff];
                } while (i < len && c1 == -1);
                if (c1 == -1) break;
                do
                {
                    c2 = base64DecodeChars[str[i++] & 0xff];
                } while (i < len && c2 == -1);
                if (c2 == -1) break;
                Out += (char)((c1 << 2) | ((c2 & 0x30) >> 4));
                do
                {
                    c3 = str[i++] & 0xff;
                    if (c3 == 61) return Out;
                    c3 = base64DecodeChars[c3];
                } while (i < len && c3 == -1);
                if (c3 == -1) break;
                Out += (char)(((c2 & 0XF) << 4) | ((c3 & 0x3C) >> 2));
                do
                {
                    c4 = str[i++] & 0xff;
                    if (c4 == 61) return Out;
                    c4 = base64DecodeChars[c4];
                } while (i < len && c4 == -1);
                if (c4 == -1) break;
                Out += (char)(((c3 & 0x03) << 6) | c4);
            }
            return Out;
        }


        /// <summary>
        /// 将数字字符串转化为Url参数
        /// </summary>
        public static string ToShortUrl(this string str)
        {
            Int64 number = Convert.ToInt64(str);
            return Convert.ToString(number, 16);
        }

        /// <summary>
        /// 从Url参数还原数字字符串
        /// </summary>
        public static string FromShortUrl(this string str)
        {
            return Convert.ToInt64(str, 16).ToString();
        }

        /// <summary>
        /// 返回当天最大的时间(23:59:59)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMaxTimeString(this string str)
        {
            DateTime dt = DateTime.Parse(str);
            return dt.ToString("yyy-MM-dd") + " 23:59:59";
        }

        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToReverse(this string str)
        {
            StringBuilder sb = new StringBuilder();
            char[] cs = str.ToCharArray();
            for (int i = cs.Length - 1; i > -1; i--)
            {
                sb.Append(cs[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Base64编码转换成字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Binary FromBase64String(this string str)
        {
            if (str.Contains('%'))
            {
                return Convert.FromBase64String(HttpUtility.UrlDecode(str));
            }
            return Convert.FromBase64String(str);
        }

        /// <summary>
        /// 16进制字符串转换成字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Binary FromHexString(this string hexString)
        {
            if (hexString.Substring(0, 2).ToLower() == "0x")
            {
                hexString = hexString.Remove(0, 2);
            }
            if (hexString.Length % 2 != 0)
            {
                throw new FormatException("16进制字符串格式不正确!");
            }
            int len = (hexString.Length / 2);
            byte[] returnBytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary>
        /// 转换成16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes)
        {
            string res = "0x";
            if (bytes != null)
            {
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
        public static string ToBase64String(this byte[] bytes)
        {
            return HttpUtility.UrlEncode(Convert.ToBase64String(bytes));
        }

        /// <summary>
        ///  把html标签转换成常见符号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceTagChar(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            str = str.Replace("&amp;", "&");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&quot;", "\"");
            str = str.Replace("<br>", "\r\n");
            str = str.Replace("<br>", "\n");
            return str;
        }

        /// <summary>
        ///  转化sql语句中特殊的字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSqlCondtion(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            str = str.Replace("'", "''");
            str = str.Replace("[", "[[]");
            str = str.Replace("%;", "[%]");
            str = str.Replace("%", "[%]");
            str = str.Replace("_;", "[_]");
            str = str.Replace("^;", "[^]");
            return str;
        }

        /// <summary>
        /// 逗号分隔的字符串转换成List
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<Guid> FromGuidsList(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            string[] list = str.Split(',');
            List<Guid> guids = new List<Guid>();
            foreach (string item in list)
            {
                guids.Add(new Guid(item));
            }
            return guids;
        }

        /// <summary>
        /// 固定小数位数（不会四舍五入）
        /// </summary>
        /// <param name="money"></param>
        /// <param name="number">几位小数</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string money, int number)
        {
            if (number < 0)
            {
                number = 0;
            }
            if (!money.Contains("."))
            {
                return Convert.ToDecimal(money);
            }
            int start = money.IndexOf(".") + 1;
            if (money.Length - start > number)
            {
                return Convert.ToDecimal(money.Substring(0, start + number));
            }
            return Convert.ToDecimal(money);
        }
    }
}
