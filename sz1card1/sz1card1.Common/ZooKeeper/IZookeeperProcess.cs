using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZooKeeperNet;

namespace sz1card1.Common
{
    interface IZookeeperProcess
    {
       void Process(WatchedEvent @event);
    }
}