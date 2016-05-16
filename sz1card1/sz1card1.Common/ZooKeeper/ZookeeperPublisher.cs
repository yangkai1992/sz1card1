using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Configuration;
using ZooKeeperNet;

namespace sz1card1.Common
{
    /// <summary>
    /// Zookeeper发布者
    /// </summary>
    public class ZookeeperPublisher : IZookeeperProcess
    {
        private static readonly object _lock = new object();
        private static ZooKeeper _zkInstance;
        private static readonly AutoResetEvent _connetedEvent = new AutoResetEvent(false);
        private static readonly string configurationNodeName;
        private readonly string _nodeName;
        private readonly int _sessionTimeout;
        private readonly IWatcher _watcher;

        protected string _root = "/root";

        static ZookeeperPublisher()
        {
            configurationNodeName = ConfigurationManager.AppSettings["sz1card1.zookeeper"];
        }

        protected ZookeeperPublisher(string nodeName)
            : this(null,nodeName, 30000)
        {
        }

        public ZookeeperPublisher(string nodePath,string nodeName)
            : this(nodePath, nodeName, 30000)
        {
        }

        public ZookeeperPublisher(string nodePath, string nodeName, int sessionTimeout)
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
                        var zkInstance = new ZooKeeper(configurationNodeName, TimeSpan.FromMilliseconds(_sessionTimeout), new ZooKeeperWatcher(this));
                        //等待连接完成
                        if (!_connetedEvent.WaitOne(_sessionTimeout))
                        {
                            throw new Exception(string.Format("创建ZooKeeper连接失败，连接到{0}超时", configurationNodeName));
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
        }

        /// <summary>
        /// 发布数据
        /// </summary>
        /// <param name="value"></param>
        public void Publish(string value)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
            var stat = getZooKeeperInstance().Exists(_root + "/" + _nodeName, false);
            if (stat == null)
            {
                try
                {
                    getZooKeeperInstance().Create(_root + "/" + _nodeName, new byte[0], Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }
                catch(ZooKeeperNet.KeeperException.NoNodeException) //路径根目录不存在时，则创建根目录再创建节点
                {
                    CreatePath(getZooKeeperInstance(), _root);
                    getZooKeeperInstance().Create(_root + "/" + _nodeName, new byte[0], Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }
            }
            getZooKeeperInstance().SetData(_root + "/" + _nodeName, data, -1);
        }
    }


}