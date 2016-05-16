using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace sz1card1.Common.SerializableClass
{
    public class SerializableDictionary<TKey, TValue>
           : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable 成员
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            if (reader.IsEmptyElement || !reader.Read())
            {
                return;
            }

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadEndElement();
                reader.MoveToContent();
                this.Add(key, value);
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                valueSerializer.Serialize(writer, this[key]);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
        #endregion

        public string ToXml()
        {
            XmlSerializer ser = new XmlSerializer(this.GetType());
            StringBuilder result = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(result))
            {
                ser.Serialize(writer, this);
            }
            return result.ToString();
        }

        public static SerializableDictionary<TKey, TValue> FromXml(string xml)
        {
            SerializableDictionary<TKey, TValue> entity;
            XmlSerializer ser = new XmlSerializer(typeof(SerializableDictionary<TKey, TValue>));
            using (StringReader reader = new StringReader(xml))
            {
                entity = ser.Deserialize(reader) as SerializableDictionary<TKey, TValue>;
            }
            return entity;
        }

        public static SerializableDictionary<TKey, TValue> FromDictionary(Dictionary<TKey, TValue> dict)
        {
            if (dict == null)
            {
                return null;
            }
            SerializableDictionary<TKey, TValue> sdict = new SerializableDictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> item in dict)
            {
                sdict.Add(item.Key, item.Value);
            }
            return sdict;
        }


        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> item in this)
            {
                dict.Add(item.Key, item.Value);
            }
            return dict;
        }
    }
}
