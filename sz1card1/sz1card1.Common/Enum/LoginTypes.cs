using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// 登录方式
    /// </summary>
    public enum LoginTypes
    {
        /// <summary>
        /// 电脑端
        /// </summary>
        一卡易 = 1,

        /// <summary>
        /// 云卡
        /// </summary>
        云卡 = 2,

        /// <summary>
        /// GP730端
        /// </summary>
        GP730 = 4,

        /// <summary>
        /// 微POS和EPOS
        /// </summary>
        微POS = 32,

        /// <summary>
        /// 移动收单
        /// </summary>
        移动收单 = 64,

        /// <summary>
        /// 钱客多
        /// </summary>
        钱客多 = 128
    }
}
