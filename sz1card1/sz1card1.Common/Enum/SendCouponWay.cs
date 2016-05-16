using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    public enum SendCouponWay
    {
        //途径，发放途径包含：1.消费发送2.直接发放3.充值发送4.礼品兑换发送

        /// <summary>
        /// 1 消费发券
        /// </summary>
        SendByConsume = 1,

        /// <summary>
        /// 2 直接发券
        /// </summary>
        SendDirectly = 2,

        /// <summary>
        /// 充值发券
        /// </summary>
        SendByCharge =3,

        /// <summary>
        /// 礼品兑换券
        /// </summary>
        SendByGift = 4

    }
}
