using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace WebUserControl.Common
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
                dic = null;
                return dic;
            }

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
    }
}
