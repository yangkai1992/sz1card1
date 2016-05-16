using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace sz1card1.Common.Queue
{
    public class QueueSender<T>
    {
        private string queueName = string.Empty;
        private MessageQueue queue;

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName
        {
            get
            {
                return queueName;
            }
            set
            {
                queueName = value;
            }
        }

        public QueueSender(string queueName, IMessageFormatter messageFormatter)
        {
            this.queueName = queueName;
            queue = new MessageQueue(QueueName);
            queue.Formatter = messageFormatter;
        }

        public QueueSender(string queueName)
            : this(queueName, new XmlMessageFormatter(new Type[] { typeof(T) }))
        {
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        public void Send(T message)
        {
            Message msg = new Message(message, queue.Formatter);
            queue.Send(msg);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="label">标签</param>
        public void Send(T message, string label)
        {
            Message msg = new Message(message, queue.Formatter);
            msg.Label = label;
            queue.Send(msg);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="label">标签</param>
        /// <param name="priority">优先级</param>
        public void Send(T message, string label, MessagePriority priority)
        {
            Message msg = new Message(message, queue.Formatter);
            msg.Label = label;
            msg.Priority = priority;
            queue.Send(msg);
        }
    }
}
