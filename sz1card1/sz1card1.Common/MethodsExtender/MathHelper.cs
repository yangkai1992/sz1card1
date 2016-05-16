using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    public static class MathHelper
    {
        /// <summary>
        /// 数值小数处理
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flag">消费积分小数位类型（参考系统参数）</param>
        /// <returns></returns>
        public static decimal toMath(this decimal value, string flag)
        {
            decimal globalValue;
            switch (flag.Trim())
            {
                //case "小数取整":
                //    globalValue = Math.Floor(value);
                //    break;
                //case "四舍五入":
                //    globalValue = Math.Round(value, MidpointRounding.AwayFromZero);
                //    break;
                //case "保留1位":
                //    globalValue = Convert.ToDecimal(value.ToString("#0.0"));
                //    break;
                //case "保留2位":
                //    globalValue = Convert.ToDecimal(value.ToString("#0.00"));
                //    break;
                //case "保留3位":
                //    globalValue = Convert.ToDecimal(value.ToString("#0.000"));
                //    break;
                //case "保留4位":
                //    globalValue = Convert.ToDecimal(value.ToString("#0.0000"));
                    //break;
                case "四舍五入到元":
                    globalValue = Math.Round(value, MidpointRounding.AwayFromZero);
                    break;
                case "四舍五入到角":
                    globalValue = Convert.ToDecimal(value.ToString("#0.0"));
                    break;
                case "四舍五入到分":
                    globalValue = Convert.ToDecimal(value.ToString("#0.00"));
                    break;
                case "抹零到元":
                    globalValue = Math.Floor(value);
                    break;
                case "抹零到角":
                    globalValue = Math.Floor(value * 10) / 10;
                    break;
                case "保留两位小数":
                    globalValue = Convert.ToDecimal(value.ToString("#0.00"));
                    break;
                case "抹零取整":
                    globalValue = Math.Floor(value);
                    break;
                case "四舍五入取整":
                    globalValue = Math.Round(value, MidpointRounding.AwayFromZero);
                    break;
                case "保留四位小数":
                    globalValue = Convert.ToDecimal(value.ToString("0.####"));
                    break;
                default:
                    globalValue = Convert.ToDecimal(value.ToString("#0.00"));
                    break;
            }
            return globalValue;
        }



    }


   
  


}
