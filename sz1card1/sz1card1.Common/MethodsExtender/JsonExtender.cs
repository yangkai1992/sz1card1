///<summary>
///Copyright (C) 深圳市一卡易科技发展有限公司
///创建标识：2009-05-11 Created by pq
///功能说明：提供Json序列化对象以及反序列化Json字符串的扩展方法类
///注意事项：待反序列化的字符串必须是Json格式
///修改记录：
///2009-05-17 [by pq] 新增对匿名对象的Json序列化
///</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Data;
using System.Collections;

namespace sz1card1.Common
{
    /// <summary>
    /// Json扩展方法类
    /// </summary>
    public static class JsonExtender
    {
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T">待序列化的类型</typeparam>
        /// <param name="obj">待序列化的对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson<T>(this T obj) where T : class
        {
            if (obj.GetType().GetCustomAttributes(typeof(DataContractAttribute), true).Length > 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
                string output = string.Empty;
                using (MemoryStream ms = new MemoryStream())
                {
                    ser.WriteObject(ms, obj);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length));
                    output = sb.ToString();
                }
                return output;
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 4194304;
                return serializer.Serialize(obj);
            }
        }


        /// <summary>
        /// DataTable转json
        /// </summary>
        /// <param name="obj">DataTable转json</param>
        /// <returns>Json字符串</returns>
        public static string DataTableToJson(this DataTable dt)
        //public static string DataTableToJson(DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值  
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合  
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值  
            }
            return javaScriptSerializer.Serialize(arrayList);
        }
        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T">待反序列化的类型</typeparam>
        /// <param name="jsonString">待反序列化的Json字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T FromJson<T>(this string jsonString) where T : class
        {
            T ouput = null;
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                ouput = serializer.Deserialize<T>(jsonString);
            }
            else
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    ouput = (T)ser.ReadObject(ms);
                }
            }
            return ouput;
        }
    }
}
