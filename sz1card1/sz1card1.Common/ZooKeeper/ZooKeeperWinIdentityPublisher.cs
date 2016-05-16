using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using sz1card1.Common.IdentityImpersonate;

namespace sz1card1.Common
{
    internal class ZooKeeperWinIdentityPublisher:ZookeeperPublisher
    {
        public ZooKeeperWinIdentityPublisher(string nodeName, int sessionTimeout)
            : base(null, nodeName, sessionTimeout)
        {
            string identity = WindowsIdentity.GetCurrent().User.ToString();
            _root += "/" + identity;
        }

        public ZooKeeperWinIdentityPublisher(string nodeName)
            :this(nodeName,30000)
        {
        }
    }
}
