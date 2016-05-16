using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common.Timeline
{
    public class TimelineEventArg<T> : EventArgs where T : ITimeline
    {
        private T message;

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
    }
}
