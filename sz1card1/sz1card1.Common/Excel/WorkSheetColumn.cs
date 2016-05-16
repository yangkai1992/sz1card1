///<summary>
///Copyright (C) 深圳市一卡易科技发展有限公司
///创建标识：2009-12-01 Created by pq
///功能说明：工作表列模型(标题，数据类型，宽度)
///注意事项：
///</summary>
using System;
using System.Collections.Generic;
using System.Text;
using sz1card1.Common.Enum;

namespace sz1card1.Common.Excel
{
    public class WorkSheetColumn
    {
        private string columnName;
        private ColDataType dataType;
        private decimal width;

        public WorkSheetColumn()
        {
        }

        public WorkSheetColumn(string columnName)
        {
            this.columnName = columnName;
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = value;
            }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public ColDataType DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
            }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public decimal Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
    }
}
