using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using System.Web;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Windows.Forms;
using sz1card1.Common.Iso8583Communication;
using System.Xml.Serialization;
using System.Xml;
using sz1card1.Common.FileServer;
using System.Configuration;
using System.Collections.Specialized;

namespace sz1card1.Common
{
    public class Utility
    {
        public static bool CheckWTIp(string inIp, string ipDetailTxtPaht)
        {
            string[] InIpAtrr = inIp.Split('.');
            int InIpAtrr1 = Convert.ToInt32(InIpAtrr[0]);
            int InIpAtrr2 = Convert.ToInt32(InIpAtrr[1]);
            int InIpAtrr3 = Convert.ToInt32(InIpAtrr[2]);
            using (StreamReader sr = new StreamReader(ipDetailTxtPaht))
            {
                string sline = string.Empty;

                while (sline != null)
                {
                    sline = sr.ReadLine();
                    try
                    {
                        string[] arr = sline.Split('/')[0].Split('.');
                        string[] ttIpArr = sline.Split('/')[1].Split('.');
                        int ttIp1 = Convert.ToInt32(ttIpArr[0]);   //掩码第一节
                        int ttIp2 = Convert.ToInt32(ttIpArr[1]);   //掩码第二节
                        int ttIp3 = Convert.ToInt32(ttIpArr[2]);   //掩码第三节


                        int Ip1 = Convert.ToInt32(arr[0]);
                        int Ip2 = Convert.ToInt32(arr[1]);
                        int Ip3 = Convert.ToInt32(arr[2]);
                        int Ip1_ttIp = InIpAtrr1 & ttIp1;
                        int Ip2_ttIp = InIpAtrr2 & ttIp2;
                        int Ip3_ttIp = InIpAtrr3 & ttIp3;


                        if ((Ip1 == Ip1_ttIp) && (Ip2 == Ip2_ttIp) && (Ip3 == Ip3_ttIp))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            return false;
        }
        public static DateTime DayToDateTime(DateTime dt)
        {
            DateTime nowTime = DateTime.Now;
            DateTime realTime = Convert.ToDateTime(dt.ToString("HH:mm"));
            if (DateTime.Compare(nowTime, realTime) > 0)
            {
                return Convert.ToDateTime(nowTime.ToString("yyyy-MM-dd") + " " + dt.ToString("HH:mm")).AddDays(1);
            }
            return Convert.ToDateTime(nowTime.ToString("yyyy-MM-dd") + " " + dt.ToString("HH:mm"));
        }
        /// <summary>
        /// 用于计算不同经纬度间的距离的参数和方法
        /// </summary>
        private const double EARTH_RADIUS = 6378.137;
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// 计算两点之间的距离
        /// </summary>
        /// <param name="Lat1">起点的经度</param>
        /// <param name="Lng1">起点的纬度</param>
        /// <param name="Lat2">终点的经度</param>
        /// <param name="Lng2">终点的纬度</param>
        /// <returns>返回距离，以千米为单位</returns>
        public static double CalDistance(double Lat1, double Lng1, double Lat2, double Lng2)
        {
            double radLat1 = rad(Lat1);
            double radLat2 = rad(Lat2);
            double a = radLat1 - radLat2;
            double b = rad(Lng1) - rad(Lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        /// <summary>
        /// 获取更新日期 
        /// </summary>
        /// <param name="Week">每周的星期几</param>
        /// <param name="dt">小时与分钟</param>
        /// <returns></returns>
        public static DateTime WeekToDateTime(int Week, DateTime dt)
        {
            DateTime nowTime = DateTime.Now;
            int[] Day = new int[] { 1, 2, 3, 4, 5, 6, 7 };
            int nowWeek = Day[Convert.ToInt32(nowTime.DayOfWeek)];
            DateTime time;
            if (Week < nowWeek)
            {
                time = Convert.ToDateTime(nowTime.AddDays((7 + Week) - nowWeek));

            }
            else
            {
                time = Convert.ToDateTime(nowTime.AddDays(Week - nowWeek));
            }
            return Convert.ToDateTime(time.ToString("yyyy-MM-dd") + " " + dt.ToString("HH:mm"));
        }

        public static DateTime MonthToDateTime(string day, DateTime dt)
        {
            DateTime nowTime = DateTime.Now;
            DateTime realTime = Convert.ToDateTime(nowTime.Year + "-" + nowTime.Month + "-" + day + " " + dt.ToString("HH:mm"));
            if (DateTime.Compare(nowTime, realTime) > 0)
            {
                return Convert.ToDateTime(nowTime.Year + "-" + nowTime.Month + "-" + day + " " + dt.ToString("HH:mm")).AddMonths(1);
            }
            return Convert.ToDateTime(nowTime.Year + "-" + nowTime.Month + "-" + day + " " + dt.ToString("HH:mm"));
        }
        public static DateTime YearToDateTime(DateTime Month, DateTime dt)
        {
            DateTime nowTime = DateTime.Now;
            DateTime realTime = Convert.ToDateTime(nowTime.Year + "-" + Month.ToString("MM-dd") + " " + dt.ToString("HH:mm"));
            if (DateTime.Compare(nowTime, realTime) > 0)
            {
                return Convert.ToDateTime(nowTime.Year + "-" + Month.ToString("MM-dd") + " " + dt.ToString("HH:mm")).AddYears(1);
            }
            return Convert.ToDateTime(nowTime.Year + "-" + Month.ToString("MM-dd") + " " + dt.ToString("HH:mm"));
        }
        public static DateTime GetExecuteTime(int typeId, string dt)
        {
            DateTime resultTime = DateTime.Now;
            if (dt.Contains(","))
            {
                Regex rg = new Regex("(.*),(.*)");
                Match mt = rg.Match(dt);
                if (mt.Success)
                {
                    resultTime = WeekToDateTime(Convert.ToInt32(mt.Result("$1")), Convert.ToDateTime(mt.Result("$2")));
                }
            }
            else
            {
                Regex rg = new Regex("(.*) (.*)");
                Match mt = rg.Match(dt);
                if (mt.Success)
                {
                    switch (typeId)
                    {
                        case 0:
                            resultTime = Convert.ToDateTime(dt);
                            break;
                        case 3:
                            if (DateTime.TryParse(dt, out resultTime) == false)
                                resultTime = MonthToDateTime(mt.Result("$1"), Convert.ToDateTime(mt.Result("$2")));
                            break;
                        case 4:
                            resultTime = YearToDateTime(Convert.ToDateTime(mt.Result("$1")), Convert.ToDateTime(mt.Result("$2")));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Regex rg1 = new Regex(@"\d{2}:\d{2}");
                    Match mt1 = rg1.Match(dt);
                    if (mt1.Success)
                    {
                        resultTime = Utility.DayToDateTime(Convert.ToDateTime(dt));
                    }
                }
            }
            return resultTime;
        }
        /// <summary>
        /// 获取不同时间类型格式
        /// </summary>
        /// <param name="typeId">类型</param>
        /// <param name="dt">时间</param>
        /// <returns></returns>
        public static string GetDateTime(int typeId, DateTime dt)
        {
            string dateString = string.Empty;
            switch (typeId)
            {
                case 0:
                    dateString = dt.ToString("yyyy-MM-dd HH:mm");
                    break;
                case 1:
                    dateString = dt.ToString("HH:mm");
                    break;
                case 2:
                    int[] Day = new int[] { 1, 2, 3, 4, 5, 6, 7 };
                    dateString = Day[Convert.ToInt16(dt.DayOfWeek)] + "," + dt.ToString("HH:mm");
                    break;
                case 3:
                    dateString = dt.ToString("dd HH:mm");
                    break;
                case 4:
                    dateString = dt.ToString("MM-dd HH:mm");
                    break;
                default:
                    break;

            };
            return dateString;
        }
        public static string GetDateTimeString(int typeId, DateTime dt)
        {
            string dateString = string.Empty;
            switch (typeId)
            {
                case 0:
                    dateString = dt.ToString("yyyy-MM-dd HH:mm");
                    break;
                case 1:
                    dateString = "每天" + dt.Hour.ToString() + "时" + dt.Minute.ToString() + "分";
                    break;
                case 2:
                    string[] Day = new string[] { "日", "一", "二", "三", "四", "五", "六" };
                    dateString = "每周" + Day[Convert.ToInt16(dt.DayOfWeek)] + dt.Hour.ToString() + "时" + dt.Minute.ToString() + "分";
                    break;
                case 3:
                    dateString = "每月" + dt.Day.ToString() + "日" + dt.Hour.ToString() + "时" + dt.Minute.ToString() + "分";
                    break;
                case 4:
                    dateString = "每年" + dt.Month.ToString() + "月" + dt.Day.ToString() + "日" + dt.Hour.ToString() + "时" + dt.Minute.ToString() + "分";
                    break;
                default:
                    break;

            };
            return dateString;
        }

        /// <summary>
        /// 根据对象生成Post数据
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static string BuildPostParams(Encoding encoding, params object[] objects)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object obj in objects)
            {
                if (obj is ICollection)
                {
                    foreach (object subObj in (ICollection)obj)
                    {
                        sb.Append(BuildPostParams(encoding, subObj) + "&");
                    }
                }
                else if (obj.GetType() == typeof(KeyValuePair<string, string>))
                {
                    KeyValuePair<string, string> keyValue = (KeyValuePair<string, string>)obj;
                    sb.Append(string.Format("{0}={1}&", keyValue.Key, HttpUtility.UrlEncode(keyValue.Value, encoding)));
                }
                else if (obj.GetType().GetCustomAttributes(typeof(DataContractAttribute), true).Length > 0)
                {
                    foreach (PropertyInfo pi in obj.GetType().GetProperties())
                    {
                        if (pi.GetCustomAttributes(typeof(DataMemberAttribute), true).Length > 0)
                        {
                            if (pi.GetValue(obj, null) != null)
                            {
                                sb.Append(string.Format("{0}={1}&", pi.Name, HttpUtility.UrlEncode(pi.GetValue(obj, null).ToString(), encoding)));
                            }
                        }
                    }
                }
            }
            if (sb.Length > 0)
            {
                sb = sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public static byte[] bs;

        /// <summary>
        /// 自定义同步http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static string PostDataSync(string url, params object[] objects)
        {
            return PostDataSync(url, Encoding.GetEncoding("GB2312"), objects);
        }

        public static string PostDataSync(string url, Encoding requestEncoding, Encoding responseEncoding, string paramsString)
        {
            return PostDataSync(url, "application/x-www-form-urlencoded", requestEncoding, responseEncoding, paramsString);
        }


        public static string PostDataSync(string url, string contentType, Encoding requestEncoding,
            Encoding responseEncoding, string paramsString)
        {
            if (url == "")
                throw new ArgumentNullException("url");
            string result;
            byte[] bresult = requestEncoding.GetBytes(paramsString);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.ServicePoint.ConnectionLimit = 512;
            webRequest.Method = "POST";
            webRequest.ContentType = contentType;
            webRequest.ContentLength = bresult.Length;
            webRequest.KeepAlive = false;
            webRequest.Proxy = null;
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bresult, 0, bresult.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream, responseEncoding))
            {
                result = reader.ReadToEnd();
            }
            response.Close();
            return result;
        }

        /// <summary>
        /// 自定义同步http请求GB2312编码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramsString"></param>
        /// <returns></returns>
        public static string PostDataSync(string url, string paramsString)
        {
            return PostDataSync(url, Encoding.GetEncoding("GB2312"), paramsString);
        }

        /// <summary>
        /// 自定义HTTP 请求，不等待返回值
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static string PostDataNoWait(string url, Encoding encoding, params object[] objects)
        {
            string paramsString = BuildPostParams(encoding, objects);
            string contentType = "application/x-www-form-urlencoded";
            Encoding requestEncoding = encoding;
            Encoding responseEncoding = encoding;
            if (url == "")
                throw new ArgumentNullException("url");
            string result;
            byte[] bresult = requestEncoding.GetBytes(paramsString);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.ServicePoint.ConnectionLimit = 512;
            webRequest.Method = "POST";
            webRequest.ContentType = contentType;
            webRequest.ContentLength = bresult.Length;
            webRequest.KeepAlive = false;
            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(bresult, 0, bresult.Length);
                requestStream.Close();
            }
            webRequest.Abort();
            result = "提交成功！";
            return result;
        }

        /// <summary>
        /// 自定义同步http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static string PostDataSync(string url, Encoding encoding, params object[] objects)
        {
            string paramsString = BuildPostParams(encoding, objects);
            return PostDataSync(url, encoding, encoding, paramsString);
        }

        /// <summary>
        /// 自定义异步http请求
        /// </summary>
        /// <param name="url">url</param>
        ///<param name="objects">待传递的数据对象</param>
        public static void PostData(string url, params object[] objects)
        {
            if (url == "")
                throw new ArgumentNullException("url");
            string data = BuildPostParams(Encoding.GetEncoding("GB2312"), objects);
            bs = Encoding.UTF8.GetBytes(data);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.ServicePoint.ConnectionLimit = 512;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = bs.Length;
            try
            {
                webRequest.BeginGetRequestStream(new AsyncCallback(PostDataCallBack), webRequest);
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn("【异步http请求的出错url[" + url + "]\n\n[出错原因]（" + ex.Message + "）】");
            }

        }

        private static void PostDataCallBack(IAsyncResult result)
        {
            HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
            Stream requestStream = webRequest.EndGetRequestStream(result);
            requestStream.Write(bs, 0, bs.Length);
            requestStream.Close();
            try
            {
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        sz1card1.Common.Log.LoggingService.Warn("【异步请求返回值" + sr.ReadToEnd() + "】");
                    }
                }
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn("【异步请求返回出错，出错原因（" + ex.Message + "）】");
            }
        }

        /// <summary>
        /// 获取当前http请求的客户端IP
        /// </summary>
        /// <param name="request">http请求</param>
        /// <returns>IP地址</returns>
        public static string GetIPAddress(HttpRequest request)
        {
            string ip = "";

            if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
            }
            else
            {
                ip = request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;
        }

        /// <summary>
        /// 获取当前http请求的客户端IP
        /// </summary>
        /// <returns>IP地址</returns>
        public static string GetIPAddress()
        {
            if (HttpContext.Current != null)
                return GetIPAddress(HttpContext.Current.Request);
            else
                return "";
        }

        /// <summary>
        /// 生成批量延期提示/wzq
        /// </summary>
        /// <param name="filter">过滤器获得字符串</param>
        /// <returns>提示语</returns>
        public static string BatchExt(string filter)
        {
            string result = string.Empty;
            string[] columns = filter.Split('$');
            foreach (var item in columns)
            {
                string[] keys = item.Split('|');
                switch (keys[0])
                {
                    case "Mobile":
                        result += string.Format("手机号包含{0};", keys[2]);
                        break;
                    case "Title":
                        result += string.Format("电子券标题包含{0};", keys[2]);
                        break;
                    case "CouponCode":
                        result += string.Format("券号包含{0};", keys[2]);
                        break;
                    case "EndDate":
                        switch (keys[2])
                        {
                            case "eq":
                                result += string.Format("电子券有效期为{0};", keys[3]);
                                break;
                            case "lt":
                                result += string.Format("电子券有效期起始日期为{0};", keys[3]);
                                break;
                            case "gt":
                                result += string.Format("电子券有效期终止日期为{0};", keys[3]);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "OperateTime":
                        switch (keys[2])
                        {
                            case "eq":
                                result += string.Format("电子券发送时间为{0};", keys[3]);
                                break;
                            case "lt":
                                result += string.Format("电子券发送日期起始日期为{0};", keys[3]);
                                break;
                            case "gt":
                                result += string.Format("电子券发送日期终止日期为{0};", keys[3]);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "SendCount":
                        switch (keys[2])
                        {
                            case "eq":
                                result += string.Format("发送数量为{0};", keys[3]);
                                break;
                            case "lt":
                                result += string.Format("发送数量大于{0};", keys[3]);
                                break;
                            case "gt":
                                result += string.Format("发送数量小于{0};", keys[3]);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "UsedCount":
                        switch (keys[2])
                        {
                            case "eq":
                                result += string.Format("使用数量为{0};", keys[3]);
                                break;
                            case "lt":
                                result += string.Format("使用数量大于{0};", keys[3]);
                                break;
                            case "gt":
                                result += string.Format("使用数量小于{0};", keys[3]);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "UserAccount":
                        result += string.Format("操作者包含{0};", keys[2]);
                        break;
                    case "TrueName":
                        result += string.Format("接收者包含{0};", keys[2]);
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 生成筛选条件查询语句
        /// </summary>
        /// <param name="filter">过滤器获得字符串</param>
        /// <returns>过滤语句</returns>
        public static string DecodeQuery(string filter)
        {
            string result = "";
            string[] colunms = filter.Split('$');
            for (int i = 0; i < colunms.Length; i++)
            {
                string[] keys = colunms[i].Split('|');
                switch (keys[1])
                {
                    case "string":
                        result += string.Format(" AND {0} LIKE '%{1}%'", keys[0], keys[2].ToSqlCondtion());
                        break;
                    case "boolean":
                        if (keys[2] != "0")
                        {
                            result += string.Format(" AND {0}={1}", keys[0], keys[2] == "true" ? 1 : 0);
                        }
                        break;
                    case "date":
                        switch (keys[2])
                        {
                            case "eq":
                                if (keys[0].Contains("ExtValue"))
                                {
                                    result += string.Format(" AND DATEDIFF(dd,DATEADD(YEAR, DATEDIFF(YEAR, CAST((YEAR(GETDATE()) - 1) AS NVARCHAR(10))+'-'+{0}, GETDATE()), CAST((YEAR(GETDATE()) - 1) AS NVARCHAR(10))+'-'+{0}),'{1}')=0", keys[0], keys[3]);
                                }
                                else
                                {
                                    result += string.Format(" AND DATEDIFF(dd,{0},'{1}')=0", keys[0], keys[3]);
                                }
                                break;
                            case "lt":
                                if (keys[0].Contains("ExtValue"))
                                {
                                    result += string.Format(" AND DATEDIFF(dd,DATEADD(YEAR, DATEDIFF(YEAR, CAST((YEAR(GETDATE()) - 1) AS NVARCHAR(10))+'-'+{0}, GETDATE()), CAST((YEAR(GETDATE()) - 1) AS NVARCHAR(10))+'-'+{0}),'{1}')<=0 ", keys[0], keys[3]);
                                }
                                else
                                {
                                    result += string.Format(" AND (DATEDIFF(dd,{0},'{1}')<=0 OR {0} IS NULL)", keys[0], keys[3]);
                                }
                                break;
                            case "gt":
                                if (keys[0].Contains("ExtValue"))
                                {
                                    result += string.Format(" AND DATEDIFF(dd,DATEADD(YEAR, DATEDIFF(YEAR, CAST((YEAR(GETDATE()) - 1) AS NVARCHAR(10))+'-'+{0}, GETDATE()), CAST((YEAR(GETDATE()) - 1) AS NVARCHAR(10))+'-'+{0}),'{1}') >=0 ", keys[0], keys[3]);
                                }
                                else
                                {
                                    result += string.Format(" AND DATEDIFF(dd,{0},'{1}')>=0", keys[0], keys[3]);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case "numeric":
                        switch (keys[2])
                        {
                            case "eq":
                                result += string.Format(" AND {0}={1}", keys[0], keys[3]);
                                break;
                            case "lt":
                                result += string.Format(" AND {0}>{1}", keys[0], keys[3]);
                                break;
                            case "gt":
                                result += string.Format(" AND {0}<{1}", keys[0], keys[3]);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "list":
                        if (keys[0].Contains("EXT_"))
                        {
                            string[] values = keys[2].Split(',');
                            for (int j = 0; j < values.Length; j++)
                            {
                                if (j == 0)
                                {
                                    result += string.Format(" AND ({0} LIKE '%{1}%'", keys[0], values[j].ToSqlCondtion());
                                }
                                else
                                {
                                    result += string.Format(" OR {0} LIKE '%{1}%'", keys[0], values[j].ToSqlCondtion());
                                }
                            }
                            result += ")";
                        }
                        else
                        {
                            if (keys[2].Split(',').Contains("0"))
                            {
                                result += string.Format(" AND ({0} IN ({1}) or {0} is null)", keys[0], keys[2]);
                            }
                            else
                            {
                                string guids = keys[2].GuidsToWhereString();
                                if (!string.IsNullOrEmpty(guids))
                                {
                                    result += string.Format(" AND {0} IN ({1})", keys[0], keys[2].GuidsToWhereString());
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取Excel文件的表名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetSheetName(string filePath)
        {
            string sheetName = "";
            Stream stream = File.OpenRead(filePath);
            byte[] fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            byte[] bytes = new byte[]{Convert.ToByte(11),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),
                Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0), Convert.ToByte(11),Convert.ToByte(0),Convert.ToByte(0),
                Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0),Convert.ToByte(0), Convert.ToByte(30),
                Convert.ToByte(16),Convert.ToByte(0),Convert.ToByte(0)};
            int index = GetSheetIndex(fileBytes, bytes);
            if (index > -1)
            {
                index += 16 + 12;
                System.Collections.ArrayList sheetNameList = new System.Collections.ArrayList();

                for (int i = index; i < fileBytes.Length - 1; i++)
                {
                    byte temp = fileBytes[i];
                    if (temp != Convert.ToByte(0))
                        sheetNameList.Add(temp);
                    else
                        break;
                }
                byte[] sheetNameByte = new byte[sheetNameList.Count];
                for (int i = 0; i < sheetNameList.Count; i++)
                    sheetNameByte[i] = Convert.ToByte(sheetNameList[i]);
                sheetName = System.Text.Encoding.Default.GetString(sheetNameByte);
            }
            return sheetName;
        }

        /// <summary> 
        /// 只供方法GetSheetName()使用 
        /// </summary> 
        /// <returns></returns> 
        private static int GetSheetIndex(byte[] FindTarget, byte[] FindItem)
        {
            int index = -1;

            int FindItemLength = FindItem.Length;
            if (FindItemLength < 1) return -1;
            int FindTargetLength = FindTarget.Length;
            if ((FindTargetLength - 1) < FindItemLength) return -1;

            for (int i = FindTargetLength - FindItemLength - 1; i > -1; i--)
            {
                System.Collections.ArrayList tmpList = new System.Collections.ArrayList();
                int find = 0;
                for (int j = 0; j < FindItemLength; j++)
                {
                    if (FindTarget[i + j] == FindItem[j]) find += 1;
                }
                if (find == FindItemLength)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// 将日期转化为当天最大日期
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToMaxDateTime(string datetime)
        {
            return DateTime.Parse(datetime).ToShortDateString() + " 23:59:59";
        }

        /// <summary>
        /// Resize图片
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <returns>处理以后的图片</returns>
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="data">待加密的字符串</param>
        /// <param name="key">DES密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string ToDes(string data, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(data);
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="data">待解密的字符串</param>
        /// <param name="key">DES密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string FromDes(string data, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[data.Length / 2];
            for (int x = 0; x < data.Length / 2; x++)
            {
                int i = (Convert.ToInt32(data.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 将指定类型的数据转换成字节数组
        /// </summary>
        /// <param name="value">字符串形式的数据</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="property">数据属性</param>
        /// <param name="length">数据长度</param>
        /// <param name="isVariable">是否为变长</param>
        /// <param name="isLengthVar">是否为长度参数</param>
        /// <returns></returns>
        public static byte[] GetBytes(string value, Iso8583DataType dataType, Iso8583Property property, int length, bool isVariable, bool isLengthVar)
        {
            //转换后的数组所占字节数
            int varLen = length;
            //pos 数据指针起始位置
            int pos = 0;
            //BCD码占半字节
            if (dataType == Iso8583DataType.BCD)
            {
                varLen = (length + 1) / 2;
            }
            //如果为长度参数，则BCD码和ASCII码均占用相同的字节数
            if (isLengthVar)
            {
                varLen = length;
            }
            byte[] data = new byte[varLen];

            //预定义填充的字符
            string fillStr = "";
            if (isVariable)
            {
                if (value.Length < length || isLengthVar)
                {
                    switch (dataType)
                    {
                        //BCD码补0
                        case Iso8583DataType.BCD:
                            fillStr = "0";
                            break;
                        //ASCII码补“0”的ASCII码20
                        case Iso8583DataType.AS:
                            fillStr = "20";
                            break;
                        case Iso8583DataType.GBK:
                            fillStr = "20";
                            break;
                        default:
                            break;
                    }
                    switch (property)
                    {
                        //如果是靠右，指针偏移相应的长度
                        case Iso8583Property.N:
                            if (isLengthVar)
                                pos += (varLen * 2 - value.Length) / 2;
                            else
                                pos += varLen - value.Length;
                            break;
                        default:
                            break;
                    }
                }
            }
            switch (dataType)
            {
                case Iso8583DataType.BCD:
                    {
                        switch (property)
                        {
                            case Iso8583Property.N:
                                {
                                    for (int i = 0; i < pos; i++)
                                    {
                                        data[i] = Convert.ToByte(fillStr, 16);
                                    }
                                    for (int i = 0; i < varLen - pos; i++)
                                    {
                                        if (value.Length % 2 == 1)
                                        {
                                            value = value.Insert(0, "0");
                                        }
                                        int num = int.Parse(value.Substring(i * 2, 2));
                                        int highNum = (num / 10) << 4;
                                        int lowNum = num % 10;
                                        byte m = Convert.ToByte(highNum);
                                        m += Convert.ToByte(lowNum);
                                        data[pos + i] = m;
                                    }
                                    break;
                                }
                            case Iso8583Property.ANS:
                                {
                                    for (int i = 0; i < varLen; i++)
                                    {
                                        if (value.Length % 2 == 1)
                                        {
                                            value = value.Insert(value.Length, "0");
                                        }
                                        int num = int.Parse(value.Substring(i * 2, 2));
                                        int highNum = (num / 10) << 4;
                                        int lowNum = num % 10;
                                        byte m = Convert.ToByte(highNum);
                                        m += Convert.ToByte(lowNum);
                                        data[i] = m;
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    break;
                case Iso8583DataType.AS:
                    {
                        switch (property)
                        {
                            case Iso8583Property.N:
                                {
                                    for (int i = 0; i < pos; i++)
                                    {
                                        data[i] = Convert.ToByte(fillStr, 16);
                                    }
                                    byte[] m = Encoding.ASCII.GetBytes(value.ToCharArray());
                                    Array.Copy(m, 0, data, pos, m.Length);
                                }
                                break;
                            case Iso8583Property.ANS:
                                {
                                    for (int i = value.Length; i < length; i++)
                                    {
                                        value += fillStr;
                                    }
                                    byte[] m = Encoding.ASCII.GetBytes(value.ToCharArray());
                                    Array.Copy(m, data, m.Length);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case Iso8583DataType.GBK:
                    switch (property)
                    {
                        case Iso8583Property.N:
                            {
                                for (int i = 0; i < pos; i++)
                                {
                                    data[i] = Convert.ToByte(fillStr, 16);
                                }
                                byte[] m = Encoding.GetEncoding("GBK").GetBytes(value.ToCharArray());
                                Array.Copy(m, 0, data, pos, m.Length);
                            }
                            break;
                        case Iso8583Property.ANS:
                            {
                                for (int i = value.Length * 2; i < length; i++)
                                {
                                    value += fillStr;
                                }
                                byte[] m = Encoding.GetEncoding("GBK").GetBytes(value.ToCharArray());
                                Array.Copy(m, data, m.Length);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return data;
        }

        /// <summary>
        /// 按指定的数据类型解码并格式化数据
        /// </summary>
        /// <param name="data">源字节数组</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="property">属性</param>
        /// <param name="length">数据的实际长度</param>
        /// <param name="isVariable">是否为变长域</param>
        /// <returns></returns>
        public static string ToFormatString(byte[] data, Iso8583DataType dataType, Iso8583Property property, int length, bool isVariable)
        {
            StringBuilder sb = new StringBuilder();
            string returnStr = "";
            //如果数据类型为B，直接处理
            if (dataType == Iso8583DataType.B)
            {
                foreach (byte b in data)
                {
                    sb.Append(b.ToString("x2") + "-");
                }
                sb.Remove(sb.Length - 1, 1);
                returnStr = sb.ToString();
                return returnStr;
            }
            //按数据类型进行数据转换处理
            switch (dataType)
            {
                case Iso8583DataType.BCD:
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            sb.Append(data[i].ToString("X2"));
                        }
                        break;
                    }
                case Iso8583DataType.AS:
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            sb.Append(Encoding.ASCII.GetString(data, i, 1));
                        }
                        break;
                    }
                case Iso8583DataType.GBK:
                    for (int i = 0; i < data.Length / 2; i++)
                    {
                        sb.Append(Encoding.GetEncoding("GBK").GetString(data, i * 2, 2));
                    }
                    break;
                default:
                    sb.Append(Encoding.Default.GetString(data));
                    break;
            }
            //按属性进行去头或去尾操作
            switch (property)
            {
                case Iso8583Property.N:
                    if (length < sb.Length)
                    {
                        sb.Remove(0, sb.Length - length);
                    }
                    if (isVariable)
                    {
                        returnStr = sb.ToString().TrimStart('0');
                    }
                    else
                    {
                        returnStr = sb.ToString();
                    }
                    break;
                case Iso8583Property.ANS:
                    if (length < sb.Length)
                    {
                        sb.Remove(length, sb.Length - length);
                    }
                    if (isVariable)
                    {
                        returnStr = sb.ToString().TrimEnd(' ');
                    }
                    else
                    {
                        returnStr = sb.ToString().TrimEnd(' ');
                    }
                    break;
                default:
                    break;
            }
            return returnStr;
        }

        /// <summary>
        /// 字节数组转换成十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string ByteToHex(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 字节数组转换成十六进制字符串(带空格)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexWithBlank(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + " ";
                }
            }
            return returnStr;
        }

        /// <summary>
        /// BCD数字转换成指定长度的二进制数组
        /// </summary>
        /// <param name="num">BCD字符串</param>
        /// <param name="bytesLength">转换后的二进制数组长度,左边补0</param>
        /// <returns></returns>
        public static byte[] BCDHexToByte(string BCDStr, int bytesLength)
        {
            byte[] data = new byte[bytesLength];
            //numLength:BCD字符串长度
            int numLength = BCDStr.Length;
            //numBytesLen:BCD所占的字节长度
            int numBytesLen = (numLength + 1) / 2;
            //左侧多余的字节补0
            for (int i = 0; i < bytesLength - numBytesLen; i++)
            {
                data[i] = (byte)0;
            }
            string numStr = BCDStr;
            //补足整数字节
            if (numLength % 2 == 1)
            {
                numStr = numStr.Insert(0, "0");
            }
            //按字节进行转换
            for (int i = 0; i < numBytesLen; i++)
            {
                string tempNum = numStr.Substring(i * 2, 2);
                data[bytesLength - numBytesLen + i] = byte.Parse(tempNum, System.Globalization.NumberStyles.HexNumber);
            }
            return data;
        }
        /// <summary>
        /// 将字符串以给定域的格式进行转换，type表示传入的value是字符串还是数字
        /// </summary>
        /// <param name="value">传入值</param>
        /// <param name="field">给定的域</param>
        /// <returns></returns>
        public static string SetFormat(string value, Iso8583Field field)
        {
            switch (field.Format)
            {
                case Iso8583Format.LVAR:
                case Iso8583Format.LLVAR:
                case Iso8583Format.LLLVAR:
                    return value;
                default:
                    {
                        if (value.Length == field.Length)
                        {
                            return value;
                        }

                        char fillChar = ' ';
                        switch (field.DataType)
                        {
                            case Iso8583DataType.BCD:
                                fillChar = '0';
                                break;
                            case Iso8583DataType.AS:
                                fillChar = ' ';
                                break;
                            default:
                                break;
                        }
                        int len = field.Length - value.Length;
                        string filledStr = "";
                        for (int i = 0; i < len; i++)
                        {
                            filledStr += fillChar;
                        }

                        switch (field.Property)
                        {
                            case Iso8583Property.N:
                                value = value.Insert(0, filledStr);
                                break;
                            case Iso8583Property.ANS:
                                value = value.Insert(value.Length, filledStr);
                                break;
                            default:
                                break;
                        }
                    }
                    return value;
            }
        }

        /// <summary>   
        /// 得到字符串的长度，一个汉字算2个字符   
        /// </summary>   
        /// <param name="str">字符串</param>   
        /// <returns>返回字符串长度</returns>   
        public static int GetLength(string str)
        {
            if (str.Length == 0) return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="where">条件语句</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">第几页(从1开始)</param>
        /// <param name="pageSize">页数</param>
        /// <returns></returns>
        public static DataTable GetPagedTable(DataTable dt, string where, string orderBy, int pageIndex, int pageSize)
        {
            DataTable newdt = dt.Clone();
            DataRow[] drs = dt.Select(where, orderBy);
            int rowbegin = (pageIndex - 1) * pageSize;
            int rowend = pageIndex * pageSize;
            if (rowbegin >= drs.Length)
            {
                foreach (DataRow dr in drs)
                {
                    newdt.ImportRow(dr);
                }
                return newdt;
            }

            if (rowend > drs.Length)
                rowend = drs.Length;

            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                newdt.ImportRow(drs[i]);
            }
            return newdt;
        }

        /// <summary>
        /// XML转义字符处理
        /// </summary>
        public static string ConvertXml(string xml)
        {
            xml = (char)1 + xml;   //为了避免首字母为要替换的字符，前加前缀
            for (int intNext = 0; true; )
            {
                int intIndexOf = xml.IndexOf("&", intNext);
                intNext = intIndexOf + 1;  //避免&被重复替换
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&amp;" + xml.Substring(intIndexOf + 1);
                }
            }
            for (; true; )
            {
                int intIndexOf = xml.IndexOf("<");
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&lt;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf(">");
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&gt;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf("\"");
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&quot;" + xml.Substring(intIndexOf + 1);
                }
            }
            return xml.Replace(((char)1).ToString(), "");
        }

        /// <summary>
        ///  -1 非手机及电话号码，0手机，1电话
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="telPrefix">前缀</param>
        /// <param name="telMain">主体</param>
        /// <returns></returns>
        public static int CheckTelNumber(string tel, out string telPrefix, out string telMain)
        {
            tel = tel.Replace("-", "");
            telMain = string.Empty;
            telPrefix = string.Empty;
            Match m = Regex.Match(tel.ToDBC(), RegularExp.FullMobile);
            Match m1 = Regex.Match(tel.ToDBC(), RegularExp.Phone);
            if (m.Success) // 手机号码
            {
                if (tel.Length == 11)
                {
                    telMain = tel;
                }
                else
                {
                    telPrefix = "0";
                    telMain = tel.Substring(1, tel.Length - 1);
                }
                return 0;
            }
            else if (m1.Success)  //电话号码
            {  // （010），广州（020），上海（021），天津（022），重庆（023），沈阳（024），南京（025），武汉（027），成都（028），西安（029）
                if (tel.Length == 7 || tel.Length == 8)
                {
                    telMain = tel;
                }
                else
                {
                    List<string> threePrefixs = new List<string>();
                    threePrefixs.Add("010");
                    threePrefixs.Add("021");
                    threePrefixs.Add("022");
                    threePrefixs.Add("023");
                    threePrefixs.Add("025");
                    threePrefixs.Add("027");
                    threePrefixs.Add("028");
                    threePrefixs.Add("029");
                    string s = tel.Substring(0, 3);
                    //Match m3 = Regex.Match(tel.ToDBC(), "(010|021|022|023|024|025|026|027|028|029)d{8}");
                    if (threePrefixs.Contains(s)) // 如果区号是3位的
                    {
                        telPrefix = tel.Substring(0, 3);
                        telMain = tel.Substring(3, tel.Length - 3);
                    }
                    else
                    {
                        telPrefix = tel.Substring(0, 4);
                        telMain = tel.Substring(4, tel.Length - 4);
                    }
                }
                return 1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 根据当前账号SID获取数据库服务器
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public static T GetDatabaseServerId<T>(List<T> servers)
        {
            int count = servers.Count - 1;
            string sidString = WindowsIdentity.GetCurrent().User.ToString();
            string userName = WindowsIdentity.GetCurrent().Name;
            string[] sids = sidString.Split('-');
            int sid = int.Parse(sids[sids.Length - 1]);
            if (userName.ToLower() == "sz1card1\\tech-admin")
            {
                return servers[0];
            }
            return servers[(sid % count) + 1];
        }

        /// <summary>
        /// 获取当前连接数据库服务器名称
        /// </summary>
        /// <returns></returns>
        public static string GetBasebaseServerName()
        {
            NameValueCollection values = (NameValueCollection)ConfigurationManager.GetSection("dataBaseServers");
            List<string> servers = new List<string>();
            foreach (string key in values.AllKeys)
            {
                servers.Add(values[key]);
            }
            return GetDatabaseServerId<string>(servers);
        }

        /// <summary>
        /// 对象序列化成 XML String
        /// </summary>
        public static string XmlSerialize<T>(T obj)
        {
            string xmlString = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }
        /// <summary>
        /// XML String 反序列化成对象
        /// </summary>
        public static T XmlDeserialize<T>(string xmlString)
        {
            T t = default(T);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    Object obj = xmlSerializer.Deserialize(xmlReader);
                    t = (T)obj;
                }
            }
            return t;
        }

        /// <summary>
        /// 标准 HMAC_MD5 的软件实现，参照 RFC2104 标准。
        /// </summary>
        /// <param name="pucText">[in]等处理的数据缓存区指针，大于 0 小于等于 51 个字节 </param>
        /// <param name="ulText_Len">[in]数据长度，大于 0 小于等于 51 </param>
        /// <param name="pucKey">[in]密钥，按标准 RFC2104，长度可以任意 </param>
        /// <param name="ulKey_Len">[in]密钥长度 </param>
        /// <param name="pucToenKey">[out]硬件计算需要的 KEY，固定 32 字节。 </param>
        /// <param name="pucDigest">[out]计算结果，固定 16 字节。 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        private static extern uint MD5_HMAC(byte[] pucText, byte ulText_Len, byte[] pucKey, byte ulKey_Len, [MarshalAs(UnmanagedType.LPArray)]byte[] pucToenKey, [MarshalAs(UnmanagedType.LPArray)]byte[] pucDigest);

        public static string GetUsbKeyMd5(string randomNumber, string secretKey)
        {
            byte[] sbMd5Key = new byte[32];
            byte[] sbdigest = new byte[16];
            byte[] bytRandomCode = Encoding.ASCII.GetBytes(randomNumber);
            byte[] bytShortKey = Encoding.ASCII.GetBytes(secretKey);
            MD5_HMAC(bytRandomCode, (byte)bytRandomCode.Length, bytShortKey, (byte)bytShortKey.Length, sbMd5Key, sbdigest);
            string res = string.Empty;
            foreach (char c in sbdigest)
            {
                res += ((byte)c).ToString("x2");
            }
            return res;
        }

        /// <summary>
        /// 获取托管和非托管文件的版本信息(如：1.0.0.2)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("version.dll")]
        static extern private bool GetFileVersionInfo(string fileName, uint handle, uint verLen, byte[] data);

        [DllImport("version.dll")]
        static extern private uint GetFileVersionInfoSize(string fileName, out uint handle);

        [DllImport("version.dll")]
        unsafe static extern private bool VerQueryValue(byte[] data, string sub, out short* subData, out uint subLen);

        [DllImport("version.dll")]
        private static extern bool VerQueryValue(byte[] pBlock, string pSubBlock, out string pValue, out uint len);

        unsafe public static string GetFileVersion(string path)
        {
            uint handle = 0;
            uint size = GetFileVersionInfoSize(path, out handle);
            if (size != 0)
            {
                byte[] buffer = new byte[size];
                if (GetFileVersionInfo(path, handle, size, buffer))
                {
                    short* subBlock = null;
                    uint len = 0;
                    if (VerQueryValue(buffer, @"\VarFileInfo\Translation", out subBlock, out len))
                    {
                        string spv = @"\StringFileInfo\" + subBlock[0].ToString("X4") + subBlock[1].ToString("X4") + @"\ProductVersion";
                        string versionInfo;
                        if (VerQueryValue(buffer, spv, out versionInfo, out len))
                        {
                            return versionInfo;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 去掉html标签
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string ClearHtml(string htmlstring)
        {
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<img[\s\S]*?>", "[图片]", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = htmlstring.Replace("\r\n", "");
            htmlstring = HttpContext.Current.Server.HtmlEncode(htmlstring).Trim();
            return htmlstring;
        }


        /// <summary>
        /// 获取字符串的MD5值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetMD5HashFromText(string context)
        {
            MD5CryptoServiceProvider crypto = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF7.GetBytes(context);
            bytes = crypto.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte num in bytes)
            {
                sb.AppendFormat("{0:x2}", num);
            }
            return sb.ToString();
        }

        /// <summary>
        ///  获取文件的MD5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            byte[] fileBytes = null;
            //FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            FileServerClient server = new FileServerClient(ConfigurationManager.AppSettings["sz1card1.FileServer.Address"]);
            server.GetFileBytes(fileName, out fileBytes);
            Stream stream = new MemoryStream(fileBytes);
            return GetMD5HashFromFile(stream);
        }

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(Stream stream)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);
                stream.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
        public static string GetTimeStamp()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(timeSpan.TotalSeconds).ToString();
        }

        public static void WriteHttpContextLog(HttpContext context)
        {
            String[] keys = context.Request.Form.AllKeys;
            foreach (string key in keys)
            {
                sz1card1.Common.Log.LoggingService.Info(key + "____" + context.Request[key]);
            }
        }
    }
}
