using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using sz1card1.Common.Enum;

namespace sz1card1.Common.Timeline
{
    public class TimelineService<T> where T : ITimeline
    {
        private List<T> currentList = new List<T>();
        private List<T> expiredList = new List<T>();
        private bool dirtyFlag = false;
        private PersistenceTypes persistenceType = PersistenceTypes.None;
        private int persistenceSeconds = 10;
        private int executeSecondes = 1;
        private string persistenceFilePath;
        private bool isRunning = false;

        /// <summary>
        /// 清理过期节点的时间间隔
        /// </summary>
        private TimeSpan clearExpiredNodeTimeSpan = new TimeSpan(30, 0, 0, 0);//一个月

        public event EventHandler<TimelineEventArg<T>> Execute;

        public TimelineService()
        {
        }

        public TimelineService(string filePath)
        {
            this.persistenceFilePath = filePath;
            Load();
        }

        /// <summary>
        /// 持久化类型
        /// </summary>
        public PersistenceTypes PersistenceType
        {
            get
            {
                return persistenceType;
            }
            set
            {
                persistenceType = value;
            }
        }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        /// <summary>
        /// 定时持久化间隔秒数
        /// </summary>
        public int PersistenceSeconds
        {
            get
            {
                return persistenceSeconds;
            }
            set
            {
                persistenceSeconds = value;
            }
        }

        /// <summary>
        /// 持久化目录路径
        /// </summary>
        public string PersistenceFilePath
        {
            get
            {
                return persistenceFilePath;
            }
            set
            {
                persistenceFilePath = value;
            }
        }

        /// <summary>
        /// 执行间隔秒数
        /// </summary>
        public int ExecuteSecondes
        {
            get
            {
                return executeSecondes;
            }
            set
            {
                executeSecondes = value;
            }
        }

        /// <summary>
        /// 是否发生改变
        /// </summary>
        public bool DirtyFlag
        {
            get
            {
                return dirtyFlag;
            }
        }

        /// <summary>
        /// 插入节点到当前时间轴
        /// </summary>
        /// <param name="value">节点</param>
        public void Insert(T value)
        {
            Remove(value.Id);
            lock (currentList)
            {
                int low = 0;
                int high = currentList.Count - 1;
                int middle = -1;
                bool status = false;
                while (low <= high)
                {
                    middle = (high + low) / 2;
                    if (currentList[middle].Id == value.Id)
                        throw new ArgumentException("Id");
                    if (currentList[middle].ExecuteTime.CompareTo(value.ExecuteTime) == 0)
                    {
                        status = true;
                        break;
                    }
                    else if (currentList[middle].ExecuteTime.CompareTo(value.ExecuteTime) < 0)
                        low = middle + 1;
                    else
                        high = middle - 1;
                }

                if (status)
                {
                    currentList.Insert(middle + 1, value);
                }
                else
                {
                    currentList.Insert(low, value);

                }
                dirtyFlag = true;
                if (persistenceType == PersistenceTypes.OnChanged)
                {
                    Save();
                }
                if (low == 0)
                {
                    Monitor.PulseAll(currentList);
                }
            }
        }

        /// <summary>
        /// 插入节点到过期时间轴
        /// </summary>
        /// <param name="value">节点</param>
        public void InsertExpiredNode(T value)
        {
            lock (expiredList)
            {
                expiredList.RemoveAll(t => t.Id == value.Id);
                expiredList.Add(value);
                if (persistenceType == PersistenceTypes.OnChanged)
                {
                    SaveExpired();
                }
                Monitor.PulseAll(expiredList);
            }
        }

        /// <summary>
        /// 获取满足条件的过期节点并推向客户端
        /// </summary>
        /// <param name="clause">谓词表达式</param>
        /// <returns></returns>
        public void GetExpiredNodes(Predicate<T> clause)
        {
            List<T> list = expiredList.FindAll(clause);
            foreach (T item in list)
            {
                if (Execute != null)
                {
                    TimelineEventArg<T> args = new TimelineEventArg<T>() { Message = item };
                    Execute.BeginInvoke(this, args, new AsyncCallback(OnExecuted), Execute);
                }
                RemoveExpiredNode(item.Id);
            }
        }

        /// <summary>
        /// 开始处理
        /// </summary>
        public void Start()
        {
            isRunning = true;
            if (persistenceType == PersistenceTypes.Timely)
            {
                Thread persistenceThread = new Thread(new ThreadStart(delegate()
                {
                    while (isRunning)
                    {
                        if (dirtyFlag)
                        {
                            lock (currentList)
                            {
                                Save();
                            }
                            lock (expiredList)
                            {
                                SaveExpired();
                            }
                        }
                        Thread.Sleep(persistenceSeconds * 1000);
                    }
                }));
                persistenceThread.IsBackground = true;
                persistenceThread.Start();
            }
            Thread executeThread = new Thread(new ThreadStart(delegate()
            {
                while (isRunning)
                {
                    lock (currentList)
                    {
                        if (currentList.Count == 0)
                        {
                            Monitor.Wait(currentList);
                        }
                        else
                        {
                            TimeSpan waitSpan = currentList[0].ExecuteTime - DateTime.Now;
                            //处理历史节点
                            if (waitSpan <= new TimeSpan(0, 0, -30))
                            {
                                if (currentList[0].RedoOnExpired)
                                {
                                    if (Execute != null)
                                    {
                                        TimelineEventArg<T> args = new TimelineEventArg<T>() { Message = currentList[0] };
                                        Execute.BeginInvoke(this, args, new AsyncCallback(OnExecuted), Execute);
                                    }
                                }
                                RemoveFirst();
                            }
                            else if (waitSpan > new TimeSpan(0, 0, -30) && waitSpan <= new TimeSpan(0, 0, 0))
                            {
                                if (Execute != null)
                                {
                                    TimelineEventArg<T> args = new TimelineEventArg<T>() { Message = currentList[0] };
                                    Execute.BeginInvoke(this, args, new AsyncCallback(OnExecuted), Execute);
                                }
                                RemoveFirst();
                            }
                            else
                            {
                                Monitor.Wait(currentList, (int)waitSpan.TotalMilliseconds);
                            }
                        }
                    }
                }
            }));
            executeThread.IsBackground = true;
            executeThread.Start();
            ClearExpiredNode();
        }

        /// <summary>
        /// 停止时间服务
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            Save();
            SaveExpired();
        }

        private void OnExecuted(IAsyncResult ar)
        {
            EventHandler<TimelineEventArg<T>> execute = ar.AsyncState as EventHandler<TimelineEventArg<T>>;
            execute.EndInvoke(ar);
        }

        /// <summary>
        /// 删除一个待处理事件
        /// </summary>
        /// <param name="identity">标识</param>
        public void Remove(Guid id)
        {
            for (int i = 0; i < currentList.Count; i++)
            {
                if (currentList[i].Id == id)
                {
                    lock (currentList)
                    {
                        currentList.RemoveAt(i);

                        dirtyFlag = true;
                        if (persistenceType == PersistenceTypes.OnChanged)
                        {
                            Save();
                        }
                        if (i == 0)
                        {
                            Monitor.PulseAll(currentList);
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 删除过期事件
        /// </summary>
        /// <param name="id"></param>
        public void RemoveExpiredNode(Guid id)
        {
            for (int i = 0; i < expiredList.Count; i++)
            {
                if (expiredList[i].Id == id)
                {
                    lock (expiredList)
                    {
                        expiredList.RemoveAt(i);
                        dirtyFlag = true;
                        if (persistenceType == PersistenceTypes.OnChanged)
                        {
                            SaveExpired();
                        }
                        if (i == 0)
                        {
                            Monitor.PulseAll(expiredList);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除第一个事件
        /// </summary>
        private void RemoveFirst()
        {
            if (currentList.Count != 0)
            {
                if (currentList[0].IntervalType != IntervalTypes.Once && currentList[0].ExecuteTimes != 1)
                {
                    T node = (T)currentList[0].Clone();
                    if (currentList[0].ExecuteTimes > 1)
                    {
                        node.ExecuteTimes--;
                    }
                    switch (node.IntervalType)
                    {
                        case IntervalTypes.Daily:
                            while (DateTime.Now >= node.ExecuteTime)
                            {
                                node.ExecuteTime = node.ExecuteTime.AddDays(node.IntervalCount);
                            }
                            break;
                        case IntervalTypes.Weekly:
                            while (DateTime.Now >= node.ExecuteTime)
                            {
                                node.ExecuteTime = node.ExecuteTime.AddDays(7 * node.IntervalCount);
                            }
                            break;
                        case IntervalTypes.Monthly:
                            while (DateTime.Now >= node.ExecuteTime)
                            {
                                node.ExecuteTime = node.ExecuteTime.AddMonths(node.IntervalCount);
                            }
                            break;
                        case IntervalTypes.Yearly:
                            while (DateTime.Now >= node.ExecuteTime)
                            {
                                node.ExecuteTime = node.ExecuteTime.AddYears(node.IntervalCount);
                            }
                            break;
                        default:
                            break;
                    }
                    currentList.RemoveAt(0);
                    Insert(node);
                }
                else
                {
                    currentList.RemoveAt(0);
                }
                dirtyFlag = true;
                if (persistenceType == PersistenceTypes.OnChanged)
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// 清理过期已久的垃圾消息节点
        /// </summary>
        private void ClearExpiredNode()
        {
            Thread executeThread = new Thread(new ThreadStart(delegate()
            {
                try
                {
                    while (isRunning)
                    {
                        lock (expiredList)
                        {
                            if (expiredList.Count == 0)
                            {
                                Monitor.Wait(expiredList);
                            }
                            TimeSpan waitSpan = DateTime.Now - expiredList[0].ExecuteTime;
                            if (waitSpan >= clearExpiredNodeTimeSpan)
                            {
                                RemoveExpiredNode(expiredList[0].Id);
                            }
                            else
                            {
                                Monitor.Wait(expiredList, (int)waitSpan.TotalMilliseconds);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    sz1card1.Common.Log.LoggingService.Warn(this, ex);
                }
            }));
            executeThread.IsBackground = true;
            executeThread.Start();
        }

        private void Save()
        {
            if (persistenceFilePath == string.Empty)
                throw new ArgumentNullException("persistenceFilePath");
            if (!Directory.Exists(persistenceFilePath))
            {
                Directory.CreateDirectory(persistenceFilePath);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream fs = new FileStream(persistenceFilePath + "currentList.xml", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                serializer.Serialize(fs, currentList);
                dirtyFlag = false;
            }
        }

        private void SaveExpired()
        {
            if (persistenceFilePath == string.Empty)
                throw new ArgumentNullException("persistenceFilePath");
            if (!Directory.Exists(persistenceFilePath))
            {
                Directory.CreateDirectory(persistenceFilePath);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream fs = new FileStream(persistenceFilePath + "expiredList.xml", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                serializer.Serialize(fs, expiredList);
                dirtyFlag = false;
            }
        }

        private void Load()
        {
            if (persistenceFilePath == string.Empty)
                throw new ArgumentNullException("persistenceFilePath");
            if (!Directory.Exists(persistenceFilePath))
            {
                Directory.CreateDirectory(persistenceFilePath);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream currentfs = new FileStream(persistenceFilePath + "currentList.xml", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            {
                if (currentfs.Length > 0)
                {
                    lock (currentList)
                    {
                        currentList = (List<T>)serializer.Deserialize(currentfs);
                        currentList.Sort((x, y) => x.ExecuteTime.CompareTo(y.ExecuteTime));
                    }
                }
            }
            using (FileStream expiredfs = new FileStream(persistenceFilePath + "expiredList.xml", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            {
                if (expiredfs.Length > 0)
                {
                    lock (expiredList)
                    {
                        expiredList = (List<T>)serializer.Deserialize(expiredfs);
                    }
                }
            }
        }
    }
}
