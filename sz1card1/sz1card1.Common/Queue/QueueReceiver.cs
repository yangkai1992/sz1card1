using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Threading;

namespace sz1card1.Common.Queue
{
    public class QueueReceiver<T>
    {
        private string queueName = string.Empty;
        private MessageQueue queue;
        private int maxThreadsCount = 5;
        private int timeOutSecondes = 10;
        private bool isRunning = false;
        private WaitHandle[] waitArray;
        private Dictionary<string, object> attributes;
        public event EventHandler<QueueEventArg<T>> Execute;
        public event EventHandler<QueueExceptionArg<T>> ExecuteError;

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

        /// <summary>
        /// 最大线程数
        /// </summary>
        public int MaxThreadsCount
        {
            get
            {
                return maxThreadsCount;
            }
            set
            {
                maxThreadsCount = value;
            }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                isRunning = value;
            }
        }

        /// <summary>
        /// 线程等待秒数
        /// </summary>
        public int TimeOutSeconds
        {
            get
            {
                return timeOutSecondes;
            }
            set
            {
                timeOutSecondes = value;
            }
        }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public Dictionary<string, object> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public QueueReceiver(string queueName)
            : this(queueName, new XmlMessageFormatter(new Type[] { typeof(T) }))
        {
        }

        public QueueReceiver(string queueName, IMessageFormatter messageFormatter)
        {
            this.queueName = queueName;
            attributes = new Dictionary<string, object>();
            queue = new MessageQueue(QueueName);
            queue.Formatter = messageFormatter;
            queue.ReceiveCompleted += new ReceiveCompletedEventHandler(queue_ReceiveCompleted);
        }

        public void Start()
        {
            waitArray = new WaitHandle[maxThreadsCount];
            for (int i = 0; i < maxThreadsCount; i++)
            {
                waitArray[i] = queue.BeginReceive().AsyncWaitHandle;
            }
            isRunning = true;
        }

        public void Stop()
        {
            try
            {
                queue.Close();
            }
            catch
            {
            }
            finally
            {
                isRunning = false;
            }
        }

        void queue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue mq = sender as MessageQueue;
                Message msg = mq.EndReceive(e.AsyncResult);
                T message = (T)msg.Body;
                QueueEventArg<T> args = new QueueEventArg<T>();
                args.Message = message;
                if (!string.IsNullOrEmpty(msg.Label))
                    args.Counter = int.Parse(msg.Label);
                args.CurrentThreadId = Thread.CurrentThread.ManagedThreadId;
                try
                {
                    if (Execute != null)
                    {
                        Execute(this, args);
                    }
                }
                catch (Exception ex)
                {
                    QueueExceptionArg<T> exception = new QueueExceptionArg<T>();
                    if (!string.IsNullOrEmpty(msg.Label))
                        exception.Counter = int.Parse(msg.Label);
                    exception.Message = message;
                    exception.Exception = ex;
                    exception.CurrentThreadId = Thread.CurrentThread.ManagedThreadId;
                    if (ExecuteError != null)
                    {
                        ExecuteError(this, exception);
                    }
                }
                if (isRunning)
                {
                    queue.BeginReceive();
                }
            }
            catch
            {
                if (isRunning)
                {
                    queue.BeginReceive();
                }
            }
        }
    }
}
