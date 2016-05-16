///<summary>
///Copyright (C) 深圳市一卡易科技发展有限公司
///创建标识：2009-12-01 Created by pq
///功能说明：提供创建Excel工作表的功能
///注意事项：
///</summary>
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using sz1card1.Common.Enum;

namespace sz1card1.Common.Excel
{
    public class WorkSheet
    {
        private string sheetName;
        public List<WorkSheetColumn> columns;
        private Workbook parent;
        public int rowsCount = 0;
        public List<object[]> rows;

        public WorkSheet()
        {
            columns = new List<WorkSheetColumn>();
            rows = new List<object[]>();
        }

        public WorkSheet(string sheetName)
            : this()
        {
            this.sheetName = sheetName;
        }

        /// <summary>
        /// 工作表名
        /// </summary>
        public string SheetName
        {
            get
            {
                return sheetName;
            }
            set
            {
                sheetName = value;
            }
        }

        /// <summary>
        /// 列
        /// </summary>
        public List<WorkSheetColumn> Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }

        public Workbook Parent
        {
            get
            {
                return parent;
            }
            internal set
            {
                parent = value;
            }
        }

        public int RowsCount
        {
            get
            {
                return rowsCount;
            }
        }

        public void AddColumn(string columnName)
        {
            AddColumn(columnName, ColDataType.String);
        }

        public void AddColumn(string columnName, ColDataType dataType)
        {
            AddColumn(columnName, dataType, 80);
        }

        public void AddColumn(string columnName, ColDataType dataType, decimal width)
        {
            WorkSheetColumn column = new WorkSheetColumn(columnName);
            column.DataType = dataType;
            column.Width = width;
            columns.Add(column);
        }

        internal void InitHeader()
        {
            if (parent == null)
                throw new NullReferenceException("parent");
            Parent.Writer.Write(string.Format("<Worksheet ss:Name=\"{0}\">", SheetName));
            Parent.Writer.Write(string.Format("<Table ss:ExpandedColumnCount=\"{0}\" ss:ExpandedRowCount=\"{1}\" x:FullColumns=\"1\" x:FullRows=\"1\" ss:DefaultColumnWidth=\"80\" ss:DefaultRowHeight=\"14.25\">", columns.Count, rowsCount + 1));
            foreach (WorkSheetColumn column in columns)
            {
                Parent.Writer.Write(string.Format("<Column ss:StyleID=\"s{0}\" {1}/>", (int)column.DataType, column.Width != 0 ? "ss:Width=\"" + column.Width.ToString() + "\"" : ""));
            }
            Parent.Writer.Write("<Row ss:StyleID=\"s23\">");
            foreach (WorkSheetColumn column in columns)
            {
                Parent.Writer.Write(string.Format(@"<Cell>
                                                        <Data ss:Type={0}String{0}>{1}</Data>
                                                    </Cell>", "\"", column.ColumnName));
            }
            Parent.Writer.Write("</Row>");
        }

        internal void InitFooter()
        {
            Parent.Writer.Write("</Table>");
            Parent.Writer.Write(string.Format(@"<WorksheetOptions xmlns={0}urn:schemas-microsoft-com:office:excel{0}>
                                                    <ProtectObjects>False</ProtectObjects>
                                                    <ProtectScenarios>False</ProtectScenarios>
                                                </WorksheetOptions>", "\""));
            Parent.Writer.Write("</Worksheet>");
        }

        internal void InitRows()
        {
            for (int i = 0; i < rowsCount; i++)
            {
                Parent.Writer.Write("<Row>");
                for (int j = 0; j < columns.Count; j++)
                {
                    if (columns[j].DataType == ColDataType.String)
                    {
                        //表头为文本型
                        Parent.Writer.Write(string.Format("<Cell><Data ss:Type=\"{0}\">{1}</Data></Cell>", columns[j].DataType, rows[i][j].ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("&", "&amp;")));
                    }
                    else
                    {
                        Parent.Writer.Write(string.Format("<Cell><Data ss:Type=\"{0}\">{1}</Data></Cell>", columns[j].DataType, rows[i][j]));
                    }
                }
                Parent.Writer.Write("</Row>");
            }
        }

        public void WriteRows(params object[] args)
        {
            if (args.Length < columns.Count)
                throw new ArgumentException("args");
            rows.Add(args);
            rowsCount++;
        }
    }
}
