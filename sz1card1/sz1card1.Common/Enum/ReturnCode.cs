using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    /// <summary>
    /// Vx520返回值信息
    /// </summary>
    public enum ReturnCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        OK = 0,
        /// <summary>
        /// 结算不平
        /// </summary>
        BalanceAccount,
        /// <summary>
        /// 会话超时
        /// </summary>
        SessionTimeout,
        /// <summary>
        /// 被迫下线
        /// </summary>
        ForcedLogout,
        /// <summary>
        /// 服务器配置错误
        /// </summary>
        ServerError,
        /// <summary>
        /// 后台错误
        /// </summary>
        BackError,
        /// <summary>
        /// 等待第三方支付
        /// </summary>
        WaitPay=6,
        /// <summary>
        /// 其他错误
        /// </summary>
        Other = 99
    }
}
