using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.DistributedLock
{
    /// <summary>
    /// 分布式锁接口
    /// </summary>
    public interface IDistributedLock : IDisposable
    {
        /// <summary>
        /// 尝试加锁
        /// </summary>
        /// <returns></returns>
        bool TryLock();

        /// <summary>
        /// 尝试加锁
        /// </summary>
        /// <param name="time">超时时间</param>
        /// <returns></returns>
        bool TryLock(TimeSpan time);

        /// <summary>
        /// 解锁
        /// </summary>
        void UnLock();

        /// <summary>
        /// 获取已阻塞的长度
        /// </summary>
        /// <returns></returns>
        int BlockLength{get;}
    }
}
