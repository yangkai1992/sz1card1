///<summary>
///Copyright (C) 深圳市一卡易科技发展有限公司
///创建标识：2009-09-02 Created by pq
///功能说明：提供公历农历互相转化的扩展方法类
///注意事项：转化成的阴历也为日期格式
///修改记录：
///2009-09-02 [by pq]
///</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace sz1card1.Common
{
    public static class DateTimeExtender
    {

        /// <summary>
        /// 转化为公历
        /// </summary>
        /// <param name="lunarDate">农历日期</param>
        /// <returns>公历日期</returns>
        public static DateTime ToSolarDate(this DateTime lunarDate)
        {
            ChineseLunisolarCalendar c = new ChineseLunisolarCalendar();
            int year, month, day;
            year = lunarDate.Year;
            month = lunarDate.Month;
            if (c.IsLeapYear(year) && c.GetLeapMonth(year) < month)
            {
                month++;
            }
            day = lunarDate.Day;
            return c.ToDateTime(year, month, day, 0, 0, 0, 0);
        }

        /// <summary>
        /// 转化为农历
        /// </summary>
        /// <param name="lunarDate">公历日期农历日期</param>
        /// <returns>农历日期</returns>
        public static DateTime ToLunarDate(this DateTime solarDate)
        {
            ChineseLunisolarCalendar c = new ChineseLunisolarCalendar();
            int year, month, day;
            year = c.GetYear(solarDate);
            month = c.GetMonth(solarDate);
            if (c.IsLeapYear(year) && c.GetLeapMonth(year) < month)
            {
                month--;
            }
            day = c.GetDayOfMonth(solarDate);
            try
            {
                return new DateTime(year, month, day);
            }
            catch
            {
                return new DateTime(year, month, day-1);
            }
        }

        /// <summary>
        /// 格式化为自定义格式（如：3个小时前，昨天，前天）
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToCustomString(this DateTime datetime)
        {
            string result = "";
            TimeSpan timeSpan = datetime - DateTime.Now;
            if (timeSpan <= new TimeSpan(24, 0, 0) && timeSpan > new TimeSpan(1, 0, 0))
            {
                result = string.Format("{0}小时后", Math.Abs(timeSpan.Hours));
            }
            else if (timeSpan <= new TimeSpan(1, 0, 0) && timeSpan > new TimeSpan(0, 1, 0))
            {
                result = string.Format("{0}分钟后", Math.Abs(timeSpan.Minutes));
            }
            else if (timeSpan <= new TimeSpan(0, 1, 0) && timeSpan > new TimeSpan(0, 0, 0))
            {
                result = string.Format("{0}秒后", Math.Abs(timeSpan.Seconds));
            }
            else if (timeSpan <= new TimeSpan(0, 0, 0) && timeSpan > new TimeSpan(0, -1, 0))
            {
                result = string.Format("{0}秒前", Math.Abs(timeSpan.Seconds));
            }
            else if (timeSpan <= new TimeSpan(0, -1, 0) && timeSpan > new TimeSpan(-1, 0, 0))
            {
                result = string.Format("{0}分钟前", Math.Abs(timeSpan.Minutes));
            }
            else if (timeSpan <= new TimeSpan(-1, 0, 0) && timeSpan >= new TimeSpan(-24, 0, 0))
            {
                result = string.Format("{0}小时前", Math.Abs(timeSpan.Hours));
            }
            else if (timeSpan < new TimeSpan(-24, 0, 0) || timeSpan > new TimeSpan(24, 0, 0))
            {
                TimeSpan dateSpan = datetime.Date - DateTime.Now.Date;
                if (dateSpan <= new TimeSpan(2, 0, 0, 0) && dateSpan > new TimeSpan(1, 0, 0, 0))
                {
                    result = "后天 " + datetime.ToShortTimeString().ToString();
                }
                else if (dateSpan <= new TimeSpan(1, 0, 0, 0) && dateSpan > new TimeSpan(0, 0, 0, 0))
                {
                    result = "明天 " + datetime.ToShortTimeString().ToString();
                }
                else if (dateSpan < new TimeSpan(0, 0, 0, 0) && dateSpan >= new TimeSpan(-1, 0, 0, 0))
                {
                    result = "昨天 " + datetime.ToShortTimeString().ToString();
                }
                else if (dateSpan < new TimeSpan(-1, 0, 0, 0) && dateSpan >= new TimeSpan(-2, 0, 0, 0))
                {
                    result = "前天 " + datetime.ToShortTimeString().ToString();
                }
                else if (datetime.Year != DateTime.Now.Year)
                {
                    result = datetime.ToLongDateString().ToString();
                }
                else if (dateSpan > new TimeSpan(2, 0, 0, 0) || dateSpan < new TimeSpan(-2, 0, 0, 0))
                {
                    result = datetime.GetDateTimeFormats('M')[0].ToString() + " " + datetime.ToShortTimeString().ToString(); ;
                }
            }
            else
            {
                result = datetime.ToLongDateString().ToString();
            }
            return result;
        }

        public static DateTime  GetFirstDateOfMonth(this DateTime datetime)
        {
           return  datetime.AddDays(-datetime.Day + 1).Date;
        }

        public static DateTime GetLastDateOfMonth(this DateTime datetime)
        {
            return datetime.AddMonths(1).AddDays(-datetime.Day).Date;
        }
    }
}
