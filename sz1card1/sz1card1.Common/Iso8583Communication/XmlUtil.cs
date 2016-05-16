using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace sz1card1.Common.Iso8583Communication
{
    public class XmlUtil
    {
        private static Dictionary<string, XmlSerializer> XmlSerializerDict = new Dictionary<string, XmlSerializer>();
        /// <summary>
        /// 缓存XmlSerializer对象,以避免XmlSerializer产生时自动编译
        /// </summary>
        /// <param >XmlSerializer类型</param>
        /// <param >根元素名称</param>
        /// <param >是否生成Soap格式的Xml文件</param>
        /// <returns>XmlSerializer对象</returns>
        public static XmlSerializer GetXmlSerializer(Type typeXS, string rootnodeName, bool IsSoapXml)
        {
            XmlSerializer rtVal;
            string keyHsXS = string.Format("{0}_{1}_{2}", typeXS.Name, rootnodeName, IsSoapXml);
            lock (XmlSerializerDict)//确保多线程安全性
            {
                if (XmlSerializerDict.ContainsKey(keyHsXS))
                {
                    rtVal = XmlSerializerDict[keyHsXS];
                }
                else
                {
                    if (IsSoapXml)
                    {
                        XmlTypeMapping myTypeMapping = (new SoapReflectionImporter()).ImportTypeMapping(typeXS);
                        rtVal = new XmlSerializer(myTypeMapping);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(rootnodeName))
                        {
                            rtVal = new XmlSerializer(typeXS, new XmlRootAttribute(rootnodeName));
                        }
                        else
                        {
                            rtVal = new XmlSerializer(typeXS);
                        }
                    }

                    XmlSerializerDict.Add(keyHsXS, rtVal);
                }
            }

            return rtVal;
        }
    }
}
