using System;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace WebUserControl.UI
{
    /// <summary>
    /// Toolbar设计器
    /// </summary>
    public class ToolbarDesigner : ContainerControlDesigner
    {

        public ToolbarDesigner()
        {
            base.FrameStyle.Width = Unit.Percentage(100);
        }


        public override string GetDesignTimeHtml()
        {
            Toolbar toolbar = this.Component as Toolbar;
            if (toolbar.Items.Count == 0)
            {
                return CreatePlaceHolderDesignTimeHtml("Add Toolbar Items...");
            }
            else
            {
                return base.GetDesignTimeHtml();
            }
        }


        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            string pattern = "Error while creating design time HTML:<br/>{0}";
            return String.Format(pattern, e.Message);
        }
    }
}
