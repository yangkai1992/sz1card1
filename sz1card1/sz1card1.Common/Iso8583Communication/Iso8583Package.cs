using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace sz1card1.Common.Iso8583Communication
{
    /// <summary>
    /// ISO 8583 数据包类
    /// </summary>
    public class Iso8583Package
    {
        private int length = 0;
        private string messageType = string.Empty;
        private string deviceId = string.Empty;
        private string headInfo = string.Empty;
        private string processCode = string.Empty;
        private string bitmapStr = string.Empty;
        private string xmlPath = string.Empty;
        private string macKey = string.Empty;

        private IsoBitmap bitmap;
        private Iso8583Schema schema;
        private SortedList<int, object> values;
        private byte[] buffer;
        //不用每次读取xml
        private static XmlDocument[] xmlDocArray = null;
        private static Object lockObj = new object();

        private static byte[] MAC_KEY = { 0x73, 0x7a, 0x31, 0x63, 0x61, 0x72, 0x64, 0x31 };
        #region 构造函数

        /// <summary>
        /// 使用xml文件名构造数据包类
        /// </summary>
        public Iso8583Package(string xmlPath)
        {
            this.bitmap = new IsoBitmap();
            this.values = new SortedList<int, object>(IsoBitmap.FieldCount);
            this.headInfo = "6000200000";
            this.xmlPath = xmlPath;
        }
        #endregion

        #region 初始化方法
        /// <summary>
        /// 根据消息类型和消息处理码，生成数据域列表
        /// </summary>
        private void SetSchema()
        {
            if (this.xmlPath != string.Empty)
            {
                InitXmlDocument();
                string[] xmlArray = new string[2];
                string typeEnd = messageType.Substring(2, 2);

                if (typeEnd == "20")
                    typeEnd = "00";
                if (typeEnd == "30")
                    typeEnd = "10";
                xmlArray[0] = xmlDocArray[0].SelectSingleNode(string.Format("mappings/mapping[@messageType='{0}']", "**" + typeEnd)).InnerXml;
                //签到或者签退，不需要匹配processCode
                if (messageType != "0400" && messageType != "0410")
                {
                    xmlArray[1] = xmlDocArray[1].SelectSingleNode(string.Format("mappings/mapping[@messageType='{0}' and @processCode='{1}']", messageType, processCode)).InnerXml;
                }
                else
                {
                    xmlArray[1] = xmlDocArray[1].SelectSingleNode(string.Format("mappings/mapping[@messageType='{0}']", messageType)).InnerXml;
                }
                this.schema = new Iso8583Schema(xmlArray);
            }
        }

        /// <summary>
        /// 生成单例xmlDoc
        /// </summary>
        private void InitXmlDocument()
        {
            if (xmlDocArray == null)
            {
                lock (lockObj)
                {
                    if (xmlDocArray == null)
                    {
                        xmlDocArray = new XmlDocument[2];
                        xmlDocArray[0] = new XmlDocument();
                        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        if (baseDirectory[baseDirectory.Length - 1] == '\\')
                        {
                            baseDirectory = baseDirectory.Remove(baseDirectory.Length - 1);
                        }
                        xmlDocArray[0].Load(baseDirectory + this.xmlPath.Replace("Iso8583", "Iso8583_Base"));
                        xmlDocArray[1] = new XmlDocument();
                        xmlDocArray[1].Load(baseDirectory + this.xmlPath.Replace("Iso8583", "Iso8583_Advance"));
                    }
                }
            }
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// XML文件路径
        /// </summary>
        public string XmlPath
        {
            get { return this.xmlPath; }
            set { this.xmlPath = value; }
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType
        {
            get { return this.messageType; }
            set
            {
                if (value.Length != 4)
                    throw new Exception("消息类型长度不正确。");
                this.messageType = value;
                if (this.processCode != string.Empty)
                {
                    SetSchema();
                }
            }
        }
        /// <summary>
        /// Mac密钥
        /// </summary>
        public string MacKey
        {
            set { this.macKey = value; }
        }
        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceId
        {
            get { return this.deviceId; }
        }
        /// <summary>
        /// 消息头，作用未知
        /// </summary>
        public string HeadInfo
        {
            get { return headInfo; }
        }
        /// <summary>
        /// 消息总长度，不包括前两个字节
        /// </summary>
        public int Length
        {
            get { return length; }
        }
        /// <summary>
        /// 消息处理码
        /// </summary>
        public string ProcessCode
        {
            get { return processCode; }
            set
            {
                if (value.Length != 6)
                {
                    throw new Exception("处理码长度不正确。");
                }
                processCode = value;
                if (this.messageType != string.Empty)
                {
                    SetSchema();
                }
            }
        }
        #endregion


        #region 获取和设置需要人工处理的参数

        public bool CheckMacValue()
        {
            if (buffer != null)
            {
                //如果有64域，那么校验mac码
                if (bitmap.Get(64))
                {
                    if (string.IsNullOrEmpty(macKey))
                    {
                        Log.LoggingService.Info("未设置MAC密钥");
                        macKey = Encoding.ASCII.GetString(MAC_KEY);
                    }
                    if (macKey.Length != 8)
                    {
                        throw new Exception("MAC密钥的长度不正确");
                    }
                    byte[] checkData = new byte[buffer.Length - 15];
                    Array.Copy(buffer, 7, checkData, 0, buffer.Length - 15);
                    byte[] MAC_IV = new byte[8];
                    byte[] mac_Cal = Iso8583Mac_CBC.MAC_CBC(Encoding.ASCII.GetBytes(macKey), MAC_IV, checkData);
                    byte[] mac_Receive = new byte[8];
                    Array.Copy(buffer, buffer.Length - 8, mac_Receive, 0, 8);
                    for (int i = 0; i < 8; i++)
                    {
                        if (mac_Receive[i] != mac_Cal[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                throw new Exception("未设置数据包!");
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public object[] GetParam()
        {
            if (this.schema == null)
            {
                throw new Exception("数据包类没有正确初始化!");
            }
            if (this.values.Count == 0)
            {
                throw new Exception("数据未解包!");
            }
            IsoBitmap map = this.schema.bitmap;
            List<object> objs = new List<object>();
            for (int bitNum = 2; bitNum < IsoBitmap.FieldCount; bitNum++)
            {
                if (map.Get(bitNum))
                {
                    Iso8583Field field = this.schema.fields[bitNum];
                    if (field.Flag == 1)
                    {
                        if (ExistValue(bitNum))
                        {
                            objs.Add(this.values[bitNum]);
                        }
                        else
                        {
                            objs.Add(string.Empty);
                        }
                    }
                }
            }
            return objs.ToArray();
        }


        /// <summary>
        /// 获取需要手动配置参数的位域
        /// </summary>
        /// <returns></returns>
        public int[] GetParamBitNums()
        {
            Dictionary<int, bool> dic = GetAllBitNums();
            List<int> paramBitNums = new List<int>();
            if (dic.Keys.Count > 0)
            {
                foreach (int bitNum in dic.Keys)
                {
                    if (dic[bitNum] == true)
                    {
                        paramBitNums.Add(bitNum);
                    }
                }
            }
            return paramBitNums.ToArray();
        }

        /// <summary>
        /// 获取所有位域
        /// </summary>
        /// <returns>int:位域值 bool:是否需要手动配置</returns>
        public Dictionary<int, bool> GetAllBitNums()
        {
            if (this.schema == null)
            {
                throw new Exception("数据包类没有正确初始化!");
            }
            IsoBitmap map = this.schema.bitmap;
            Dictionary<int, bool> dic = new Dictionary<int, bool>();
            for (int bitNum = 2; bitNum < IsoBitmap.FieldCount; bitNum++)
            {
                if (map.Get(bitNum))
                {
                    Iso8583Field field = this.schema.fields[bitNum];
                    dic.Add(bitNum, field.Flag == 1);
                }
            }
            return dic;
        }
        /// <summary>
        /// 获取方法
        /// </summary>
        /// <returns></returns>
        public string GetAction()
        {
            InitXmlDocument();
            string action;
            if (messageType != "0400")
            {
                action = xmlDocArray[1].SelectSingleNode(string.Format("mappings/mapping[@processCode='{0}']", processCode)).Attributes["actionName"].Value;
            }
            else
            {
                action = xmlDocArray[1].SelectSingleNode(string.Format("mappings/mapping[@messageType='{0}']", messageType)).Attributes["actionName"].Value;
            }
            return action;
        }


        /// <summary>
        /// 获取数据域域描述
        /// </summary>
        /// <param name="bitNum"></param>
        /// <returns></returns>
        public string GetDescription(int bitNum)
        {
            if (this.schema != null && this.schema.fields != null && this.schema.fields.Keys.Contains(bitNum))
            {
                return this.schema.fields[bitNum].Description;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 为数据域设置值
        /// <summary>
        /// 清除所有数据。
        /// </summary>
        public void Clear()
        {
            this.bitmap = new IsoBitmap();
            this.values = new SortedList<int, object>(IsoBitmap.FieldCount);
            this.schema = null;
            this.length = 0;
            this.messageType = string.Empty;
            this.processCode = string.Empty;
            this.macKey = string.Empty;
            this.buffer = null;
        }

        /// <summary>
        /// 为位域设置值
        /// </summary>
        /// <param name="valuePairs">所有位域和值的字典</param>
        public void SetValue(Dictionary<int, object> valuePairs)
        {
            foreach (KeyValuePair<int, object> valuePair in valuePairs)
            {
                SetValue(valuePair.Key, valuePair.Value);
            }
        }
        /// <summary>
        /// 为位域设置值
        /// </summary>
        /// <param name="bitNum">指定位域</param>
        /// <param name="value">值</param>
        public void SetValue(int bitNum, object value)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
            {
                throw new Exception(String.Format("数据包定义不包含此域：{0}", bitNum));
            }
            Iso8583Field field = this.schema.fields[bitNum];
            Type type = value.GetType();
            switch (type.FullName)
            {
                case "System.Int32":
                    SetNumber(bitNum, (int)value);
                    break;
                case "System.Decimal":
                    SetMoney(bitNum, (decimal)value);
                    break;
                case "System.DateTime":
                    SetDateTime(bitNum, (DateTime)value);
                    break;
                case "System.Byte[]":
                    SetArrayData(bitNum, (byte[])value);
                    break;
                case "System.String":
                    SetString(bitNum, value.ToString());
                    break;
                default:
                    throw new Exception(string.Format("[{0}]域格式不正确!", bitNum));
            }
        }
        /// <summary>
        /// 为指定数据域设置一个字符串值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <param name="value">字符串值</param>
        public void SetString(int bitNum, string value)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
                throw new Exception(String.Format("数据包定义不包含此域：{0}", bitNum));
            Iso8583Field field = this.schema.fields[bitNum];
            if (Encoding.Default.GetByteCount(value) > field.Length)
                throw new Exception(string.Format("[{0}]域长度过长。", bitNum));
            switch (field.DataType)
            {
                case Iso8583DataType.B:
                    throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
                default:
                    //设置数据格式
                    value = Utility.SetFormat(value, field);
                    values[bitNum] = value;
                    break;
            }
            this.bitmap.Set(bitNum, value != null);
        }
        /// <summary>
        /// 为指定数据域设置一个数字值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <param name="value">数字值</param>
        public void SetNumber(int bitNum, Int64 value)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
                throw new Exception(String.Format("数据包定义不包含此域：{0}", bitNum));
            Iso8583Field field = this.schema.fields[bitNum];
            string strValue = value.ToString();
            if (strValue.Length > field.Length)
                throw new ArgumentException(string.Format("[{0}]域数值过大。", bitNum), "value");
            switch (field.DataType)
            {
                case Iso8583DataType.B:
                    throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
                default:
                    //设置数据格式
                    strValue = Utility.SetFormat(strValue, field);
                    values[bitNum] = strValue;
                    break;
            }
            this.bitmap.Set(bitNum, true);
        }
        /// <summary>
        /// 为指定数据域设置一个金额值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <param name="money">金额值</param>
        public void SetMoney(int bitNum, decimal money)
        {
            int result = money.ToString().IndexOf('.');
            if (result != -1 && money.ToString().Length - result > 3)
            {
                throw new Exception(string.Format("[{0}]域小数位不能多于两位!", bitNum));
            }
            Int64 value = Convert.ToInt64(money * 100);
            this.SetNumber(bitNum, value);
        }
        /// <summary>
        /// 为指定数据域设置一个日期值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <param name="time">日期值</param>
        public void SetDateTime(int bitNum, DateTime time)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
                throw new Exception(String.Format("数据包定义不包含此域：[{0}]", bitNum));
            Iso8583Field field = this.schema.fields[bitNum];
            switch (field.DataType)
            {
                case Iso8583DataType.B:
                    throw new Exception(string.Format("[{0}]域格式不符，不能为byte类型设置日期值", bitNum));
                default:
                    switch (field.Format)
                    {
                        case Iso8583Format.YYYYMMDD:
                            values[bitNum] = time.ToString("yyyyMMdd");
                            break;
                        case Iso8583Format.YYYY:
                            values[bitNum] = time.ToString("yyyy");
                            break;
                        case Iso8583Format.YYMM:
                            values[bitNum] = time.ToString("yyMM");
                            break;
                        case Iso8583Format.MMDD:
                            values[bitNum] = time.ToString("MMdd");
                            break;
                        case Iso8583Format.hhmmss:
                            values[bitNum] = time.ToString("HHmmss");
                            break;
                        case Iso8583Format.MMDDhhmmss:
                            values[bitNum] = time.ToString("MMddHHmmss");
                            break;
                        default:
                            throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
                    }
                    break;
            }
            this.bitmap.Set(bitNum, true);
        }
        /// <summary>
        /// 为指定数据域设置一个二进制值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <param name="data">二进制值</param>
        public void SetArrayData(int bitNum, byte[] data)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
                throw new Exception(String.Format("数据包定义不包含此域：{0}", bitNum));
            Iso8583Field field = this.schema.fields[bitNum];
            if (data.Length > field.Length)
                throw new Exception(string.Format("[{0}]域长度过长。", bitNum));
            switch (field.DataType)
            {
                case Iso8583DataType.B:
                    values[bitNum] = data;
                    break;
                default:
                    throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
            }
            this.bitmap.Set(bitNum, data != null);
        }
        #endregion

        #region 从数据域获取值
        /// <summary>
        /// 获取某个域上是否存在有效值。
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <returns></returns>
        public bool ExistValue(int bitNum)
        {
            return this.values.ContainsKey(bitNum) && (this.values[bitNum] != null);
        }
        /// <summary>
        /// 从指定数据域获取字符串值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <returns></returns>
        public string GetString(int bitNum)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
                throw new Exception(String.Format("数据包定义不包含此域：{0}", bitNum));
            Iso8583Field field = this.schema.fields[bitNum];
            if (!this.values.ContainsKey(bitNum) || (this.values[bitNum] == null))
                throw new Exception(String.Format("数据域 [{0}] 不包含任何有效值。", bitNum));
            switch (field.DataType)
            {
                case Iso8583DataType.B:
                    throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
                default:
                    return this.values[bitNum].ToString().Trim();
            }
        }
        /// <summary>
        /// 从指定数据域获取数字值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <returns></returns>
        public int GetNumber(int bitNum)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
                throw new Exception(String.Format("数据包定义不包含此域：{0}", bitNum));
            Iso8583Field field = this.schema.fields[bitNum];
            if (!this.values.ContainsKey(bitNum) || (this.values[bitNum] == null))
                throw new Exception(String.Format("数据域 [{0}] 不包含任何有效值。", bitNum));
            switch (field.DataType)
            {
                case Iso8583DataType.B:
                    throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
                default:
                    int result;
                    if (!int.TryParse(this.values[bitNum].ToString(), out result))
                    {
                        throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
                    }
                    return Convert.ToInt32(this.values[bitNum]);
            }
        }
        /// <summary>
        /// 从指定数据域获取金额值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <returns></returns>
        public decimal GetMoney(int bitNum)
        {
            decimal money = this.GetNumber(bitNum);
            return money / 100;
        }
        /// <summary>
        /// 从指定数据域获取日期值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <returns></returns>
        public DateTime GetDateTime(int bitNum)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
                throw new Exception(String.Format("数据包定义不包含此域：[{0}]", bitNum));
            Iso8583Field field = this.schema.fields[bitNum];
            if (!this.values.ContainsKey(bitNum) || (this.values[bitNum] == null))
                throw new Exception(String.Format("数据域 [{0}] 不包含任何有效值。", bitNum));
            switch (field.DataType)
            {
                case Iso8583DataType.B:
                    throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
                default:
                    string value = (string)this.values[bitNum];
                    switch (field.Format)
                    {
                        case Iso8583Format.YYYYMMDD:
                            return DateTime.ParseExact(value, "yyyyMMdd", null);
                        case Iso8583Format.YYYY:
                            return DateTime.ParseExact(value, "yyyy", null);
                        case Iso8583Format.YYMM:
                            return DateTime.ParseExact(value, "yyMM", null);
                        case Iso8583Format.MMDD:
                            return DateTime.ParseExact(value, "MMdd", null);
                        case Iso8583Format.hhmmss:
                            return DateTime.ParseExact(value, "HHmmss", null);
                        case Iso8583Format.MMDDhhmmss:
                            return DateTime.ParseExact(value, "MMddHHmmss", null);
                        default:
                            throw new Exception(string.Format("[{0}]域格式不符。", bitNum));
                    }
            }
        }
        /// <summary>
        /// 从指定数据域获取二进制值
        /// </summary>
        /// <param name="bitNum">数据域</param>
        /// <returns></returns>
        public byte[] GetArrayData(int bitNum)
        {
            if (!this.schema.fields.ContainsKey(bitNum))
                throw new Exception(String.Format("数据包定义不包含此域：[{0}]", bitNum));
            Iso8583Field field = this.schema.fields[bitNum];
            if (!this.values.ContainsKey(bitNum) || (this.values[bitNum] == null))
                throw new Exception(String.Format("数据域 [{0}] 不包含任何有效值。", bitNum));
            switch (field.DataType)
            {
                case Iso8583DataType.B:
                    return (byte[])this.values[bitNum];
                default:
                    throw new Exception(String.Format("数据域 [{0}] 格式不是二进制。", bitNum));
            }
        }
        #endregion

        #region 组包
        /// <summary>
        /// 获取指定域的长度
        /// </summary>
        /// <param name="bitNum">域号</param>
        /// <returns></returns>
        private int GetLength(int bitNum)
        {
            Iso8583Field field = this.schema.fields[bitNum];
            switch (field.Format)
            {
                //如果是变长域
                case Iso8583Format.LVAR:
                case Iso8583Format.LLVAR:
                case Iso8583Format.LLLVAR:
                    string value = "";
                    int len = 0;
                    if (this.values.ContainsKey(bitNum) && (this.values[bitNum] != null))
                    {
                        value = (string)values[bitNum];
                        switch (field.DataType)
                        {
                            //BCD码每数字占半字节
                            case Iso8583DataType.BCD:
                                len = (value.Length + 1) / 2;
                                break;
                            case Iso8583DataType.AS:
                                len = Encoding.ASCII.GetByteCount(value);
                                break;
                            case Iso8583DataType.GBK:
                                len = Encoding.GetEncoding("GBK").GetByteCount(value);
                                break;
                            default:
                                len = Encoding.Default.GetByteCount(value);
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("{0}域不在位图表中或值为空", bitNum));
                    }
                    //BCD码变长域，首部的长度，每个数字只占半字节
                    //其他变长域，每个数字占一个字节
                    if (field.DataType == Iso8583DataType.BCD)
                    {
                        return len + (field.Format - Iso8583Format.LVAR + 2) / 2;
                    }
                    else
                    {
                        int calLen = (field.Format - Iso8583Format.LVAR + 1);
                        calLen = calLen > 2 ? 2 : calLen;
                        return len + calLen;
                    }
                //如果是定长域
                default:
                    switch (field.DataType)
                    {
                        //BCD码，同上
                        case Iso8583DataType.BCD:
                            return (field.Length + 1) / 2;
                        default:
                            return field.Length;
                    }
            }
        }
        /// <summary>
        /// 向二进制数组中添加消息头、消息类型等固定变量
        /// </summary>
        /// <param name="str">要添加的数据，字符串格式</param>
        /// <param name="dst">二进制数组</param>
        /// <param name="pos">添加的起始位置</param>
        /// <param name="length">所占字节数</param>
        private void AppendData(string str, Array dst, ref int pos, int length)
        {
            AppendData(str, dst, ref pos, length, Iso8583DataType.BCD, Iso8583Property.ANS, false, false);
        }

        /// <summary>
        /// 向二进制数组中以指定格式添加数据
        /// </summary>
        /// <param name="str">要添加的数据，字符串格式</param>
        /// <param name="dst">二进制数组</param>
        /// <param name="pos">添加的起始位置</param>
        /// <param name="length">所占字节数</param>
        /// <param name="dataType">数据类型（BCD或ASCII）</param>
        /// <param name="property">属性（N或ANS）</param>
        /// <param name="isVariable">是否为变长</param>
        /// <param name="isLengthVar">是否为长度参数（每个变长域开头的长度指示）</param>
        private void AppendData(string str, Array dst, ref int pos, int length, Iso8583DataType dataType, Iso8583Property property, bool isVariable, bool isLengthVar)
        {
            byte[] field;
            field = Utility.GetBytes(str, dataType, property, length, isVariable, isLengthVar);
            System.Buffer.BlockCopy(field, 0, dst, pos, field.Length);
            pos += field.Length;
        }


        /// <summary>
        /// 组包一个 ISO 8583 数据包
        /// </summary>
        /// <returns></returns>
        public byte[] GetSendBuffer()
        {
            //计算数据包的最大长度
            //此长度不包括前两个字节
            int maxBuffLength = 0;
            this.headInfo = "6000000020";
            //消息类型
            if (string.IsNullOrEmpty(this.messageType))
                throw new Exception("消息类型不能为空!");
            maxBuffLength += 2;
            //请求头
            if (string.IsNullOrEmpty(this.headInfo))
            {
                throw new Exception("请求头不能为空!");
            }
            maxBuffLength += 5;
            maxBuffLength += 8;

            if (this.schema == null)
            {
                throw new Exception("未导入位域!");
            }
            //位图表
            IsoBitmap map = this.schema.bitmap;
            //统计其他域
            for (int bitNum = 2; bitNum <= IsoBitmap.FieldCount; bitNum++)
            {
                if (map.Get(bitNum))
                {
                    Iso8583Field field = this.schema.fields[bitNum];
                    if (!this.values.Keys.Contains(bitNum))
                    {
                        switch (field.Format)
                        {
                            case Iso8583Format.hhmmss:
                            case Iso8583Format.MMDD:
                            case Iso8583Format.MMDDhhmmss:
                            case Iso8583Format.YYMM:
                            case Iso8583Format.YYYY:
                            case Iso8583Format.YYYYMMDD:
                                SetDateTime(bitNum, DateTime.Now);
                                break;
                            case Iso8583Format.LVAR:
                            case Iso8583Format.LLVAR:
                            case Iso8583Format.LLLVAR:
                            case Iso8583Format.None:
                                switch (field.DataType)
                                {
                                    case Iso8583DataType.AS:
                                        SetString(bitNum, " ");
                                        break;
                                    case Iso8583DataType.B:
                                        SetArrayData(bitNum, new byte[] { 0x01 });
                                        break;
                                    case Iso8583DataType.BCD:
                                        SetNumber(bitNum, 0);
                                        break;
                                    case Iso8583DataType.GBK:
                                        SetString(bitNum, " ");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    maxBuffLength += this.GetLength(bitNum);
                    if (bitNum > 64)
                        throw new Exception("导入的位图表大于64位!");
                }
            }

            byte[] result = new byte[maxBuffLength + 2];
            //pos 添加数据指针的当前位置
            int pos = 0;
            //添加两个字节的长度
            byte[] maxLen = new byte[2];
            maxLen = Utility.BCDHexToByte(maxBuffLength.ToString("X2"), 2);
            Array.Copy(maxLen, result, 2);
            pos += 2;
            //添加消息头、消息类型、位图表
            this.AppendData(headInfo, result, ref pos, 10);
            this.AppendData(messageType, result, ref pos, 4);
            map.CopyTo(result, pos);
            pos += 8;
            //num 数据的长度
            int num;
            //添加域
            for (int bitNum = 2; bitNum <= IsoBitmap.FieldCount; bitNum++)
            {
                if (!map.Get(bitNum)) continue;
                Iso8583Field field = this.schema.fields[bitNum];
                num = 0;
                //生成Mac校验
                if (bitNum == 64)
                {
                    byte[] MAC_IV = new byte[8];
                    byte[] data = new byte[result.Length - 15];
                    Array.Copy(result, 7, data, 0, result.Length - 15);
                    byte[] mac = Iso8583Mac_CBC.MAC_CBC(MAC_KEY, MAC_IV, data);
                    this.SetArrayData(64, mac);
                }

                switch (field.Format)
                {
                    //如果是变长域
                    case Iso8583Format.LVAR:
                    case Iso8583Format.LLVAR:
                    case Iso8583Format.LLLVAR:
                        {
                            switch (field.DataType)
                            {
                                case Iso8583DataType.B:
                                    num = ((byte[])this.values[bitNum]).Length;
                                    break;
                                case Iso8583DataType.GBK:
                                    num = Encoding.GetEncoding("GBK").GetByteCount(this.values[bitNum].ToString());
                                    break;
                                default:
                                    num = this.values[bitNum].ToString().Length;
                                    break;
                            }
                            if (num > field.Length)
                            {
                                throw new Exception("域长度超过限制!");
                            }
                            //变长域抬头指示域长的字节数
                            int varLen = field.Format - Iso8583Format.LVAR + 1;
                            if (field.DataType == Iso8583DataType.BCD)
                            {
                                varLen = (varLen + 1) / 2;
                            }
                            if (varLen > 2)
                            {
                                varLen = 2;
                            }
                            //将域长添加进数组中
                            this.AppendData(num.ToString(), result, ref pos, varLen, Iso8583DataType.BCD, Iso8583Property.N, true, true);
                            break;
                        }
                    default:
                        break;
                }

                //根据数据类型的不同，采取不同的方式添加数据
                switch (field.DataType)
                {
                    case Iso8583DataType.B:
                        if (this.ExistValue(bitNum))
                        {
                            byte[] data = (byte[])this.values[bitNum];
                            data.CopyTo(result, pos);
                        }
                        pos += field.Length;
                        break;
                    default:
                        if (this.ExistValue(bitNum))
                        {
                            string value = "";
                            value = (string)this.values[bitNum];
                            switch (field.Format)
                            {
                                //如果为变长域
                                case Iso8583Format.LVAR:
                                case Iso8583Format.LLVAR:
                                case Iso8583Format.LLLVAR:
                                    this.AppendData(value, result, ref pos, num, field.DataType, field.Property, true, false);
                                    break;
                                //定长域
                                default:
                                    this.AppendData(value, result, ref pos, field.Length, field.DataType, field.Property, false, false);
                                    break;
                            }
                        }
                        break;
                }
            }
            buffer = result;
            return result;
        }
        #endregion

        #region 解包
        /// <summary>
        /// 解包一个 ISO 8583 数据包
        /// </summary>
        /// <param name="buf">数据包</param>
        public void ParseBuffer(byte[] buf)
        {
            if (buf == null)
                throw new ArgumentNullException("数据包不能为空");
            if (buf.Length < 20)
                throw new ArgumentException("数据包长度不能小于20");
            buffer = buf;
            //pos 解包数据指针的当前位置
            int pos = 0;
            //前两个字节指示后面的数据大小，不包括这两个字节
            byte[] data = new byte[2];
            Array.Copy(buf, pos, data, 0, 2);
            this.length = int.Parse(Utility.ByteToHex(data), System.Globalization.NumberStyles.HexNumber);
            pos += 2;
            //获取请求头
            data = new byte[5];
            Array.Copy(buf, pos, data, 0, 5);
            this.headInfo = Utility.ToFormatString(data, Iso8583DataType.BCD, Iso8583Property.N, 10, false);
            pos += 5;
            //获取消息类型
            data = new byte[2];
            Array.Copy(buf, pos, data, 0, 2);
            this.messageType = Utility.ToFormatString(data, Iso8583DataType.BCD, Iso8583Property.ANS, 4, false);
            pos += 2;
            //获取位图表
            data = new byte[8];
            Array.Copy(buf, pos, data, 0, 8);
            this.bitmap = new IsoBitmap(data);
            this.bitmapStr = Utility.ToFormatString(data, Iso8583DataType.B, Iso8583Property.N, 64, false);
            pos += 8;

            //获取消息处理码
            if (!bitmap.Get(2))
            {
                data = new byte[3];
                Array.Copy(buf, pos, data, 0, 3);
                processCode = Utility.ToFormatString(data, Iso8583DataType.BCD, Iso8583Property.N, 6, false);
            }
            else
            {
                //在dll中，兼容ascii码和bcd码两种格式
                //具体使用哪种格式由xml来指定
                //存在第2域的情况下，判断首字节是否为0，如果是则说明为ascii码格式
                byte[] tmp = new byte[1];
                Array.Copy(buf, pos, tmp, 0, 1);
                if (tmp[0] == 0x00)
                {
                    //变长域的长度指示
                    int varLen;
                    //变长域的占用字节数
                    int len;
                    data = new byte[2];
                    Array.Copy(buf, pos, data, 0, 2);
                    varLen = int.Parse(Utility.ByteToHex(data));
                    len = varLen;
                    pos += 2 + len;
                    data = new byte[3];
                    Array.Copy(buf, pos, data, 0, 3);
                    this.processCode = Utility.ToFormatString(data, Iso8583DataType.BCD, Iso8583Property.N, 6, false);
                    pos -= 2 + len;
                }
                else
                {
                    data = new byte[1];
                    Array.Copy(buf, pos, data, 0, 1);
                    int len = int.Parse(Utility.ByteToHex(data));
                    pos += 1;
                    pos += (len + 1) / 2;
                    data = new byte[3];
                    Array.Copy(buf, pos, data, 0, 3);
                    this.processCode = Utility.ToFormatString(data, Iso8583DataType.BCD, Iso8583Property.N, 6, false);
                    pos -= 1;
                    pos -= (len + 1) / 2;
                }
            }
            //判断消息类型和处理码，加载给定的xml
            SetSchema();
            //开始正式解码
            for (int bitNum = 2; bitNum <= IsoBitmap.FieldCount; bitNum++)
            {
                if (!bitmap.Get(bitNum)) continue;
                Iso8583Field field = this.schema.fields[bitNum];
                //域数据的字节数
                int len = 0;
                //域数据的实际长度
                int num = 0;
                //是否为变长
                bool isVariable = false;
                //先根据定长还是变长，获取域的长度
                switch (field.Format)
                {
                    case Iso8583Format.LVAR:
                    case Iso8583Format.LLVAR:
                    case Iso8583Format.LLLVAR:
                        //varLen 域的长度所占的字节数
                        int varLen = field.Format - Iso8583Format.LVAR + 1;
                        //BCD码域的长度所占字节为普通的一半
                        if (field.DataType == Iso8583DataType.BCD)
                        {
                            varLen = (varLen + 1) / 2;
                        }
                        if (varLen > 2)
                        {
                            varLen = 2;
                        }
                        data = new byte[varLen];
                        Array.Copy(buf, pos, data, 0, varLen);
                        num = int.Parse(Utility.ByteToHex(data));
                        pos += varLen;
                        isVariable = true;
                        break;
                    default:
                        num = field.Length;
                        break;
                }
                //BCD码每个数字只占半字节
                if (field.DataType == Iso8583DataType.BCD)
                {
                    len = (num + 1) / 2;
                }
                else
                {
                    len = num;
                }

                if (buf.Length < pos + len)
                {
                    Log.LoggingService.Warn(string.Format("buf.Length:{0}  pos:{1}  len:{2}", buf.Length, pos, len));
                    throw new ArgumentException(string.Format("数据包长度不符合定义:{0}", bitNum), "buf");
                }
                if (len == 0)
                    continue;
                //进行域的解码
                switch (field.DataType)
                {
                    case Iso8583DataType.B:
                        data = new byte[len];
                        Array.Copy(buf, pos, data, 0, num);
                        this.values[bitNum] = data;
                        break;
                    default:
                        data = new byte[len];
                        Array.Copy(buf, pos, data, 0, len);
                        this.values[bitNum] = Utility.ToFormatString(data, field.DataType, field.Property, num, isVariable);
                        break;
                }
                if (bitNum == 41)
                {
                    this.deviceId = this.values[bitNum].ToString();
                }
                pos += len;
            }
        }
        #endregion

        #region Util
        /// <summary>
        /// 获取一个适合在日志中输入的字符串
        /// </summary>
        /// <returns></returns>
        public string GetLogText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Package(MessageType:{0} ProcessCode:{1}):", this.messageType, this.processCode);
            sb.AppendLine();
            sb.AppendLine("{");
            foreach (KeyValuePair<int, object> kvp in this.values)
            {
                Iso8583Field field = this.schema.fields[kvp.Key];
                string value = "";
                if (kvp.Value != null)
                {
                    switch (field.DataType)
                    {
                        case Iso8583DataType.B:
                            value = BitConverter.ToString((byte[])kvp.Value);
                            break;
                        default:
                            value = (string)kvp.Value;
                            break;
                    }
                }
                sb.AppendFormat("    [{0}]:{1}", field.FieldName, value);
                sb.AppendLine();
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion
    }
}
