
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using ZooKeeperNet;

namespace sz1card1.Common.DistributedLock
{
    /// <summary>
    /// ZooKeeper实现的分布式锁 by zxq
    /// </summary>
    public class ZooKeeperDistributedLock : IDisposable
    {
        private static ZooKeeper _zkInstance;
        private static readonly AutoResetEvent _connetedEvent = new AutoResetEvent(false);
        private static readonly object _lock = new object();
        private static readonly string _root = "/root/sz1card1/lock"; //根        
        private readonly string _lockName; //竞争资源的标志        
        private string _waitNode; //等待前一个锁        
        private string _myZnode; //当前锁 
        private int _sessionTimeout;
        private ManualResetEvent _autoEvent;
        

        /// <summary>
        /// 服务器地址
        /// </summary>
        private static readonly string Server;

        static ZooKeeperDistributedLock()
        {
            Server = ConfigurationManager.AppSettings["sz1card1.zookeeper"];
        }

        /// <summary>
        /// 创建分布式锁
        /// </summary>
        /// <param name="lockName">锁名称</param>
        public ZooKeeperDistributedLock(string lockName)
            : this(lockName, 30000)
        {

        }

        /// <summary>
        /// 创建分布式锁
        /// </summary>
        /// <param name="lockName">锁名称</param>
        /// <param name="sessionTimeout">session超时时间</param>
        public ZooKeeperDistributedLock(string lockName, int sessionTimeout)
        {
            this._lockName = lockName;
            this._sessionTimeout = sessionTimeout;
            const string splitStr = "lock";
            if (lockName.Contains(splitStr))
            {
                throw new Exception("lockName can not contains lock");
            }
        }

        /// <summary>
        /// ZooKeeper实例
        /// </summary>
        /// <returns></returns>
        private ZooKeeper getZooKeeperInstance()
        {
            if (_zkInstance == null || !_zkInstance.State.Equals(ZooKeeper.States.CONNECTED))
            {
                lock (_lock)
                {
                    if (_zkInstance == null || !_zkInstance.State.Equals(ZooKeeper.States.CONNECTED))
                    {
                        //释放上一个连接
                        if (_zkInstance != null) _zkInstance.Dispose();
                        //创建一个新连接
                        _connetedEvent.Reset();
                        var zkInstance = new ZooKeeper(Server, TimeSpan.FromMilliseconds(_sessionTimeout), new ZooKeeperWatcher(this));
                        //等待连接完成
                        if (!_connetedEvent.WaitOne(_sessionTimeout))
                        {
                            throw new Exception(string.Format("创建ZooKeeper连接失败，连接到{0}超时", Server));
                        }
                        CreatePath(zkInstance, _root);
                        _zkInstance = zkInstance;
                    }
                }
            }
            return _zkInstance;
        }

        private void CreatePath(ZooKeeper zkInstance, string root)
        {
            var dirs = root.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var path = string.Empty;
            foreach (var dir in dirs)
            {
                path += "/" + dir;
                //判断根节点是否存在，不存在则创建
                var stat = zkInstance.Exists(path, false);
                if (stat == null)
                {
                    // 创建根节点                    
                    zkInstance.Create(path, new byte[0], Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }
            }
        }

        /// <summary>        
        /// zookeeper节点的监视器        
        /// </summary>        
        public void Process(WatchedEvent @event)
        {
            //Console.WriteLine(@event.Path + @event.State);
            if (@event.State == KeeperState.Expired && _zkInstance != null)
            {
                _zkInstance.Dispose();
                _zkInstance = null;
                return;
            }
            if (@event.State == KeeperState.SyncConnected && @event.Type == EventType.None)
            {
                _connetedEvent.Set(); //通知连接完成
            }
            if (@event.Path == _waitNode && @event.Type == EventType.NodeDeleted && _autoEvent != null)
            {
                this._autoEvent.Set();
            }
        }

        /// <summary>
        /// 尝试获得锁
        /// </summary>
        /// <returns></returns>
        public bool TryLock()
        {
            const string splitStr = "_lock_";
            try
            {
                //创建临时子节点                
                _myZnode = getZooKeeperInstance().Create(_root + "/" + _lockName + splitStr, new byte[0], Ids.OPEN_ACL_UNSAFE, CreateMode.EphemeralSequential);
            }
            catch (ZooKeeperNet.KeeperException.NoNodeException)
            {
                CreatePath(getZooKeeperInstance(), _root);
                //创建临时子节点                
                _myZnode = getZooKeeperInstance().Create(_root + "/" + _lockName + splitStr, new byte[0], Ids.OPEN_ACL_UNSAFE, CreateMode.EphemeralSequential);
            }
            //取出所有子节点                
            var subNodes = getZooKeeperInstance().GetChildren(_root, false);
            //取出所有lockName的锁                
            IList<string> lockObjNodes = subNodes.Where(node => node.StartsWith(_lockName)).ToList();
            lockObjNodes = lockObjNodes.OrderBy(p => p).ToList();
            if (_myZnode.Equals(_root + "/" + lockObjNodes[0]))
            {
                //如果是最小的节点,则表示取得锁                    
                return true;
            }
            //如果不是最小的节点，找到比自己小1的节点               
            long subMyZnode = long.Parse(_myZnode.Substring(_myZnode.LastIndexOf("_", StringComparison.Ordinal) + 1));
            _waitNode = _root + "/" + _lockName + splitStr + (subMyZnode - 1).ToString("0000000000");
            return false;
        }

        /// <summary>
        /// 尝试获得锁,若锁已被占用则等待直到超时
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool TryLock(TimeSpan time)
        {
            if (this.TryLock())
            {
                return true;
            }
            return WaitForLock(_waitNode, time);
        }

        private bool WaitForLock(string lower, TimeSpan waitTime)
        {
            _autoEvent = new ManualResetEvent(false);
            var stat = getZooKeeperInstance().Exists(lower, new ZooKeeperWatcher(this));
            //判断比自己小一个数的节点是否存在,如果不存在则无需等待锁,同时注册监听            
            if (stat != null)
            {
                //Console.WriteLine("Thread " + System.Threading.Thread.CurrentThread.Name + " waiting for " + root + "/" + lower);
                bool r = _autoEvent.WaitOne(waitTime);
                _autoEvent.Dispose();
                _autoEvent = null;
                return r;
            }
            else return true;
        }

        public void UnLock()
        {
            getZooKeeperInstance().Delete(_myZnode, -1);
            _myZnode = null;
        }

        /// <summary>
        /// 获取已阻塞的长度
        /// </summary>
        public int BlockLength
        {
            get
            {
                return getZooKeeperInstance().GetChildren(_root, false).Where(p => p.StartsWith(_lockName)).Count();
            }
        }

        #region Dispose

        //是否回收完毕
        bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~ZooKeeperDistributedLock()
        {
            Dispose(false);
        }
        //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return; //如果已经被回收，就中断执行
            if (disposing)
            {
                //TODO:释放那些实现IDisposable接口的托管对象
                if (_myZnode != null)
                {
                    UnLock();
                }
                if (_autoEvent != null)
                {
                    _autoEvent.Dispose();
                    _autoEvent = null;
                }
            }
            //TODO:释放非托管资源，设置对象为null
            _disposed = true;
        }

        #endregion
    }

    internal class ZooKeeperWatcher : IWatcher
    {
        private readonly ZooKeeperDistributedLock _zooKeeperDistributedLock;
        public ZooKeeperWatcher(ZooKeeperDistributedLock zooKeeperDistributedLock)
        {
            _zooKeeperDistributedLock = zooKeeperDistributedLock;
        }
        public void Process(WatchedEvent @event)
        {
            _zooKeeperDistributedLock.Process(@event);
        }
    }
}


