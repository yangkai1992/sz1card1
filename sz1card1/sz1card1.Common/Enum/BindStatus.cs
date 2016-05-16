using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    public enum BindStatus
    {

        /// <summary>
        /// 默认一卡易、云卡会员0x0000
        /// </summary> 
        Common = 0, //0x0000
        /// <summary>
        /// 微信会员0x0001
        /// </summary>
        WeChat = 1,//0x0001
        /// <summary>
        /// 云会员0x0010
        /// </summary>
        VipCloud = 16//0x0010

    }
}
