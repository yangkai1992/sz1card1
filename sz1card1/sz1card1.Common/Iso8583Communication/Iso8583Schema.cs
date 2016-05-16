using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;

namespace sz1card1.Common.Iso8583Communication
{
    /// <summary>
    /// 表示 ISO 8583 包的所有数据域集合
    /// </summary>
    public class Iso8583Schema
    {
        internal IsoBitmap bitmap;
        internal SortedList<int, Iso8583Field> fields;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Iso8583Schema()
        {
            this.bitmap = new IsoBitmap();
            this.fields = new SortedList<int, Iso8583Field>(IsoBitmap.FieldCount);
        }
        /// <summary>
        /// 从指定的xml流加载并构造实例
        /// </summary>
        /// <param name="xml">xml流</param>
        public Iso8583Schema(string[] xmlArray)
            : this()
        {
            this.LoadFromXml(xmlArray);
        }

        public SortedList<int, Iso8583Field> GetAllFileds()
        {
            return this.fields;
        }

        public void SerializeTest()
        {
            List<Iso8583Field> field = new List<Iso8583Field>();
            for (int i = 0; i < this.fields.Count; i++)
            {
                field.Add(this.fields[this.fields.Keys[i]]);
            }
            XmlSerializer ser = new XmlSerializer(typeof(List<Iso8583Field>));
            StringBuilder result = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(result))
            {
                ser.Serialize(writer, field);
            }
        }


        /// <summary>
        /// 增加一个数据域
        /// </summary>
        /// <param name="field">数据域信息</param>
        public void AddField(Iso8583Field field)
        {
            if (field == null)
                throw new ArgumentNullException("添加的数据域为空");
            if (this.fields.ContainsKey(field.BitNum))
            {
                //如果数据域已存在，替换掉先前的域
                this.fields.Remove(field.BitNum);
            }
                if (field.BitNum < 1 || field.BitNum > IsoBitmap.FieldCount)
                throw new Exception(string.Format("数据域的位置越界。"));
            this.fields.Add(field.BitNum, field);
            this.bitmap.Set(field.BitNum, true);
        }
        /// <summary>
        /// 移除一个数据域
        /// </summary>
        /// <param name="bitNum"></param>
        public void RemoveField(int bitNum)
        {
            this.fields.Remove(bitNum);
            this.bitmap.Set(bitNum, false);
        }

        /// <summary>
        /// 从Xml文本导入架构
        /// </summary>
        /// <param name="xml">xml文本</param>
        /// <returns></returns>
        public void LoadFromXml(string[] xmlArray)
        {
            XmlSerializer serial =XmlUtil.GetXmlSerializer(typeof(Iso8583Field[]), "", false);
            StringReader reader;
            foreach (string xml in xmlArray)
            {
                if (!string.IsNullOrEmpty(xml))
                {
                    reader = new StringReader(xml);
                    Iso8583Field[] array = serial.Deserialize(reader) as Iso8583Field[];
                    foreach (Iso8583Field field in array)
                    {

                        if (field.Flag >= 0)
                        {
                            this.AddField(field);
                        }
                        else
                        {
                            if (this.fields.ContainsKey(field.BitNum))
                            {
                                this.RemoveField(field.BitNum);
                            }
                        }
                    }
                }
            }
        }
      
      
        /// <summary>
        /// 把架构导出成文本
        /// </summary>
        /// <returns></returns>
        public string ExportToXml()
        {
            Iso8583Field[] array = new Iso8583Field[this.fields.Count];
            int i = 0;
            foreach (KeyValuePair<int, Iso8583Field> kvp in this.fields)
            {
                array[i++] = kvp.Value;
            }
            XmlSerializer serial = new XmlSerializer(typeof(Iso8583Field[]));
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            serial.Serialize(writer, array);
            return sb.ToString();
        }
        /// <summary>
        /// 保存架构到文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void SaveToFile(string fileName)
        {
            string xml = this.ExportToXml();
            StreamWriter writer = new StreamWriter(fileName);
            writer.Write(xml);
            writer.Close();
        }
    }
}
