using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    public enum CheckOutType
    {
        /// <summary>
        /// 现金
        /// </summary>
        Cash = 1,

        /// <summary>
        /// 积分
        /// </summary>
        Point = 2,

        /// <summary>
        /// 储值
        /// </summary>
        StoredValue = 3,

        /// <summary>
        /// 银行卡
        /// </summary>
        Card = 4,

        /// <summary>
        /// 优惠券
        /// </summary>
        Coupon = 5,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 6,

        /// 支付宝
        /// </summary>
        Alipay = 7,

        /// <summary>
        /// 拉卡拉
        /// </summary>
        Lakala = 8,

        /// <summary>
        /// 快钱
        /// </summary>
        KQ = 9,

        /// <summary>
        /// 微信
        /// </summary>
        Weixinpay = 10,

        /// <summary>
        /// 百度钱包
        /// </summary>
        Baidupay =11,

        /// <summary>
        /// 微信个人收款
        /// </summary>
        PersonalWeixinpay = 12,

        /// <summary>
        /// 支付宝个人收款
        /// </summary>
        PersonalAlipay = 13,

        /// <summary>
        /// 拉卡拉-京东支付
        /// </summary>
        Lakala_Jingdongpay = 14,

        /// <summary>
        /// 拉卡拉-支付宝支付
        /// </summary>
        Lakala_Alipay = 15,

        /// <summary>
        /// 拉卡拉-微信支付
        /// </summary>
        Lakala_Weixinpay = 16,

        /// <summary>
        /// 拉卡拉-百度支付
        /// </summary>
        Lakala_Baidupay = 17,
    }
}
