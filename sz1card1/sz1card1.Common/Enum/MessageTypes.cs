using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Enum
{
    public enum MessageTypes
    {
        None = 0,

        /// <summary>
        /// 待办事项
        /// </summary>
        ServiceAlert = 1,

        /// <summary>
        /// 服务计划
        /// </summary>
        MemberServiceAlert = 2,

        /// <summary>
        ///  扣费提醒
        /// </summary>
        FeeDueAlert = 3,

        /// <summary>
        /// 公告提醒
        /// </summary>
        BulletinAlert = 4,

        /// <summary>
        /// 被迫下线提醒 
        /// </summary>
        ForcedLogoutAlert = 5,

        /// <summary>
        /// 登录超时提醒
        /// </summary>
        LoginTimeOutAlert = 6
    }

    /// <summary>
    /// 通道类型
    /// </summary>
    public enum ChannelTypes
    {
        /// <summary>
        /// 短信通道
        /// </summary>
        Sms = 1,

        /// <summary>
        /// 彩信通道
        /// </summary>
        Mms = 2,
    }

    /// <summary>
    /// 通道运营商类型
    /// </summary>
    public enum ChannelOperatorTypes
    {
        /// <summary>
        /// 移动
        /// </summary>
        [Description("移动")]
        Mobile = 0x0001,

        /// <summary>
        /// 联通
        /// </summary>
        [Description("联通")]
        Unicom = 0x0010,

        /// <summary>
        /// 电信
        /// </summary>
        [Description("电信")]
        Telecom = 0x0100,
    }

    /// <summary>
    /// 通道属性
    /// </summary>
    public enum ChannelProperty
    {
        /// <summary>
        /// 支持单发
        /// </summary>
        单发 = 0x0001,

        /// <summary>
        /// 支持群发
        /// </summary>
        群发 = 0x0010,

        /// <summary>
        /// 支持回复
        /// </summary>
        回复 = 0x0100,

        /// <summary>
        /// 支持短信签名
        /// </summary>
        签名 = 0x1000,

        /// <summary>
        /// 支持验证码
        /// </summary>
        验证码 = 0x10000
    }

    /// <summary>
    /// 通道归属类型
    /// </summary>
    public enum ChannelPertainTypes
    {
        一卡易签名公共通道 = 0,
        自定义签名公共通道 = 1,
        代理商通道 = 2,
        商家通道 = 3,
    }

    /// <summary>
    /// 发送类型
    /// </summary>
    public enum MessageSendTypes
    {
        /// <summary>
        /// 单发
        /// </summary>
        [Description("单发")]
        Single = 1,

        /// <summary>
        /// 群发
        /// </summary>
        [Description("群发")]
        Multip = 2,

        /// <summary>
        /// 发验证码
        /// </summary>
        [Description("验证码")]
        Verify = 3
    }

    /// <summary>
    /// 推送消息类型
    /// </summary>
    public enum NotificationTypes
    {
        /// <summary>
        /// 普通消息
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 链接消息
        /// </summary>
        Linker,

        /// <summary>
        /// 富媒体
        /// </summary>
        RichMedia
    }

    #region ChannelRule相关的枚举

    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// 试用
        /// </summary>
        [Description("试用")]
        Trying = 0x00000001,

        /// <summary>
        /// 签约
        /// </summary>
        [Description("使用")]
        Signed = 0x00000010
    }

    /// <summary>
    /// 签名类型
    /// </summary>
    public enum SignedType
    {
        /// <summary>
        /// 一卡易签名
        /// </summary>
        YiKaYi = 0x00000001,

        /// <summary>
        /// 自定义签名
        /// </summary>
        Custom = 0x00000010
    }

    #endregion
}
