using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZooKeeperNet;

namespace sz1card1.Common
{
    internal class ZooKeeperWatcher : IWatcher
    {
        private readonly IZookeeperProcess _zooKeeper;
        public ZooKeeperWatcher(IZookeeperProcess zooKeeper)
        {
            _zooKeeper = zooKeeper;
        }
        public void Process(WatchedEvent @event)
        {
            _zooKeeper.Process(@event);
        }
    }
}