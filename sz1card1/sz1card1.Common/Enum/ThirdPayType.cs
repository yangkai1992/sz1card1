using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// 第三方支付 类型
    /// </summary>
    public enum ThirdPayType
    {
        /// <summary>
        /// 支付宝支付
        /// </summary>
        Alipay = 0,

        /// <summary>
        /// 拉卡拉支付
        /// </summary>
        LaKaLa = 1,

        /// <summary>
        /// 微信支付
        /// </summary>
        WeiXinpay = 2,

        /// <summary>
        /// 百度钱包支付
        /// </summary>
        BaiDupay = 3,

        /// <summary>
        /// 快钱支付
        /// </summary>
        KQ = 4,

        /// <summary>
        /// 微信个人收款
        /// </summary>
        PersonalWeixinpay = 5,

        /// <summary>
        /// 支付宝个人收款
        /// </summary>
        PersonalAlipay = 6,

        /// <summary>
        /// 拉卡拉-京东支付
        /// </summary>
        Lakala_Jingdongpay = 7,

        /// <summary>
        /// 拉卡拉-支付宝支付
        /// </summary>
        Lakala_Alipay = 8,

        /// <summary>
        /// 拉卡拉-微信支付
        /// </summary>
        Lakala_Weixinpay = 9,

        /// <summary>
        /// 拉卡拉-百度支付
        /// </summary>
        Lakala_Baidupay = 10,
    }
}
