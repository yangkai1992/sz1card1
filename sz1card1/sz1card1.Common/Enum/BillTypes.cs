using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    public enum BillTypes
    {
        /// <summary>
        /// 快速积分
        /// </summary>
        PointNote = 1,
        /// <summary>
        /// 储值(增加)
        /// </summary>
        MemberValueNote_In = 2,
        /// <summary>
        /// 储值(减少)
        /// </summary>
        MemberValueNote_Out = 3,
        /// <summary>
        /// 计次(增加) 
        /// </summary>
        MemberCountNote_In = 4,
        /// <summary>
        /// 计次(减少)
        /// </summary>
        MemberCountNote_Out = 5,
        /// <summary>
        /// 收银 
        /// </summary>
        GoodsNote = 6,
        /// <summary>
        /// 积分兑换
        /// </summary>
        GiftNote = 7,
        ///// <summary>
        ///// 采购(进货)
        ///// </summary>
        //Procurement_In = 8,
        ///// <summary>
        ///// 采购(退货)
        ///// </summary>
        //Procurement_Out = 9,
        ///// <summary>
        ///// 调拨
        ///// </summary>
        //Transfer = 10,
        ///// <summary>
        ///// 盘点
        ///// </summary>
        //InventoryCheck = 11,
        /// <summary>
        /// 退货 
        /// </summary>
        GoodsReturnNote = 12,
        /// <summary>
        /// 会员登记
        /// </summary>
        MemberRegister = 13,
        /// <summary>
        /// 扣除积分
        /// </summary>
        DeductPointNote=14,
        /// <summary>
        /// 提现
        /// </summary>
        Withdraw=15,
        /// <summary>
        /// 计时消费
        /// </summary>
        TimeConsume = 16
    }
}
