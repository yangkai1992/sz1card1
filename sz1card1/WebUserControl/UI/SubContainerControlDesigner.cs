using System;
using System.Collections;
using System.Collections.Specialized;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;

namespace sz1card1.Common.UI
{
    /// <summary>
    /// 选项卡编辑类
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public class SubContainerControlDesigner : ContainerControlDesigner
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SubContainerControlDesigner()
        {
            base.FrameStyle.Width = Unit.Percentage(100);
        }
    }
}
