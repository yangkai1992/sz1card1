using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    public static class DecimalExtender
    {
        public static decimal ToDecimal(this decimal dec)
        {
            decimal endDecimal;
            if (dec.ToString().Split('.').Length > 1)
            {
                string decStrStart = dec.ToString().Split('.')[0];
                string decStrEnd = dec.ToString().Split('.')[1].Trim();
                for (int i = decStrEnd.Length - 1; i > -1; i--)
                {
                    if (decStrEnd[i] == '0')
                    {
                        decStrEnd = decStrEnd.Remove(i);
                    }
                    else
                    {
                        break;
                    }
                }
                endDecimal = Convert.ToDecimal(string.Format("{0:f2}", Convert.ToDecimal(decStrStart + "." + decStrEnd)));
            }
            else
            {
                endDecimal = Convert.ToDecimal(string.Format("{0:f2}", dec));
            }
            string decStrStart1 = endDecimal.ToString().Split('.')[0];
            string decStrEnd1 = endDecimal.ToString().Split('.')[1];
            for (int i = decStrEnd1.Length - 1; i > -1; i--)
            {
                if (decStrEnd1[i] == '0')
                {
                    decStrEnd1 = decStrEnd1.Remove(i);
                }
                else
                {
                    break;
                }
            }
            endDecimal = Convert.ToDecimal(decStrStart1 + "." + decStrEnd1);
            return endDecimal;
        }

        /// <summary>
        /// 保留小数点后多位
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="num">保留的数位</param>
        /// <returns></returns>
        public static decimal ToDecimal(this decimal dec, int num)
        {
            string format = @"{0:f" + num + @"}";
            decimal endDecimal;
            if (dec.ToString().Split('.').Length > 1)
            {
                string decStrStart = dec.ToString().Split('.')[0];
                string decStrEnd = dec.ToString().Split('.')[1].Trim();
                for (int i = decStrEnd.Length - 1; i > -1; i--)
                {
                    if (decStrEnd[i] == '0')
                    {
                        decStrEnd = decStrEnd.Remove(i);
                    }
                    else
                    {
                        break;
                    }
                }
                endDecimal = Convert.ToDecimal(string.Format(format, Convert.ToDecimal(decStrStart + "." + decStrEnd)));
            }
            else
            {
                endDecimal = Convert.ToDecimal(string.Format(format, dec));
            }
            string decStrStart1 = endDecimal.ToString().Split('.')[0];
            string decStrEnd1 = endDecimal.ToString().Split('.')[1];
            for (int i = decStrEnd1.Length - 1; i > -1; i--)
            {
                if (decStrEnd1[i] == '0')
                {
                    decStrEnd1 = decStrEnd1.Remove(i);
                }
                else
                {
                    break;
                }
            }
            endDecimal = Convert.ToDecimal(decStrStart1 + "." + decStrEnd1);
            return endDecimal;
        }
    }
}
