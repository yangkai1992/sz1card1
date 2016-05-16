using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Configuration;
using ZooKeeperNet;
using sz1card1.Common.Log;

namespace sz1card1.Common
{
    /// <summary>
    /// Zookeeper订阅者
    /// </summary>
    public class ZookeeperSubscriber : IZookeeperProcess, IDisposable
    {
        private static readonly object _lock = new object();
        private static ZooKeeper _zkInstance;
        private static readonly AutoResetEvent _connetedEvent = new AutoResetEvent(false);
        private AutoResetEvent _autoEvent = new AutoResetEvent(false);
        private static readonly string Server;
        private readonly string _nodeName;
        private readonly int _sessionTimeout;
        private readonly IWatcher _watcher;
        private bool _watching;

        protected string _root = "/root";

        /// <summary>
        /// 当节点数据变化时
        /// </summary>
        internal Action<byte[]> OnDataChanged;
        /// <summary>
        /// 当节点被删除时
        /// </summary>
        internal Action OnNodeDeleted;

        static ZookeeperSubscriber()
        {
            Server = ConfigurationManager.AppSettings["sz1card1.zookeeper"];
        }

        public ZookeeperSubscriber(string nodeName)
            : this(null, nodeName, 30000)
        {
        }


        public ZookeeperSubscriber(string nodePath, string nodeName)
            : this(nodePath, nodeName, 30000)
        {
        }

        public ZookeeperSubscriber(string nodePath, string nodeName, int sessionTimeout)
        {
            this._root += nodePath;
            this._nodeName = nodeName;
            this._sessionTimeout = sessionTimeout;
            this._watcher = new ZooKeeperWatcher(this);
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
                        var dirs = _root.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
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
                        _zkInstance = zkInstance;
                    }
                }
            }
            return _zkInstance;
        }

        /// <summary>
        /// 获取数据（立即）
        /// </summary>
        /// <returns></returns>
        private byte[] getData()
        {
            if (getZooKeeperInstance().Exists(_root + "/" + _nodeName, false) != null)
            {
                return getZooKeeperInstance().GetData(_root + "/" + _nodeName, false, null);
            }
            return null;
        }

        /// <summary>
        /// 开始监视
        /// </summary>
        private void StartWatch()
        {
            if (!_watching)
            {
                LoggingService.Info("path:" + _root + "/" + _nodeName);
                getZooKeeperInstance().Exists(_root + "/" + _nodeName, this._watcher);
                _watching = true;
            }
        }

        /// <summary>
        /// 等待节点数据变化，如果与给定值不同，则立即返回
        /// </summary>
        /// <param name="timeout">超时时间</param>
        /// <param name="value">给定值</param>
        /// <returns>是否超时</returns>
        public bool WaitForChanged(TimeSpan timeout, ref string value)
        {
            var data = getData();
            if (data != null && System.Text.Encoding.UTF8.GetString(data) != value)
            {
                value = System.Text.Encoding.UTF8.GetString(data);
                return true;
            }
            else
            {
                StartWatch();

                if (!_autoEvent.WaitOne(timeout))
                {
                    return false;
                }
                data = getData();
                if (data != null)
                {
                    value = System.Text.Encoding.UTF8.GetString(data);
                }
                else
                {
                    value = null;
                }
                return true;
            }
        }

        public void Process(WatchedEvent @event)
        {
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

            if (@event.Type == EventType.NodeDataChanged)
            {
                if (_autoEvent != null) _autoEvent.Set();
                if (OnDataChanged != null)
                {
                    var data = getData();
                    if (data != null)
                    {
                        OnDataChanged(data);
                    }
                }
            }
            else if (@event.Type == EventType.NodeDeleted)
            {
                if (OnNodeDeleted != null)
                {
                    OnNodeDeleted();
                }
            }
            else if (@event.Type == EventType.NodeCreated)
            {
                var data = getData();
                if (data != null)
                {
                    if (_autoEvent != null) _autoEvent.Set();
                    if (OnDataChanged != null) OnDataChanged(data);
                }
                else
                {
                    getZooKeeperInstance().GetData(_root + "/" + _nodeName, this._watcher, null);
                }
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
        ~ZookeeperSubscriber()
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


}