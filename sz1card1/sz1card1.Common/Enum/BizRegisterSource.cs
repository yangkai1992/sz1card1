using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// 商家注册来源
    /// </summary>
    public enum BizRegisterSource
    {
        /// <summary>
        /// 一卡易
        /// </summary>
        一卡易 = 0x000001,

        /// <summary>
        /// 云卡
        /// </summary>
        云卡 = 0x000010,

        /// <summary>
        /// Vx520端和GP730端
        /// </summary>
        Vx520端 = 0x000100,

        /// <summary>
        /// 美萍
        /// </summary>
        美萍 = 0x001000,

        /// <summary>
        /// 微信会员卡
        /// </summary>
        微信会员卡 = 0x010000,

        /// <summary>
        /// 微POS端和EPOS端
        /// </summary>
        微POS端 = 0x100000
    }
}
