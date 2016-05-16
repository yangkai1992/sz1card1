using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sz1card1.Common.Enum;

namespace sz1card1.Common.Timeline
{
    /// <summary>
    /// 时间轴
    /// </summary>
    public interface ITimeline : ICloneable
    {
        /// <summary>
        /// 标识
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 执行时间
        /// </summary>
        DateTime ExecuteTime { get; set; }

        /// <summary>
        /// 过期后是否执行或发生异常是否延迟执行
        /// </summary>
        bool RedoOnExpired { get; }

        /// <summary>
        /// 执行间隔类型
        /// </summary>
        IntervalTypes IntervalType { get; set; }

        /// <summary>
        /// 执行间隔倍数
        /// </summary>
        int IntervalCount { get; set; }

        /// <summary>
        /// 执行次数(默认为0)
        /// </summary>
        int ExecuteTimes { get; set; }
    }
}
