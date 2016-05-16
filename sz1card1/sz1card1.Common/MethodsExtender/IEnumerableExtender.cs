///功能说明：提供将IEnumerable<T>转化为字典或DataTable方法
///注意事项：
///1，除数据外，加入了length，方便Javascript中处理数据
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using sz1card1.Common.Communication;

namespace sz1card1.Common
{
    public static class IEnumerableExtender
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> array)
        {
            DataTable dt = new DataTable();
            foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
            {
                DataColumn column = new DataColumn();
                column.AllowDBNull = true;
                column.ColumnName = dp.Name;
                if (dp.PropertyType.IsGenericType)
                {
                    if (dp.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        //可空类型判断
                        Type[] typeArray = dp.PropertyType.GetGenericArguments();
                        Type nullableType = typeArray[0];
                        column.DataType = nullableType;
                    }
                    else if (dp.PropertyType.GetGenericTypeDefinition().Equals(typeof(EntitySet<>)))
                    {
                        continue;
                    }
                }
                else if (typeof(INotifyPropertyChanging).IsAssignableFrom(dp.PropertyType))
                {
                    continue;
                }
                else
                {
                    column.DataType = dp.PropertyType;
                }
                dt.Columns.Add(column);
            }
            foreach (T item in array)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                {
                    if (dt.Columns.Contains(dp.Name))
                    {
                        dr[dp.Name] = dp.GetValue(item) == null ? DBNull.Value : dp.GetValue(item);
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable ToDataTable(this IEnumerable array)
        {
            DataTable dt = new DataTable();
            int index = 0;
            foreach (object item in array)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(item.GetType()))
                {
                    if (index == 0)
                    {
                        dt.Columns.Add(dp.Name);
                    }
                    dr[dp.Name] = dp.GetValue(item);
                }
                dt.Rows.Add(dr);
                index++;
            }
            return dt;
        }

        public static Dictionary<string, object> ToDictionary<T>(this IEnumerable<T> array)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int index = 0;
            foreach (T item in array)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                {
                    if (dp.PropertyType == typeof(System.DateTime))
                    {
                        result.Add(dp.Name, ((DateTime)dp.GetValue(item)).ToString("yyyy-MM-dd hh:mm:ss"));
                    }
                    else
                    {
                        result.Add(dp.Name, dp.GetValue(item));
                    }
                }
                dic.Add(index.ToString(), result);
                index++;
            }
            dic.Add("length", array.Count<T>());
            return dic;
        }

        /// <summary>
        /// 转化为DataRecord类（泛型）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataRecord ToDataRecord<T>(this IEnumerable<T> array)
        {
            DataRecord dataRecord = new DataRecord();
            foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
            {
                dataRecord.Columns.Add(dp.Name);
            }
            foreach (T item in array)
            {
                List<string> row = new List<string>();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                {
                    row.Add(dp.GetValue(item).ToString());
                }
                dataRecord.AddRow(row);
            }
            return dataRecord;
        }

        /// <summary>
        /// 转化为DataRecord类（非泛型）
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static DataRecord ToDataRecord(this IEnumerable array)
        {
            DataRecord dataRecord = new DataRecord();
            int index = 0;
            foreach (object item in array)
            {
                List<string> row = new List<string>();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(item.GetType()))
                {
                    if (index == 0)
                    {
                        dataRecord.Columns.Add(dp.Name);
                    }
                    if (dp.GetValue(item) == null)
                    {
                        row.Add(string.Empty);
                    }
                    else
                    {
                        row.Add(dp.GetValue(item).ToString());
                    }
                }
                dataRecord.AddRow(row);
                index++;
            }
            return dataRecord;
        }

        /// <summary>
        /// 数组集合转换成带逗号的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ConvertToString<T>(this IEnumerable<T> array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in array)
            {
                sb.Append(item.ToString() + ",");
            }
            if (array.Count() > 0)
            {
                return sb.ToString(0, sb.Length - 1);
            }
            return "";
        }
    }
}
