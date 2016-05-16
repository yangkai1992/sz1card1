using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace sz1card1.Common
{
    /// <summary>
    /// 常用的正则表达式
    /// </summary>
    public static class RegularExp
    {
        /// <summary>
        /// 匹配手机号
        /// </summary>
        public static readonly string Mobile = @"^1\d{10}$";

        /// <summary>
        ///  匹配手机号包括外省的号码
        /// </summary>
        public static readonly string FullMobile = @"^(0)?1\d{10}$";

        /// <summary>
        ///  移动号码
        /// </summary>
        public static readonly string mobileTel = @"(^(134|135|136|137|138|139|147|150|151|152|154|157|158|159|182|183|184|187|188)\d{8}$)|(^1705\d{7}$)";

        /// <summary>
        /// 联通号码
        /// </summary>
        public static readonly string unicomTel = @"(^(130|131|132|155|156|171|176|185|186)\d{8}$)|(^(1704|1707|1708|1709)\d{7}$)";

        /// <summary>
        /// 电信号码
        /// </summary>
        public static readonly string telecomTel = @"(^(133|153|177|180|181|189)\d{8}$)|(^1700\d{7}$)";


        /// <summary>
        /// 匹配多个手机号形如130xxxxxxx0,130xxxxxxx1
        /// </summary>
        public static readonly string MultipMobile = @"^(1\d{10},)*(1\d{10})$";

        /// <summary>
        /// 匹配身份证
        /// </summary>
        public static readonly string IdCard1 = @"/(^\d{15}$)|(^\d{17}(?:\d|x|X)$)|(^\d{16}(?:\d|x|X)(?:\d|x|X)$)/";

        /// <summary>
        /// 匹配邮编
        /// </summary>
        public static readonly string PostCode = @"^\d{6}$";

        /// <summary>
        /// 匹配电话号码
        /// </summary>
        public static readonly string Phone = @"^(\d{3,4})?\d{7,8}$";

        /// <summary>
        ///  匹配电话号码
        /// </summary>
        public static readonly string Tel = @"^((\d{7,8})|(\d{4}|\d{3})(-)?(\d{7,8})|(\d{4}|\d{3})(-)?(\d{7,8})(-)?(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})(-)?(\d{4}|\d{3}|\d{2}|\d{1}))$";

        /// <summary>
        /// 匹配电子邮件
        /// </summary>
        public static readonly string EMail = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// 匹配整数
        /// </summary>
        public static readonly string Number = @"^-?\d+$";

        /// <summary>
        /// 匹配正整数
        /// </summary>
        public static readonly string NumberL = @"^([0-9]*[1-9][0-9]*)|0$";

        /// <summary>
        /// 匹配浮点数
        /// </summary>
        public static readonly string Float = @"^(-?\d+)(\.\d+)?$";

        /// <summary>
        /// 匹配非负浮点数
        /// </summary>
        public static readonly string Decimal = @"^\d+(\.\d+)?$";

        /// <summary>
        /// IP地址
        /// </summary>
        public static readonly string IPAddress = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

        public static readonly string GUID = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
    }
}
