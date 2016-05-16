using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Iso8583Communication
{
    /// <summary>
    /// 表示 ISO 8583 包的数据域定义
    /// </summary>
    [Serializable]
    public class Iso8583Field
    {
        private int bitNum;
        private string fieldName;
        private Iso8583DataType dataType;
        private Iso8583Property property;
        private int length;
        private Iso8583Format format;
        private string description;
        private int flag;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Iso8583Field()
        {
        }
        /// <summary>
        /// 用指定参数构造实例
        /// </summary>
        /// <param name="bitNum">位域</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="property">属性</param>
        /// <param name="length">长度</param>
        /// <param name="format">格式设置</param>
        /// <param name="flag">标识,0为自动生成（非参数）,1为手动设置（参数）</param>
        public Iso8583Field(int bitNum, string fieldName, Iso8583DataType dataType, Iso8583Property property, int length, Iso8583Format format, int flag)
        {
            this.bitNum = bitNum;
            this.fieldName = fieldName;
            this.dataType = dataType;
            this.property = property;
            this.flag = flag;
            this.length = length;
            if (dataType != Iso8583DataType.B)
                this.format = format;
            switch (this.format)
            {
                case Iso8583Format.YYYYMMDD:
                    this.length = 8;
                    break;
                case Iso8583Format.hhmmss:
                    this.length = 6;
                    break;
                case Iso8583Format.YYYY:
                case Iso8583Format.YYMM:
                case Iso8583Format.MMDD:
                    this.length = 4;
                    break;
                case Iso8583Format.MMDDhhmmss:
                    this.length = 10;
                    break;
                case Iso8583Format.LVAR:
                    if (length > 9) this.length = 9;
                    break;
                case Iso8583Format.LLVAR:
                    if (length > 99) this.length = 99;
                    break;
                case Iso8583Format.LLLVAR:
                    if (length > 999) this.length = 999;
                    break;
            }
        }

        /// <summary>
        /// 位域
        /// </summary>
        public int BitNum
        {
            get { return this.bitNum; }
            set { this.bitNum = value; }
        }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get { return this.fieldName; }
            set { this.fieldName = value; }
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        public Iso8583DataType DataType
        {
            get { return this.dataType; }
            set { this.dataType = value; }
        }
        /// <summary>
        /// 属性
        /// </summary>
        public Iso8583Property Property
        {
            get { return this.property; }
            set { this.property = value; }
        }
        /// <summary>
        /// 长度（定长域）或最大长度（变长域）
        /// </summary>
        public int Length
        {
            get { return this.length; }
            set { this.length = value; }
        }
        /// <summary>
        /// 格式设置
        /// </summary>
        public Iso8583Format Format
        {
            get { return this.format; }
            set { this.format = value; }
        }
        /// <summary>
        /// 标识
        /// </summary>
        public int Flag
        {
            get { return this.flag; }
            set { this.flag = value; }
        }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
    }
}
