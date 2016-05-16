using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace sz1card1.Common
{
    /// <summary>
    /// 自定义Xml序列化抽象类
    /// </summary>
    public abstract class CustomXmlSerializable : IXmlSerializable
    {
        /// <summary>
        /// xml序列化
        /// </summary>
        /// <returns></returns>
        public virtual string ToXml()
        {
            XmlSerializer ser = new XmlSerializer(this.GetType());
            StringBuilder result = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(result))
            {
                ser.Serialize(writer, this);
            }
            return result.ToString();
        }

        public static T FromXml<T>(string xml) where T : CustomXmlSerializable
        {
            T message;
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                message = ser.Deserialize(reader) as T;
            }
            return message;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;
            if (wasEmpty)
                return;
            Type type = this.GetType();
            PropertyInfo[] pis = type.GetProperties();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    PropertyInfo pi = pis.SingleOrDefault<PropertyInfo>(p => p.Name == reader.LocalName);
                    if (pi == null)
                        continue;
                    if (reader.IsEmptyElement)
                    {
                        if (pi.PropertyType == typeof(string))
                        {
                            pi.SetValue(this, "", null);
                        }
                    }
                    else
                    {
                        reader.ReadStartElement(pi.Name);
                        if (pi.PropertyType == typeof(string))
                        {
                            pi.SetValue(this, reader.Value, null);
                        }
                        else if (pi.PropertyType == typeof(bool))
                        {
                            pi.SetValue(this, bool.Parse(reader.Value), null);
                        }
                        else if (pi.PropertyType == typeof(int))
                        {
                            pi.SetValue(this, int.Parse(reader.Value), null);
                        }
                        else if (pi.PropertyType == typeof(decimal))
                        {
                            pi.SetValue(this, decimal.Parse(reader.Value), null);
                        }
                        else if (pi.PropertyType == typeof(DateTime))
                        {
                            pi.SetValue(this, DateTime.Parse(reader.Value), null);
                        }
                        else if (pi.PropertyType == typeof(Guid))
                        {
                            pi.SetValue(this, new Guid(reader.Value), null);
                        }
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            Type type = this.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.GetValue(this, null) != null && pi.GetValue(this, null).ToString() != string.Empty)
                {
                    writer.WriteStartElement(pi.Name);
                    string value = pi.GetValue(this, null).ToString();
                    writer.WriteValue(value);
                    writer.WriteEndElement();
                }
            }
        }
    }
}
