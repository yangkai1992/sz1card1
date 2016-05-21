using System;
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using System.Drawing.Design;
using System.Web.UI;

namespace sz1card1.Common.UI
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DragDropPanelCollection : ControlCollection
    {
        public DragDropPanelCollection(Control owner):base(owner)
        {
        }

        public override void Add(Control child)
        {
            if (!(child is DragDropPanel))
            {
                throw new ArgumentException("该集合只能添加DragDropPanel类型的控件");
            }
            base.Add(child);
        }

        public override void AddAt(int index, Control child)
        {
            if (!(child is DragDropPanel))
            {
                throw new ArgumentException("该集合只能添加DragDropPanel类型的控件");
            }
            base.AddAt(index, child);
        }

        public new DragDropPanel this[int index]
        {
            get
            {
                return (DragDropPanel)base[index];
            }
        }
    }
}
