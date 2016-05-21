using System.Web.UI;

namespace WebUserControl.UI
{
    /// <summary>
    /// Provides a container item for arbitrary controls
    /// which can be added programmatically.
    /// </summary>
    [ToolboxData("<{0}:ControlContainerItem runat=server></{0}:ControlContainerItem>")]
    public class ControlContainerItem : ToolbarItem
    {

        /// <summary>
        /// Empty default constructor.
        /// </summary>
        public ControlContainerItem()
        {
        }


        #region rendering

        /// <summary>
        /// Prevents rendering of the control itself.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
        {
            //don't render anything
        }

        /// <summary>
        /// Prevents rendering of the control itself.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(System.Web.UI.HtmlTextWriter writer)
        {
            //don't render anything
        }

        /// <summary>
        /// Renders a simple placeholder in design mode.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.IsDesignMode)
            {
                writer.Write("<span style=font-size:10px;color:blue;font-family:arial>[ .. ]</span>");
            }
            else
            {
                base.Render(writer);
            }
        }

        #endregion

    }
}
