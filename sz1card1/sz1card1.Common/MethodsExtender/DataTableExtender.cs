///功能说明：提供将DataTable转化为字典方法
///注意事项：
///1，除数据外，加入了length，方便Javascript中处理数据
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;

namespace sz1card1.Common
{
    public static class DataTableExtender
    {
        public static Dictionary<string, object> ToDictionary(this DataTable dt)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int index = 0;
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dr[dc].GetType() == typeof(System.DateTime))
                    {
                        result.Add(dc.ColumnName, ((DateTime)dr[dc]).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        result.Add(dc.ColumnName, dr[dc]);
                    }
                }
                dic.Add(index.ToString(), result);
                index++;
            }
            dic.Add("length", dt.Rows.Count);
            return dic;
        }

        public static List<object> ToObject(this DataTable dt)
        {
            List<object> dic = new List<object>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dr[dc].GetType() == typeof(System.DateTime))
                    {
                        result.Add(dc.ColumnName, ((DateTime)dr[dc]).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        result.Add(dc.ColumnName, dr[dc]);
                    }
                }
                dic.Add(result);
            }
            return dic;
        }

        public static Dictionary<string, object> ToDictionary_byServerTime(this DataTable dt)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int index = 0;
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc]);
                }
                dic.Add(index.ToString(), result);
                index++;
            }
            dic.Add("length", dt.Rows.Count);
            return dic;
        }

        public static Dictionary<string, object> ToDictionary_memberServiceNote(this DataTable dt)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int index = 0;
            DataRow drAddNew = dt.NewRow();
            drAddNew["Guid"] = Guid.NewGuid().ToString();
            drAddNew["Status"] = 3;
            drAddNew["ServiceTitle"] = "新建服务计划";
            drAddNew["AlertDateType"] = 0;
            drAddNew["IsPublic"] = true;
            dt.Rows.Add(drAddNew);
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc]);
                }
                dic.Add(index.ToString(), result);
                index++;
            }
            dic.Add("length", dt.Rows.Count);
            return dic;
        }

        public static Dictionary<string, object> ToDictionary_serviceNote(this DataTable dt)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int index = 0;
            DataRow drAddNew = dt.NewRow();
            drAddNew["Title"] = "新建代办事项";
            drAddNew["Status"] = 3;
            drAddNew["AlertDateType"] = 0;
            drAddNew["IsPublic"] = true;
            dt.Rows.Add(drAddNew);
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc]);
                }
                dic.Add(index.ToString(), result);
                index++;
            }
            dic.Add("length", dt.Rows.Count);
            return dic;
        }

        public static DataTable CopyToDataTable<T>(this IEnumerable<T> array)
        {
            var ret = new DataTable();
            foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                try
                {
                    ret.Columns.Add(dp.Name, dp.PropertyType);
                }
                catch
                {
                    ret.Columns.Add(dp.Name);
                }
            foreach (T item in array)
            {
                var Row = ret.NewRow();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                    Row[dp.Name] = dp.GetValue(item);
                ret.Rows.Add(Row);
            }
            return ret;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static DataTable Paged(this DataTable dt, string fields, string where, string orderBy, int pageIndex, int pageSize, out int total)
        {
            total = dt.Rows.Count;
            if (dt.Rows.Count == 0)
            {
                DataTable dtFields = null;
                if (!string.IsNullOrEmpty(fields) && fields != "*")
                {
                    string[] field = fields.Split(',');
                    dtFields = dt.DefaultView.ToTable(false, field);
                }
                else
                {
                    dtFields = dt;
                }
                return dt;
            }
            //过滤
            DataTable dtFilter = null;
            if (string.IsNullOrEmpty(where))
            {
                dtFilter = dt;
            }
            else
            {
                DataRow[] drs = dt.Select(where);
                total = drs.Count();
                dtFilter = dt.Clone();
                if (total == 0)
                {
                    DataTable dtFields = null;
                    if (!string.IsNullOrEmpty(fields) && fields != "*")
                    {
                        string[] field = fields.Split(',');
                        dtFields = dtFilter.DefaultView.ToTable(false, field);
                    }
                    else
                    {
                        dtFields = dtFilter;
                    }
                    return dtFilter;
                }
                foreach (DataRow dr in drs)
                {
                    dtFilter.ImportRow(dr);
                }
            }

            //排序
            if (!string.IsNullOrEmpty(orderBy))
            {
                DataView dvOrderBy = dtFilter.DefaultView;
                dvOrderBy.Sort = orderBy;
                dtFilter = dvOrderBy.ToTable();
            }

            //分页
            if (pageIndex < 0)
            {
                DataTable dtFields = null;
                if (!string.IsNullOrEmpty(fields) && fields != "*")
                {
                    string[] field = fields.Split(',');
                    dtFields = dtFilter.DefaultView.ToTable(false, field);
                }
                else
                {
                    dtFields = dtFilter;
                }
                return dtFilter;
            }
            DataTable dtPaged = dt.Clone();
            int rowBegin = pageIndex * pageSize;//当前页的第一条数据在dt中的位置
            int rowEnd = rowBegin + pageSize;//当前页的最后一条数据在dt中的位置
            if (rowBegin >= dtFilter.Rows.Count)
            {
                if (!string.IsNullOrEmpty(fields) && fields != "*")
                {
                    string[] field = fields.Split(',');
                    dtPaged = dtFilter.DefaultView.ToTable(false, field);
                }
                else
                {
                    dtPaged = dtFilter;
                }
                return dtPaged;
            }
            if (rowEnd > dtFilter.Rows.Count)
            {
                rowEnd = dtFilter.Rows.Count;
            }
            DataView dvPaged = dtFilter.DefaultView;
            for (int i = rowBegin; i <= rowEnd - 1; i++)
            {
                dtPaged.ImportRow(dvPaged[i].Row);
            }
            if (!string.IsNullOrEmpty(fields) && fields != "*")
            {
                string[] field = fields.Split(',');
                dtPaged = dtPaged.DefaultView.ToTable(false, field);
            }
            return dtPaged;
        }


        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionaryNoHtml(this DataTable dt)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int index = 0;
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dr[dc].GetType() == typeof(System.DateTime))
                    {
                        result.Add(dc.ColumnName, ((DateTime)dr[dc]).ToString("yyyy年MM月dd日"));
                    }
                    else
                    {
                        result.Add(dc.ColumnName, ClearHtml(dr[dc].ToString()));
                    }
                }
                dic.Add(index.ToString(), result);
                index++;
            }
            dic.Add("length", dt.Rows.Count);
            return dic;
        }

        /// <summary>
        /// 过滤所有html标签
        /// </summary>
        /// <param name="Htmlstring">过滤的内容</param>
        /// <returns></returns>
        public static string ClearHtml(string Htmlstring)
        {
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
    }
}
