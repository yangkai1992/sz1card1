using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace sz1card1.Common.Birthday
{
    public class BirhdayProvider
    {
        private ChineseLunisolarCalendar calendar;
        private int[] lunarInfo = new int[]{0x04bd8, 0x04ae0, 0x0a570, 0x054d5, 0x0d260, 0x0d950, 0x16554, 0x056a0, 0x09ad0, 0x055d2, 0x04ae0, 0x0a5b6, 0x0a4d0, 0x0d250, 0x1d255, 0x0b540, 0x0d6a0, 0x0ada2, 0x095b0,
        0x14977, 0x04970, 0x0a4b0, 0x0b4b5, 0x06a50, 0x06d40, 0x1ab54, 0x02b60, 0x09570, 0x052f2, 0x04970, 0x06566, 0x0d4a0, 0x0ea50, 0x06e95, 0x05ad0, 0x02b60, 0x186e3, 0x092e0, 0x1c8d7, 0x0c950,
        0x0d4a0, 0x1d8a6, 0x0b550, 0x056a0, 0x1a5b4, 0x025d0, 0x092d0, 0x0d2b2, 0x0a950, 0x0b557, 0x06ca0, 0x0b550, 0x15355, 0x04da0, 0x0a5d0, 0x14573, 0x052d0, 0x0a9a8, 0x0e950, 0x06aa0, 0x0aea6,
        0x0ab50, 0x04b60, 0x0aae4, 0x0a570, 0x05260, 0x0f263, 0x0d950, 0x05b57, 0x056a0, 0x096d0, 0x04dd5, 0x04ad0, 0x0a4d0, 0x0d4d4, 0x0d250, 0x0d558, 0x0b540, 0x0b5a0, 0x195a6, 0x095b0, 0x049b0,
        0x0a974, 0x0a4b0, 0x0b27a, 0x06a50, 0x06d40, 0x0af46, 0x0ab60, 0x09570, 0x04af5, 0x04970, 0x064b0, 0x074a3, 0x0ea50, 0x06b58, 0x055c0, 0x0ab60, 0x096d5, 0x092e0, 0x0c960, 0x0d954, 0x0d4a0,
        0x0da50, 0x07552, 0x056a0, 0x0abb7, 0x025d0, 0x092d0, 0x0cab5, 0x0a950, 0x0b4a0, 0x0baa4, 0x0ad50, 0x055d9, 0x04ba0, 0x0a5b0, 0x15176, 0x052b0, 0x0a930, 0x07954, 0x06aa0, 0x0ad50, 0x05b52,
        0x04b60, 0x0a6e6, 0x0a4e0, 0x0d260, 0x0ea65, 0x0d530, 0x05aa0, 0x076a3, 0x096d0, 0x04bd7, 0x04ad0, 0x0a4d0, 0x1d0b6, 0x0d250, 0x0d520, 0x0dd45, 0x0b5a0, 0x056d0, 0x055b2, 0x049b0, 0x0a577,
        0x0a4b0, 0x0aa50, 0x1b255, 0x06d20, 0x0ada0};
        public BirhdayProvider()
        {
            calendar = new ChineseLunisolarCalendar();
        }

        /// <summary>
        /// 返回生日，如果该年存在闰月，且生日月份晚于闰月，则减一（）
        /// 此方法供输出用。因为在写数据库时，已经加一（即存在13月）
        /// </summary>
        /// <param name="birthtime"></param>
        /// <returns></returns>
        public string GetBirthForExport(string birthtime)
        {
            try
            {
                if (birthtime.IndexOf(":") == -1)
                    return birthtime;
                string isLunar = birthtime.Split(':')[1];
                if (isLunar == "0")
                    return birthtime.Split(':')[0];
                string birthday = birthtime.Split(':')[0];
                int year = int.Parse(birthday.Split('-')[0]);
                int month = int.Parse(birthday.Split('-')[1]);
                int day = int.Parse(birthday.Split('-')[2]);
                int leapMonth = (lunarInfo[year - 1900] & 0xf);
                if (leapMonth > 0 && month > leapMonth)
                {
                    month = month - 1;
                }
                return string.Format("{0}-{1}-{2}", year, month, day);
            }
            catch (Exception ex)
            {
                //sz1card1.Common.Log.LoggingService.Warn("返回生日错误!" + birthtime + ex.Message + ex.StackTrace);
                return "";
            }
        }

        /// <summary>
        /// 普通 返回正确生日时间,如果是该年包括闰月，则加一月
        /// </summary>
        /// <param name="birthtime"></param>
        /// <param name="righttime"></param>
        /// <returns></returns>
        public string GetRightBirthday(string birthtime)
        {
            try
            {
                if (birthtime.IndexOf(":") == -1)
                    return birthtime;
                string isLunar = birthtime.Split(':')[1];
                if (isLunar == "0")
                    return birthtime;
                string birthday = birthtime.Split(':')[0];
                int year = int.Parse(birthday.Split('-')[0]);
                int month = int.Parse(birthday.Split('-')[1]);
                int day = int.Parse(birthday.Split('-')[2]);
                int leapMonth = (lunarInfo[year - 1900] & 0xf);
                if (leapMonth > 0 && month > leapMonth)
                {
                    month = month + 1;
                }
                return string.Format("{0}-{1}-{2}:1", year, month, day);
            }
            catch (Exception ex)
            {
                //sz1card1.Common.Log.LoggingService.Warn("返回生日错误!"+ birthtime+ ex.Message + ex.StackTrace);
                return "";
            }
        }
        public bool CheckBirthdayRight(string birthtime, out string message)
        {
            message = string.Empty;
            try
            {
                string birthday = birthtime.Split(':')[0];
                int year = int.Parse(birthday.Split('-')[0]);
                int month = int.Parse(birthday.Split('-')[1]);
                int day = int.Parse(birthday.Split('-')[2]);
                bool isLunar = Convert.ToBoolean(birthtime.Split(':')[1] == "1" ? "true" : "false");
                //判断年
                if (year > DateTime.Now.Year || year < 1900)
                {
                    message = "生日年份只支持1900年到" + DateTime.Now.Year + "年之间";
                    return false;
                }
                if (!isLunar)
                {
                    DateTime reDateTime;
                    if (!DateTime.TryParse(birthday, out reDateTime))
                    {
                        message = "该阳历生日不是一个合法的时间！";
                        return false;
                    }
                }
                //判断月
                if (isLunar)
                {
                    if (month > 13 || month < 1)
                    {
                        message = "农历生日月份值只支持1到13之间！";
                        return false;
                    }
                }
                //判断日
                if (isLunar)
                {
                    int leapMonth = (lunarInfo[year - 1900] & 0xf);
                    if (leapMonth > 0 && month == leapMonth + 1)
                    {
                        int leapDayMonth = (lunarInfo[year - 1900] & 0x10000) > 0 ? 30 : 29;
                        if (day > leapDayMonth)
                        {
                            message = "该年农历闰" + leapMonth + "月不支持" + day + "天";
                            return false;
                        }
                        return true;
                    }
                    if (leapMonth > 0 && month > leapMonth)
                    {
                        month = month - 1;
                    }
                    int thisMonthdays = (lunarInfo[year - 1900] & (0x10000 >> month)) != 0 ? 30 : 29;
                    if (day > thisMonthdays)
                    {
                        message = "该年农历" + month + "月不支持" + day + "天";
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //sz1card1.Common.Log.LoggingService.Warn(string.Format("生日不合法:{0}", birthtime));
                message = "生日不合法!";
                return false;
            }
        }

        public bool CheckBirthdayRight(string birthday, int isLunar, out string message)
        {
            return CheckBirthdayRight(birthday + ":" + isLunar, out message);
        }

        public bool CheckBirthdayRight(string birthday, bool isLunar, out string message)
        {
            return CheckBirthdayRight(birthday + ":" + (isLunar ? "1" : "0"), out message);
        }
    }
}
