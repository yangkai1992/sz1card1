using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;

namespace sz1card1.Common
{
    internal class ZooKeeperApplicationSubscriber:ZookeeperSubscriber
    {
        public ZooKeeperApplicationSubscriber(string nodeName, int sessionTimeout)
            : base(null, nodeName, sessionTimeout)
        {
            string identity = string.Empty;
            if (HttpContext.Current == null)
            {
                //服务
                identity = Application.ProductName;
            }
            else
            {
                //网站
                identity = HostingEnvironment.ApplicationHost.GetSiteName();
            }
            _root += "/" + identity;
        }

         public ZooKeeperApplicationSubscriber(string nodeName)
            :this(nodeName,30000)
        {
        }
    }
}
