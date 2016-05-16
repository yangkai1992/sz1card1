using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// 类型：（1.计次,2.扣次,3.积分兑换,4.快速积分,5.消费收银,6.充值,7.储值扣费.8.退货,9.交班.10.挂单,11.场地开单,12.场地消费明细,13.会员登记,14.电子优惠券,15.采购,16.调拨,17.盘点,18.单据撤销,19 当日统计,20.积分扣除）
    /// </summary>
    public enum PrintTemplateType
    {
        CountAdd = 1,
        CountConsume = 2,
        GiftExchange = 3,
        ManuelPoint = 4,
        CheckOut = 5,
        ValueAdd = 6,
        ValueConsume = 7,
        ReturnConsume = 8,
        ShiftLogin = 9,
        HangNote = 10,
        PlaceBill = 11,
        PlaceConsumeItem = 12,
        MemberRegister = 13,
        ECoupon = 14,
        Procurement = 15,
        Transfer = 16,
        InventoryCheck = 17,
        UnDo = 18,
        DateStatic = 19,
        PointDeduct = 20
    }
}
