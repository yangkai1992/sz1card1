///<summary>
///Copyright (C) 深圳市一卡易科技发展有限公司
///创建标识：2009-05-11 Created by pq
///功能说明：提供操作XML常规数据的方法类
///注意事项：有些方法返回的是Json对象
///更新记录：
///2009-05-11 [by pq] 新增对省市区三级联动的相关方法
///</summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;

namespace sz1card1.Common
{
    /// <summary>
    /// Xml数据通用方法类
    /// </summary>
    public class DataUtil
    {
        private static XmlDocument regionXml;
        private static XmlDocument RegionXml
        {
            get
            {
                if (regionXml == null)
                {
                    regionXml = new XmlDocument();
                    regionXml.Load(GetResource("sz1card1.Common.Data.Region.xml"));
                }
                return regionXml;
            }
        }
        /// <summary>
        /// 以名称值对集合的形式返回所有省份
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> GetProvinces()
        {

            IEnumerable<KeyValuePair<string, string>> dic;
            List<KeyValuePair<string, string>> dd = new List<KeyValuePair<string, string>>();
            XmlDocument xml = new XmlDocument();
            try
            {
                //xml.Load(GetResource("sz1card1.VipCloud.Common.Region.xml"));
                XmlNode xmlNode = RegionXml.SelectSingleNode("/region");
                //进行错误追踪
                if (xmlNode == null)
                {
                    string trackMsg = string.Empty;
                    trackMsg += "地区错误: xmlNode is NULL-----";
                    XmlDocument regionXml2 = new XmlDocument();
                    regionXml2.Load(GetResource("sz1card1.Common.Data.Region.xml"));
                    if (regionXml2 == null)
                    {
                        trackMsg += "地区错误: xmlNode is still NULL";
                    }
                    else 
                    {
                        trackMsg += "地区正确: xmlNode is not NULL";
                    }
                    sz1card1.Common.Log.LoggingService.Info(trackMsg);
                }
                XmlNodeList xmlNodeList = xmlNode.ChildNodes;
                foreach (XmlNode xm in xmlNodeList)
                {
                    dd.Add(new KeyValuePair<string, string>(xm.Attributes["id"].Value, xm.Attributes["name"].Value));
                }
                //dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.Province.xml")).Elements("Province")
                //      select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
                dd.Add(new KeyValuePair<string, string>("0", "请选择"));
                dic = dd;
                return dic;
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn("地区错误" + ex);
                dic = null;
                return dic;
            }

        }

        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        public static IEnumerable GetCities()
        {

            //var dic = from c in XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City")
            //          select new
            //          {
            //              ProvinceID = c.Element("ProvinceID").Value,
            //              Key = c.Element("ID").Value,
            //              Value = c.Element("Name").Value
            //          };
            //return dic;
            IEnumerable<KeyValuePair<string, string>> dic;
            XmlDocument xml = new XmlDocument();
            List<KeyValuePair<string, string>> dd = new List<KeyValuePair<string, string>>();
            //xml.Load(GetResource("sz1card1.Common.Data.Region.xml"));
            XmlNode xmlNode = RegionXml.SelectSingleNode("/region");
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;
            foreach (XmlNode no in xmlNodeList)
            {

                foreach (XmlNode xm in no.ChildNodes)
                {
                    dd.Add(new KeyValuePair<string, string>(xm.Attributes["id"].Value, xm.Attributes["name"].Value));
                }
            }
            dd.Add(new KeyValuePair<string, string>("0", "请选择"));
            //dic = from c in XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City")
            //      where c.Element("ProvinceID").Value == provinceId || c.Element("ProvinceID").Value == "0"
            //      select new KeyValuePair<string, string>(c.Element("ID").Value, c.Element("Name").Value);
            dic = dd;
            return dic;
        }

        /// <summary>
        /// 获取热门城市
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetHotCities()
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City")
                  where p.Element("HotCity") != null && bool.Parse(p.Element("HotCity").Value)
                  select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
            return dic;
        }

        /// <summary>
        /// 以名称值对集合的形式返回当前省的市级城市
        /// </summary>
        /// <param name="provinceId">省份编号</param>
        public static IEnumerable<KeyValuePair<string, string>> GetCitiesByProvince(string provinceId)
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            XmlDocument xml = new XmlDocument();
            List<KeyValuePair<string, string>> dd = new List<KeyValuePair<string, string>>();
            //xml.Load(GetResource("sz1card1.Common.Data.Region.xml"));
            XmlNode xmlNode = RegionXml.SelectSingleNode("/region");
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["id"].Value == provinceId)
                {
                    foreach (XmlNode xm in no.ChildNodes)
                    {
                        dd.Add(new KeyValuePair<string, string>(xm.Attributes["id"].Value, xm.Attributes["name"].Value));
                    }
                }
            }
            dd.Add(new KeyValuePair<string, string>("0", "请选择"));
            //dic = from c in XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City")
            //      where c.Element("ProvinceID").Value == provinceId || c.Element("ProvinceID").Value == "0"
            //      select new KeyValuePair<string, string>(c.Element("ID").Value, c.Element("Name").Value);
            dic = dd;
            return dic;
        }

        /// <summary>
        /// 以名称值对集合的形式返回当前城市的县区
        /// </summary>
        /// <param name="cityId">城市编号</param>
        public static IEnumerable<KeyValuePair<string, string>> GetCountyByCity(string cityId)
        {

            IEnumerable<KeyValuePair<string, string>> dic;
            XmlDocument xml = new XmlDocument();
            List<KeyValuePair<string, string>> dd = new List<KeyValuePair<string, string>>();
            //xml.Load(GetResource("sz1card1.Common.Data.Region.xml"));
            XmlNode xmlNode = RegionXml.SelectSingleNode("/region");
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;
            foreach (XmlNode no in xmlNodeList)
            {
                foreach (XmlNode xmn in no.ChildNodes)
                {
                    if (xmn.Attributes["id"].Value == cityId)
                    {
                        foreach (XmlNode xm in xmn.ChildNodes)
                        {
                            dd.Add(new KeyValuePair<string, string>(xm.Attributes["id"].Value, xm.Attributes["name"].Value));
                        }
                    }
                }

            }
            dd.Add(new KeyValuePair<string, string>("0", "请选择"));
            //dic = from c in XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City")
            //      where c.Element("ProvinceID").Value == provinceId || c.Element("ProvinceID").Value == "0"
            //      select new KeyValuePair<string, string>(c.Element("ID").Value, c.Element("Name").Value);
            dic = dd;
            return dic;

            //IEnumerable<KeyValuePair<string, string>> dic;
            //dic = from c in XElement.Load(GetResource("sz1card1.Common.Data.County.xml")).Elements("Area")
            //      where c.Element("CityID").Value == cityId || c.Element("CityID").Value == "0"
            //      select new KeyValuePair<string, string>(c.Element("ID").Value, c.Element("Name").Value);
            //dic = dic.Reverse();
            //return dic;
        }
        /// <summary>
        /// 以名称值对集合的形式返回日期单位
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetDateTypes()
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from c in XElement.Load(GetResource("sz1card1.Common.Data.DateType.xml")).Elements("Date")
                  select new KeyValuePair<string, string>(c.Element("ID").Value, c.Element("Name").Value);
            return dic;
        }
        public static IEnumerable<KeyValuePair<string, string>> GetDateTypes(string[] list)
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from c in XElement.Load(GetResource("sz1card1.Common.Data.DateType.xml")).Elements("Date")
                  where list.Contains(c.Element("ID").Value)
                  select new KeyValuePair<string, string>(c.Element("ID").Value, c.Element("Name").Value);
            return dic;
        }
        /// <summary>
        /// 获得时间单位名
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetDateTypeById(string Id)
        {
            return XElement.Load(GetResource("sz1card1.Common.Data.DateType.xml")).Elements("Date").First<XElement>(d => d.Element("ID").Value == Id).Element("Name").Value;
        }
        /// <summary>
        /// 获得省份名
        /// </summary>
        /// <param name="provinceId">省份ID</param>
        /// <returns>省份名</returns>
        public static string GetProvinceById(string provinceId)
        {
            if (string.IsNullOrEmpty(provinceId) || provinceId == "0")
            {
                return "";
            }
            XmlNodeList xmlNodeList = RegionXml.SelectNodes("/region/province");
            string provinceName = "";
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["id"].Value == provinceId)
                {
                    provinceName = no.Attributes["name"].Value;
                    break;
                }
            }
            return provinceName;
            //try
            //{
            //    if (string.IsNullOrEmpty(provinceId) || provinceId == "0")
            //    {
            //        return "";
            //    }
            //    return XElement.Load(GetResource("sz1card1.Common.Data.Province.xml")).Elements("Province").First<XElement>(c => c.Element("ID").Value == provinceId).Element("Name").Value;
            //}
            //catch (Exception)
            //{
            //    sz1card1.Common.Log.LoggingService.Warn("获取省份名称出错:" + provinceId);
            //    return "";
            //}
        }

        /// <summary>
        ///  获取省份Id
        /// </summary>
        /// <param name="cityId">城市Id</param>
        /// <returns>省份ID</returns>
        public static string GetprovinceIdByCityId(string cityId)
        {
            if (string.IsNullOrEmpty(cityId) || cityId == "0")
            { return ""; }
            XmlNodeList xmlNodeList = RegionXml.SelectNodes("/region/province/city");
            string provinceName = "";
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["id"].Value == cityId)
                {
                    provinceName = no.ParentNode.Attributes["id"].Value;
                    break;
                }
            }
            return provinceName;
            //try
            //{
            //    if (string.IsNullOrEmpty(cityId) || cityId == "0")
            //    { return ""; }
            //    return XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City").First<XElement>(c => c.Element("ID").Value == cityId).Element("ProvinceID").Value;

            //}
            //catch (Exception)
            //{
            //    sz1card1.Common.Log.LoggingService.Warn("获取城市名称出错:" + cityId);
            //    throw;
            //}
        }

        /// <summary>
        /// 获得城市名
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns>城市名</returns>
        public static string GetCityById(string cityId)
        {
            if (string.IsNullOrEmpty(cityId) || cityId == "0")
            {
                return "";
            }
            List<KeyValuePair<string, string>> dd = new List<KeyValuePair<string, string>>();
            XmlNodeList xmlNodeList = RegionXml.SelectNodes("/region/province/city");
            string countyName = "";
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["id"].Value == cityId)
                {
                    countyName = no.Attributes["name"].Value;
                    break;
                }
            }
            return countyName;
        }

        /// <summary>
        /// 获取城市代码
        /// </summary>
        /// <param name="city"></param>
        /// <returns>CityID</returns>
        public static string GetCityCode(string cityName)
        {
            if (string.IsNullOrEmpty(cityName.Trim()))
            {
                return "0";
            }
            XmlNodeList xmlNodeList = RegionXml.SelectNodes("/region/province/city");
            string cityId = "";
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["name"].Value == cityName)
                {
                    cityId = no.Attributes["id"].Value;
                    break;
                }
            }
            return cityId;

            //if (XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City").Count<XElement>(c => c.Element("Name").Value == cityName.Trim()) == 0)
            //{
            //    return "0";
            //}
            //else
            //{
            //    return XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City").First<XElement>(c => c.Element("Name").Value == cityName.Trim()).Element("ID").Value;
            //}
        }

        /// <summary>
        /// 获取省份ID
        /// </summary>
        /// <param name="province">省份名称</param>
        /// <returns>ProvinceID</returns>
        public static string GetProvinceCode(string provinceName)
        {
            if (string.IsNullOrEmpty(provinceName.Trim()))
            {
                return "0";
            }
            XmlNodeList xmlNodeList = RegionXml.SelectNodes("/region/province");
            string provinceId = "";
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["name"].Value == provinceName)
                {
                    provinceId = no.Attributes["id"].Value;
                    break;
                }
            }
            return provinceId;
            //if (XElement.Load(GetResource("sz1card1.Common.Data.Province.xml")).Elements("Province").Count<XElement>(c => c.Element("Name").Value == provinceName.Trim()) == 0)
            //{
            //    return "0";
            //}
            //else
            //{
            //    return XElement.Load(GetResource("sz1card1.Common.Data.Province.xml")).Elements("Province").First<XElement>(c => c.Element("Name").Value == provinceName.Trim()).Element("ID").Value;
            //}
        }
        /// <summary>
        /// 获取县区ID
        /// </summary>
        /// <param name="county">县区名称</param>
        /// <returns>CountyID</returns>
        public static string GetCountyCode(string countyName)
        {
            if (string.IsNullOrEmpty(countyName.Trim()))
            {
                return "0";
            }
            XmlNodeList xmlNodeList = RegionXml.SelectNodes("/region/province/city/county");
            string countyId = "";
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["name"].Value == countyName)
                {
                    countyId = no.Attributes["id"].Value;
                    break;
                }
            }
            return countyId;
            //if (XElement.Load(GetResource("sz1card1.Common.Data.County.xml")).Elements("Area").Count<XElement>(c => c.Element("Name").Value == countyName.Trim()) == 0)
            //{
            //    return "0";
            //}
            //else
            //{
            //    return XElement.Load(GetResource("sz1card1.Common.Data.County.xml")).Elements("Area").First<XElement>(c => c.Element("Name").Value == countyName.Trim()).Element("ID").Value;
            //}
        }

        /// <summary>
        ///  根据区id获取城市id
        /// </summary>
        /// <param name="countyId"></param>
        /// <returns></returns>
        public static string GetCityIdByCountyId(string countyId)
        {
            if (string.IsNullOrEmpty(countyId) || countyId == "0")
            {
                return "";
            }
            XmlNodeList xmlNodeList = RegionXml.SelectNodes("/region/province/city/county");
            string cityName = "";
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["id"].Value == countyId)
                {
                    cityName = no.ParentNode.Attributes["id"].Value;
                    break;
                }
            }
            return cityName;
            //return XElement.Load(GetResource("sz1card1.Common.Data.County.xml")).Elements("Area").First<XElement>(c => c.Element("ID").Value == countyId).Element("CityID").Value;
        }
        /// <summary>
        /// 获得县区名
        /// </summary>
        /// <param name="countyId">县区ID</param>
        /// <returns>县区名</returns>
        public static string GetCountyById(string countyId)
        {
            if (string.IsNullOrEmpty(countyId) || countyId == "0")
            {
                return "";
            }
            List<KeyValuePair<string, string>> dd = new List<KeyValuePair<string, string>>();
            XmlNodeList xmlNodeList = RegionXml.SelectNodes("/region/province/city/county");
            string countyName = "";
            foreach (XmlNode no in xmlNodeList)
            {
                if (no.Attributes["id"].Value == countyId)
                {
                    countyName = no.Attributes["name"].Value;
                    break;
                }
            }
            return countyName;
        }

        /// <summary>
        /// 获取银行名称
        /// </summary>
        /// <param name="bankId">银行ID</param>
        /// <returns>银行名称</returns>
        public static string GetBankById(string bankId)
        {
            return XElement.Load(GetResource("sz1card1.Common.Data.Bank.xml")).Elements("Bank").First<XElement>(c => c.Element("ID").Value == bankId).Element("Name").Value;
        }

        /// <summary>
        /// 获取银行名称
        /// </summary>
        /// <param name="code">财付通银行代码</param>
        /// <returns>银行名称</returns>
        public static string GetBankByCode(string code)
        {
            return XElement.Load(GetResource("sz1card1.Common.Data.Bank.xml")).Elements("Bank").First<XElement>(c => c.Element("Code").Value == code).Element("Name").Value;
        }

        /// <summary>
        /// 获取意见反馈类型
        /// </summary>
        /// <param name="id">意见反馈类型ID</param>
        /// <returns>意见反馈类型名称</returns>
        public static string GetFeedbackTypeById(string id)
        {
            return XElement.Load(GetResource("sz1card1.Common.Data.FeedbackType.xml")).Elements("FeedbackType").First<XElement>(c => c.Element("ID").Value == id).Element("Name").Value;
        }

        /// <summary>
        ///  以名称值对集合的形式返回所有称谓
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetSexes()
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.Sex.xml")).Elements("Sex")
                  select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
            return dic;
        }
        public static string GetSexStr(string id)
        {
            return XElement.Load(GetResource("sz1card1.Common.Data.Sex.xml")).Elements("Sex").First<XElement>(c => c.Element("ID").Value == id).Element("Name").Value;
        }
        public static IEnumerable<KeyValuePair<string, string>> GetFeedbackTypes()
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.FeedbackType.xml")).Elements("FeedbackType")
                  select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
            return dic;
        }

        /// <summary>
        /// 获取所属行业xml
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static XmlDocument GetIndustry(string str)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetResource(str));
            return xmlDoc;
        }

        /// <summary>
        /// 以名称值对集合的形式返回所有行业
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetIndustrys()
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.Industry.xml")).Elements("Industry")
                  select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
            return dic;
        }
        public static DataTable GetMobileOperators()
        {
            XmlDataDocument data = new XmlDataDocument();
            data.DataSet.InferXmlSchema(GetResource("sz1card1.Common.Data.MobileOperator.xml"), null);
            data.Load(GetResource("sz1card1.Common.Data.MobileOperator.xml"));
            return data.DataSet.Tables[0];
        }
        /// <summary>
        /// 以名称值对集合的形式返回所有行业,手机会员卡所显示的行业
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetIndustrys_MobileCard()
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.Industry.xml")).Elements("Industry")
                  where p.Element("VisibleInMobileCard").Value == "1"
                  select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
            return dic;
        }
        /// <summary>
        /// 根据资源名获得TextReader
        /// </summary>
        /// <param name="resourceName">资源文件名(命名空间+文件名)</param>
        /// <returns>TextReader</returns>
        private static TextReader GetResource(string resourceName)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (stream != null) return new StreamReader(stream);
            return null;
        }

        public static IEnumerable<KeyValuePair<int, string>> GetStatus()
        {
            IEnumerable<KeyValuePair<int, string>> dic;
            dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.Status.xml")).Elements("Status")
                  select new KeyValuePair<int, string>(Convert.ToInt32(p.Element("ID").Value), p.Element("Name").Value);
            return dic;
        }
        public static IEnumerable<KeyValuePair<string, string>> GetAlertDateType()
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.IntervalType.xml")).Elements("IntervalType")
                  select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
            return dic;
        }
        public static object GetDurationTime()
        {
            var list = from p in XElement.Load(GetResource("sz1card1.Common.Data.DurationTime.xml")).Elements("Date")
                       select new
                       {
                           Id = p.Element("ID").Value,
                           Name = p.Element("Name").Value,
                           Value = p.Element("Value").Value
                       };
            return list;
        }
        /// <summary>
        /// 获取场地状态
        /// </summary>
        /// <returns></returns>
        public static object GetPlaceStatus()
        {
            var list = from p in XElement.Load(GetResource("sz1card1.Common.Data.PlaceStatus.xml")).Elements("Status")
                       select new
                       {
                           Id = p.Element("ID").Value,
                           Name = p.Element("Name").Value,
                       };
            return list;
        }


        /// <summary>
        /// 获取单据类型
        /// </summary>
        public static object GetBillTypes()
        {
            var list = from p in XElement.Load(GetResource("sz1card1.Common.Data.BillType.xml")).Elements("BillType")
                       select new
                       {
                           Id = p.Element("ID").Value,
                           Name = p.Element("Name").Value,
                           Value = p.Element("Value").Value,
                           PermitionType = p.Element("PermitionType").Value,
                           ImgUrl = p.Element("ImgUrl").Value,
                           Description = p.Element("Description").Value
                       };
            return list;
        }

        /// <summary>
        /// 获取单据类型
        /// </summary>
        public static object GetRecyclingTypes()
        {
            var list = from p in XElement.Load(GetResource("sz1card1.Common.Data.RecyclingType.xml")).Elements("NodeType")
                       select new
                       {
                           Id = p.Element("ID").Value,
                           Name = p.Element("Name").Value,
                           Value = p.Element("Value").Value,
                           ImgUrl = p.Element("ImgUrl").Value,
                           Description = p.Element("Description").Value
                       };
            return list;
        }
        /// <summary>
        /// 通过TypeName 获得相应的Ways
        /// </summary>
        /// <param name="typeName">如，ConsumeWay,CountWay,ValueWay,PointWay,CheckOutWay</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetWaysByWayType(string typeName)
        {
            XDocument xml = XDocument.Load(GetResource("sz1card1.Common.Data.DescForWay.xml"));
            IEnumerable<KeyValuePair<string, string>> wayList;
            wayList = from p in xml.Root.Elements("WayType").Elements("WayInfo")
                      where p.Parent.Attribute("Name").Value == typeName
                      select new KeyValuePair<string, string>(
                          p.Element("Value").Value, p.Element("Text").Value);
            return wayList;
        }


        /// <summary>
        ///  获取支付类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetPayType()
        {
            IEnumerable<KeyValuePair<string, string>> dic;
            dic = from p in XElement.Load(GetResource("sz1card1.Common.Data.PayType.xml")).Elements("PayType")
                  select new KeyValuePair<string, string>(p.Element("Value").Value, p.Element("Name").Value);
            return dic;
        }

        /// <summary>
        /// 获取第三方支付类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetThirdPayType()
        {
            IEnumerable<KeyValuePair<string, string>> dict;
            dict = from p in XElement.Load(GetResource("sz1card1.Common.Data.ThirdPayType.xml")).Elements("ThirdPayType")
                   select new KeyValuePair<string, string>(p.Element("Value").Value, p.Element("Name").Value);
            return dict;
        }

        /// <summary>
        /// 获取结账第三方支付类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetCheckOutThirdPayType()
        {
            IEnumerable<KeyValuePair<string, string>> dict;
            dict = from p in XElement.Load(GetResource("sz1card1.Common.Data.CheckOutThirdPayType.xml")).Elements("ThirdPayType")
                   select new KeyValuePair<string, string>(p.Element("Value").Value, p.Element("Name").Value);
            return dict;
        }

        /// <summary>
        /// 获取提成途径
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetDeductType()
        {
            IEnumerable<KeyValuePair<string, string>> dict;
            dict = from p in XElement.Load(GetResource("sz1card1.Common.Data.DeductType.xml")).Elements("DeductType")
                   select new KeyValuePair<string, string>(p.Element("Value").Value, p.Element("Name").Value);
            return dict;
        }

        /// <summary>
        /// 获取单据类型
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> GetBillTypeDict()
        {
            IEnumerable<KeyValuePair<string, string>> dict;
            dict = from p in XElement.Load(GetResource("sz1card1.Common.Data.BillType.xml")).Elements("BillType")
                   select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Value").Value);
            return dict;
        }

        /// <summary>
        /// 获取快递类型
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> GetShippingWayDict()
        {
            try
            {
                IEnumerable<KeyValuePair<string, string>> dict;
                dict = from p in XElement.Load(GetResource("sz1card1.Common.Data.ShippingWayType.xml")).Elements("ShippingWayType")
                       select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
                return dict;
            }
            catch (Exception ex)
            {
                sz1card1.Common.Log.LoggingService.Warn("BindFreightTemplate:" + ex.Message + "##" + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 获取顾客类型列表：会员；散客
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetCustomerType()
        {
            IEnumerable<KeyValuePair<string, string>> dict;
            dict = from p in XElement.Load(GetResource("sz1card1.Common.Data.CustomerType.xml")).Elements("CustomerType")
                   select new KeyValuePair<string, string>(p.Element("ID").Value, p.Element("Name").Value);
            return dict;
        }

        /// <summary>
        /// 解析地理位置
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="provinceid"></param>
        /// <param name="cityid"></param>
        /// <param name="countryid"></param>
        public void GetGeographicalPosition(string province, string city, string country, out int? provinceid, out int? cityid, out int? countryid)
        {
            XmlDocument xmlDoc = new XmlDocument();
            provinceid = 0;
            cityid = 0;
            countryid = 0;
            if (Provinces == null)
            {
                Provinces = this.GetProvince(xmlDoc);
            }
            if (Citys == null)
            {
                Citys = this.GetCity(xmlDoc);
            }
            if (Countrys == null)
            {
                Countrys = this.GetCountry(xmlDoc);
            }
            if (string.IsNullOrEmpty(province))
            {
                return;
            }
            var Obprovin = from u
                           in Provinces
                           where u.Name == province || u.Name.Contains(province)
                           select u;
            if (Obprovin.Count() > 0)
            {
                provinceid = Obprovin.First().ID;
                if (string.IsNullOrEmpty(city))
                {
                    return;
                }
                var Obcity = from u
                            in Citys
                             where u.ProvinceID == Obprovin.First().ID
                             && (u.Name == city || u.Name.Contains(city))
                             select u;
                if (Obcity.Count() > 0)
                {
                    cityid = Obcity.First().ID;
                    if (string.IsNullOrEmpty(country))
                    {
                        return;
                    }
                    var Obcountry = from u
                                    in Countrys
                                    where u.CityID == Obcity.First().ID
                                    && (u.Name == country || u.Name.Contains(country))
                                    select u;
                    if (Obcountry.Count() > 0)
                    {
                        countryid = Obcountry.First().ID;
                    }
                }
            }
        }

        /// <summary>
        /// 解析省份xml
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public List<Province> GetProvince(XmlDocument xmlDoc)
        {
            List<Province> provinces = new List<Province>();
            var list = from p in XElement.Load(GetResource("sz1card1.Common.Data.Province.xml")).Elements("Province")
                       select new Province
                       {
                           ID = Convert.ToInt32(p.Element("ID").Value),
                           Name = p.Element("Name").Value,
                       };
            provinces = list.ToList();
            return provinces;
        }

        /// <summary>
        /// 解析城市xml
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        private List<City> GetCity(XmlDocument xmlDoc)
        {
            List<City> cities = new List<City>();
            var list = from p in XElement.Load(GetResource("sz1card1.Common.Data.City.xml")).Elements("City")
                       select new City
                       {
                           ID = Convert.ToInt32(p.Element("ID").Value),
                           Name = p.Element("Name").Value,
                           ProvinceID = Convert.ToInt32(p.Element("ProvinceID").Value)
                       };
            cities = list.ToList();
            return cities;
        }

        /// <summary>
        /// 解析区xml
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        private List<Country> GetCountry(XmlDocument xmlDoc)
        {
            List<Country> Countries = new List<Country>();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(GetResource("sz1card1.Common.Data.County.xml"), settings);
            xmlDoc.Load(reader);
            var list = from p in XElement.Load(GetResource("sz1card1.Common.Data.County.xml")).Elements("Area")
                       select new Country
                       {
                           ID = Convert.ToInt32(p.Element("ID").Value),
                           Name = p.Element("Name").Value,
                           CityID = Convert.ToInt32(p.Element("CityID").Value)
                       };
            Countries = list.ToList();
            return Countries;
        }

        /// <summary>
        /// 省份
        /// </summary>
        public class Province
        {
            public int? ID
            {

                get;
                set;
            }

            public string Name
            {

                get;
                set;
            }
        }

        /// <summary>
        /// 城市
        /// </summary>
        public class City
        {
            public int? ID
            {

                get;
                set;
            }

            public string Name
            {

                get;
                set;
            }

            public int? ProvinceID
            {
                get;
                set;
            }
        }

        /// <summary>
        /// 区
        /// </summary>
        public class Country
        {
            public int? ID
            {

                get;
                set;
            }

            public string Name
            {

                get;
                set;
            }

            public int? CityID
            {
                get;
                set;
            }
        }


        private List<Country> Countrys
        {
            get;
            set;
        }
        private List<City> Citys
        {
            get;
            set;
        }
        private List<Province> Provinces
        {
            get;
            set;
        }
    }
}
