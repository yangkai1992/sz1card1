using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    ///  单据序号的类型
    /// </summary>
    public enum BillTypesForBillNumber
    {
        /// <summary>
        ///  消费
        /// </summary>
        Consume = 1,

        /// <summary>
        ///  充值
        /// </summary>
        ValueAdd = 2,

        /// <summary>
        ///  增加计次
        /// </summary>
        CountAdd = 3,

        /// <summary>
        ///  积分兑换
        /// </summary>
        GiftExchange = 4,

        /// <summary>
        ///  退货
        /// </summary>
        GoodsReturn = 5,

        /// <summary>
        ///  积分扣除
        /// </summary>
        PointDeduct = 6,

        /// <summary>
        ///  商品订购
        /// </summary>
        GoodsOrder = 7,

        /// <summary>
        ///  采购
        /// </summary>
        Procurement = 8,

        /// <summary>
        /// 调拨
        /// </summary>
        Transfer = 9,

        /// <summary>
        ///  盘点
        /// </summary>
        InventoryCheck = 10,

        /// <summary>
        ///  电子券发送
        /// </summary>
        CouponSend = 11,
        /// <summary>
        ///  售卡
        /// </summary>
        CardSell = 12,

        /// <summary>
        /// 赠送储值
        /// </summary>
        GiftValue = 13,

        /// <summary>
        /// 退卡
        /// </summary>
        RetrunCard = 15,

        /// <summary>
        /// 赠送负储值
        /// </summary>
        GiftNegativeValue = 16,
        /// <summary>
        /// 增加储值
        /// 通过接口直接增加储值，逻辑同赠送储值
        /// </summary>
        ValueAddOther = 17,
        /// <summary>
        /// 减少储值
        /// 通过接口直接减少储值，逻辑同赠送负数储值
        /// </summary>
        ValueDeductOther = 18,

        /// <summary>
        /// 增加积分
        /// 通过接口直接增加积分，逻辑同赠送积分
        /// </summary>
        PointAddOther = 19,

        /// <summary>
        /// 减少积分
        /// 通过接口直接减少积分，逻辑同赠送负数积分
        /// </summary>
        PointDeductOther = 20,

        /// <summary>
        /// 赠送积分
        /// </summary>
        GiftPoint = 21,

        /// <summary>
        /// 赠送负积分
        /// </summary>
        GiftNegativePoint = 22,
        /// <summary>
        /// 提现
        /// </summary>
        Withdraw = 23,
        /// <summary>
        ///储值清零
        /// </summary>
        ClearValue = 24,
        /// <summary>
        /// 积分清零
        /// </summary>
        ClearPoint = 25,
        /// <summary>
        /// 推荐会员
        /// </summary>
        Recommend = 26,

        /// <summary>
        /// 积分、储值转账
        /// </summary>
        PointOrValueTrans = 27,
        /// <summary>
        /// 管理费
        /// </summary>
        ManageConsume=28,
        /// <summary>
        /// 
        /// </summary>
        GiftOrder=29
    }
}
