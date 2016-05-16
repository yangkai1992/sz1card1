using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// 积分来源类型
    /// </summary>
    public enum PointWayTypes
    {
        /// <summary>
        /// 初始积分
        /// </summary>
        InitPoint = 1,

        /// <summary>
        /// 消费收银获得积分
        /// </summary>
        CheckOut = 2,

        /// <summary>
        /// 快速消费
        /// </summary>
        ManuelPoint = 3,

        /// <summary>
        /// 会员充值
        /// </summary>
        ValueAdd = 4,

        /// <summary>
        /// 增加计次
        /// </summary>
        CountAdd = 5,

        /// <summary>
        /// 消费推荐积分
        /// </summary>
        RecommendMember = 6,

        /// <summary>
        /// 赠送积分  
        /// </summary>
        GiftPoint = 7,

        /// <summary>
        /// 储值扣费积分转入
        /// </summary>
        ValueConsume = 8,

        /// <summary>
        /// 积分转入
        /// </summary>
        PointIn = 9,


        /// <summary>
        /// 退货积分增加
        /// </summary>
        ReturnPointAdd = 10,


        /// <summary>
        /// 批量积分
        /// </summary>
        BatchPoint = 11,

        /// <summary>
        /// 推荐会员注册获得积分
        /// </summary>
        RecommendRegisterMember = 12,

        /// <summary>
        /// 储值返积分
        /// </summary>
        StoreValueInterestPoint = 13,

        /// <summary>
        /// 其他来源(增加)
        /// 目前通过接口  add by lff 2014-10-16
        /// </summary>
        OtherAdd = 14,
        /// <summary>
        /// 升级增加积分
        /// </summary>
        UpgradeMemberGroupAdd = 15,

        /// <summary>
        ///  每日签到
        /// </summary>
        DailyAttendance = 16,

        /// <summary>
        /// 会员抽奖(大转盘)
        /// </summary>
        BigTurntable = 17,

        /// <summary>
        /// 会员抽奖(刮刮卡)
        /// </summary>
        ScratchCard = 18,

        /// <summary>
        /// 抢红包
        /// </summary>
        RedEnvelope = 19,

        /// <summary>
        /// 云会员接口
        /// </summary>
        CloudMemberInterface = 20,

        /// <summary>
        /// 计时消费获得积分
        /// </summary>
        TimeConsume = 21,
        /// <summary>
        /// 计时消费获得积分
        /// </summary>
        OnLineConsume = 22,
        /// <summary>
        /// 积分兑换
        /// </summary>
        GiftExchange = -1,

        /// <summary>
        /// 积分抵现
        /// </summary>
        PaidPoint = -2,

        /// <summary>
        /// 积分扣除
        /// </summary>
        PointDeduct = -3,

        /// <summary>
        /// 升级扣除积分
        /// </summary>
        UpgradeMemberGroup = -4,

        /// <summary>
        /// 积分清零
        /// </summary>
        PointToZero = -5,


        /// <summary>
        /// 管理费
        /// </summary>
        ManageConsume = -6,

        /// <summary>
        /// 积分赠送负数
        /// </summary>
        ReductPoint = -7,


        /// <summary>
        /// 积分转出
        /// </summary>
        PointOut = -9,


        /// <summary>
        /// 退货积分扣除
        /// </summary>
        ReturnPointReduct = -10,
        /// <summary>
        /// 退卡积分扣除
        /// </summary>
        ReturnCardDeductPoint = -11,

        /// <summary>
        /// 其他来源(减少)
        /// 目前通过接口  add by lff 2014-10-16
        /// </summary>
        OtherDeduct = -14,

        /// <summary>
        /// 提现(云会员-推荐积分提现中使用)
        /// </summary>
        Withdraw = -15
    }

    /// <summary>
    /// 储值来源类型
    /// </summary>
    public enum ValueWayTypes
    {
        /// <summary>
        /// 初始储值
        /// </summary>
        InitValue = 1,

        /// <summary>
        /// 会员充值
        /// </summary>
        ValueAdd = 2,

        /// <summary>
        /// 赠送储值
        /// </summary>
        GiftValue = 3,

        /// <summary>
        /// 积分兑换储值
        /// </summary>
        GiftExchange = 4,

        /// <summary>
        /// 返赠储值
        /// </summary>
        ReturnValue = 5,


        /// <summary>
        /// 储值转入
        /// </summary>
        ValueIn = 6,


        /// <summary>
        /// 退货储值增加
        /// </summary>
        ReturnValueAdd = 7,

        /// <summary>
        ///  会员网站充值
        /// </summary>
        ValueAddByMemberWebSite = 8,


        /// <summary>
        /// 批量储值
        /// </summary>
        BatchValue = 11,

        /// <summary>
        /// 储值返储值
        /// </summary>
        StoreValueInterestValue = 13,

        /// <summary>
        /// 其他来源(增加)
        /// 目前通过接口  add by lff 2014-10-16
        /// </summary>
        OtherAdd = 14,
        /// <summary>
        /// 其他来源(增加)
        /// 目前通过接口  add by lff 2014-10-16
        /// </summary>
        ChangeMoneyStore = 15,


        /// <summary>
        /// 会员抽奖(大转盘)
        /// </summary>
        BigTurntable = 17,

        /// <summary>
        /// 会员抽奖(刮刮卡)
        /// </summary>
        ScratchCard = 18,

        /// <summary>
        /// 抢红包
        /// </summary>
        RedEnvelope = 19,

        /// <summary>
        /// 云会员接口
        /// </summary>
        CloudMemberInterface = 20,

        /// <summary>
        /// 消费收银
        /// </summary>
        CheckOut = -1,

        /// <summary>
        /// 储值扣费
        /// </summary>
        ValueConsume = -2,

        /// <summary>
        /// 增加计次
        /// </summary>
        CountAdd = -3,

        /// <summary>
        /// 储值清零
        /// </summary>
        ValueToZero = -4,

        /// <summary>
        /// 管理费
        /// </summary>
        ManageConsume = -5,

        /// <summary>
        /// 储值转出
        /// </summary>
        ValueOut = -6,

        /// <summary>
        /// 储值赠送负数
        /// </summary>
        ReductValue = -7,
        /// <summary>
        /// 退卡储值清零
        /// </summary>
        ReturnCardDeductValue = -8,

        /// <summary>
        /// 其他来源(扣除)
        /// 目前通过接口  add by lff 2014-10-16
        /// </summary>
        OtherDeduct = -14,

        /// <summary>
        /// 提现
        /// </summary>
        Withdraw = -9,

        /// <summary>
        /// 退货储值扣除
        /// </summary>
        ReturnValueDeduct = -10,

        /// <summary>
        /// 计时消费
        /// </summary>
        TimeConsume = -15

    }

    /// <summary>
    /// 计次来源类型
    /// </summary>
    public enum CountWayTypes
    {
        /// <summary>
        /// 初始计次
        /// </summary>
        InitCout = 1,

        /// <summary>
        /// 增加计次
        /// </summary>
        CountAdd = 2,

        /// <summary>
        /// 赠送计次
        /// </summary>
        GiftCount = 3,

        /// <summary>
        /// 兑换计次
        /// </summary>
        GiftExchange = 4,

        /// <summary>
        /// 批量计次
        /// </summary>
        BatchCount = 11,

        /// <summary>
        /// 计次消费
        /// </summary>
        CountConsume = -1,
        /// <summary>
        /// 退卡清零
        /// </summary>
        ReturnCard = -2,
        /// <summary>
        /// 规则充次
        /// </summary>
        RuleCount = 5,

        /// <summary>
        /// 计次消费退货
        /// </summary>
        CountConsumeReturn = 6,

        /// <summary>
        /// 增加计次退货
        /// </summary>
        AddCountReturn = -6,

        /// <summary>
        /// 积分兑换退货
        /// </summary>
        GiftExchangeReturn = -7

    }

    /// <summary>
    /// 电子卷发送途径
    /// </summary>
    public enum CouponSendWayTypes
    {
        /// <summary>
        /// 快速消费
        /// </summary>
        ManuelPoint = 1,

        /// <summary>
        /// 消费收银
        /// </summary>
        CheckOut = 2,

        /// <summary>
        /// 储值扣费
        /// </summary>
        ValueConsume = 3,

        /// <summary>
        /// 计次消费
        /// </summary>
        CountConsume = 4,


        /// <summary>
        /// 消费退货
        /// </summary>
        ReturnConsume = -2

    }

    /// <summary>
    /// 消费来源类型
    /// </summary>
    public enum ConsumeWayTypes
    {
        /// <summary>
        /// 快速消费
        /// </summary>
        ManuelPoint = 1,

        /// <summary>
        /// 消费收银
        /// </summary>
        CheckOut = 2,

        /// <summary>
        /// 储值扣费
        /// </summary>
        ValueConsume = 3,

        /// <summary>
        /// 计次消费
        /// </summary>
        CountConsume = 4,

        /// <summary>
        /// 计时消费
        /// </summary>
        TimeConsume = 5,

        /// <summary>
        /// 线上消费
        /// </summary>
        OnLineConsume = 6,
        /// <summary>
        /// 消费收银退货
        /// </summary>
        ReturnConsume = -2,

        /// <summary>
        /// 计次消费退货
        /// </summary>
        CountReturnConsume = -4,

        /// <summary>
        /// 快速消费退货
        /// </summary>
        ManuelPointReturn = -1,

        /// <summary>
        /// 储值扣费退货
        /// </summary>
        ValueReturnConsume = -3,

        /// <summary>
        /// 计时消费退货
        /// </summary>
        ReturnTimeConsume = -5

    }

    /// <summary>
    /// 结帐来源类型
    /// </summary>
    public enum CheckOutWayTypes
    {

        /// <summary>
        /// 售卡金额
        /// </summary>
        cardsell = 1,
        /// <summary>
        /// 快速消费
        /// </summary>
        ManuelPoint = 2,

        /// <summary>
        /// 消费收银
        /// </summary>
        CheckOut = 3,

        /// <summary>
        /// 会员充值
        /// </summary>
        ValueAdd = 4,

        /// <summary>
        /// 增加计次
        /// </summary>
        CountAdd = 5,

        /// <summary>
        /// 赠送储值
        /// </summary>
        GiftValue = 6,

        /// <summary>
        /// 消费收银退货 
        /// </summary>
        ReturnConsume = -3,
        /// <summary>
        /// 会员退卡 
        /// </summary>
        ReturnCard = -4,
        /// <summary>
        /// 提现
        /// </summary>
        Withdraw = -5,

        /// <summary>
        /// 快速消费退货
        /// </summary>
        ManuelPointReturn = -1,

        /// <summary>
        /// 会员充值退款
        /// </summary>
        ValueAddReturn = -6,

        /// <summary>
        /// 会员提现退款
        /// </summary>
        WithdrawReturn = 7,



        /// <summary>
        /// 增加计次退货
        /// </summary>
        CountAddReturn = -7,

        /// <summary>
        /// 计时消费
        /// </summary>
        TimeConsume = 8,


        /// <summary>
        /// 计时消费退货 
        /// </summary>
        ReturnTimeConsume = -8

    }
}
