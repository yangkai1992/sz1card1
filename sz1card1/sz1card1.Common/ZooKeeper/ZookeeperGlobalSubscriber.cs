using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    internal class ZookeeperGlobalSubscriber:ZookeeperSubscriber
    {
        public ZookeeperGlobalSubscriber(string nodeName)
            :this(nodeName,30000)
        {
        }

        public ZookeeperGlobalSubscriber(string nodeName ,int sessionTimeout)
            :base(null,nodeName,sessionTimeout)
        {
            _root += "/global";
        }
    }
}
