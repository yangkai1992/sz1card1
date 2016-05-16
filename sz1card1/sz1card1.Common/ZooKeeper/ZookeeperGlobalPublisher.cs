using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sz1card1.Common
{
    internal class ZookeeperGlobalPublisher : ZookeeperPublisher
    {
        public ZookeeperGlobalPublisher(string nodeName)
            : this(nodeName, 30000)
        {
        }

        public ZookeeperGlobalPublisher(string nodeName, int sessionTimeout)
            : base(null, nodeName, sessionTimeout)
        {
            _root +="/global";
        }
    }
}
