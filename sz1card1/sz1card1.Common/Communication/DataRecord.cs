///<summary>
///注意事项：
///添加了InsertRow方法，用于在某一位置插入数据
///</summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace sz1card1.Common.Communication
{
    public class DataRecord : IEnumerable
    {
        private List<string> columns;
        private List<Dictionary<string, string>> rows;

        public DataRecord()
        {
            rows = new List<Dictionary<string, string>>();
            columns = new List<string>();
        }

        public List<string> Columns
        {
            get
            {
                return columns;
            }
        }

        public Dictionary<string, List<string>> Column
        {
            get
            {
                return GetColumns();
            }
        }

        public List<Dictionary<string, string>> Rows
        {
            get
            {
                return rows;
            }
        }

        /// <summary>
        /// 添加表头
        /// </summary>
        /// <param name="args"></param>
        public void AddColumns(params object[] args)
        {
            foreach (object obj in args)
                columns.Add(obj.ToString());
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="args"></param>
        public void AddRow(params object[] args)
        {
            Dictionary<string, string> dictRow = new Dictionary<string, string>();
            if (columns.Count > 0)
            {
                if (args.Length <= columns.Count)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (args.Length > i)
                        {
                            dictRow.Add(columns[i], args[i].ToString());
                        }
                        else
                        {
                            dictRow.Add(columns[i], "");
                        }
                    }
                    rows.Add(dictRow);
                }
                else
                {
                    throw new ArgumentException("输入数组长度大于此表中的列数。");
                }
            }
            else
            {
                throw new ArgumentException("header 不能为 null。");
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="list"></param>
        public void AddRow(List<string> list)
        {
            Dictionary<string, string> dictRow = new Dictionary<string, string>();
            if (columns.Count > 0)
            {
                if (list.Count <= columns.Count)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (list.Count > i)
                        {
                            dictRow.Add(columns[i], list[i]);
                        }
                        else
                        {
                            dictRow.Add(columns[i], "");
                        }
                    }
                    rows.Add(dictRow);
                }
                else
                {
                    throw new ArgumentException("输入数组长度大于此表中的列数。");
                }
            }
            else
            {
                throw new ArgumentException("columns 不能为 null。");
            }
        }

        /// <summary>
        /// 插入一行数据
        /// </summary>
        /// <param name="index">插入行序号</param>
        /// <param name="args">依照列位置输入要插入的数据,数量少于列数时，其他列为""</param>
        public void InsertRow(int index, params object[] args)
        {
            Dictionary<string, string> dictRow = new Dictionary<string, string>();
            if (index < 0)
            {
                throw new ArgumentException("插入行序号不能小于0");
            }else if (index >= rows.Count)
            {
                throw new ArgumentException("插入行序号大于此表中的行数");
            }

            if (columns.Count > 0)
            {
                if (args.Length <= columns.Count)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (args.Length > i)
                        {
                            dictRow.Add(columns[i], args[i].ToString());
                        }
                        else
                        {
                            dictRow.Add(columns[i], "");
                        }
                    }
                    rows.Insert(index,dictRow);
                }
                else
                {
                    throw new ArgumentException("输入数组长度大于此表中的列数。");
                }
            }
            else
            {
                throw new ArgumentException("header 不能为 null。");
            }
        }
        /// <summary>
        /// 插入一行数据
        /// </summary>
        /// <param name="index">插入行序号</param>
        /// <param name="list">依照列位置输入要插入的数据,数量少于列数时，其他列为""</param>
        public void InsertRow(int index,List<string> list)
        {
            Dictionary<string, string> dictRow = new Dictionary<string, string>();
            if (index < 0)
            {
                throw new ArgumentException("插入行序号不能小于0");
            }
            else if (index >= rows.Count)
            {
                throw new ArgumentException("插入行序号大于此表中的行数");
            }

            if (columns.Count > 0)
            {
                if (list.Count <= columns.Count)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (list.Count > i)
                        {
                            dictRow.Add(columns[i], list[i]);
                        }
                        else
                        {
                            dictRow.Add(columns[i], "");
                        }
                    }
                    rows.Insert(index,dictRow);
                }
                else
                {
                    throw new ArgumentException("输入数组长度大于此表中的列数。");
                }
            }
            else
            {
                throw new ArgumentException("columns 不能为 null。");
            }
        }

        private Dictionary<string, List<string>> GetColumns()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            foreach (string column in columns)
            {
                List<string> row = new List<string>();
                foreach (Dictionary<string, string> item in rows)
                {
                    row.Add(item[column]);
                }
                dict.Add(column, row);
            }
            return dict;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (columns.Count > 0)
            {
                foreach (string col in columns)
                {
                    sb.Append(col);
                    sb.Append(Separator.US);
                }
                sb = sb.Remove(sb.Length - 1, 1);
                foreach (Dictionary<string, string> row in rows)
                {
                    sb.Append(Separator.RS);
                    foreach (string key in row.Keys)
                    {
                        sb.Append(row[key]);
                        sb.Append(Separator.US);
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        public static DataRecord Parse(string str)
        {
            DataRecord dataRecord = new DataRecord();
            string[] rows = str.Split(Separator.RS);
            for (int i = 0; i < rows.Length; i++)
            {
                string[] row = rows[i].Split(Separator.US);
                if (i == 0)
                {
                    foreach (string ss in row)
                    {
                        dataRecord.columns.Add(ss);
                    }
                }
                else
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    for (int j = 0; j < row.Length; j++)
                    {
                        dict.Add(dataRecord.columns[j], row[j]);
                    }
                    dataRecord.rows.Add(dict);
                }
            }
            return dataRecord;
        }

        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        #endregion
    }
}
