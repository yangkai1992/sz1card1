using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Queue
{
    public class QueueEventArg<T> : EventArgs
    {
        private T message;
        private int counter;
        private int currentThreadId;

        /// <summary>
        /// 消息内容
        /// </summary>
        public T Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        /// <summary>
        /// 计数器
        /// </summary>
        public int Counter
        {
            get
            {
                return counter;
            }
            set
            {
                counter = value;
            }
        }

        /// <summary>
        /// 当前线程标识
        /// </summary>
        public int CurrentThreadId
        {
            get
            {
                return currentThreadId;
            }
            internal set
            {
                currentThreadId = value;
            }
        }
    }
}
