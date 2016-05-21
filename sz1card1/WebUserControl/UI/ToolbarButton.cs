using System;
using System.Web.UI;
using WebUserControl.Delegate;

namespace WebUserControl.UI
{
    /// <summary>
    /// Renders an image button which posts back to its parent form.
    /// </summary>
    [ToolboxData("<{0}:ToolbarButton runat=server></{0}:ToolbarButton>")]
    public class ToolbarButton : ToolbarImage, IPostBackToolbarItem, IPostBackDataHandler
    {

        #region event declaration

        /// <summary>
        /// Raised if the item is being clicked.
        /// </summary>
        public event ItemEventHandler ItemSubmitted;

        #endregion


        #region initialization

        /// <summary>
        /// Empty default constructor.
        /// </summary>
        public ToolbarButton()
        {
        }

        #endregion


        #region prerender

        /// <summary>
        /// Registers a postback handler.
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>
        /// PostBack is being submitted with coordinates so the
        /// submitted key doesn't match the control's ID
        /// -> explicit postback notification needed.
        /// </remarks>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null)
            {
                this.Page.RegisterRequiresPostBack(this);
            }

            base.OnPreRender(e);
        }

        #endregion


        #region rendering

        /// <summary>
        /// Renders the begin tag of an image button, if the item
        /// is enabled.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (this.RenderDisabled)
            {
                //render a standard image
                base.RenderBeginTag(writer);
            }
            else
            {
                //render an <input> tag
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
            }
        }



        /// <summary>
        /// Adds image button specific attributes to the output.
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!this.RenderDisabled)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "image");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            }

            base.AddAttributesToRender(writer);
        }

        #endregion


        #region postback data handling

        /// <summary>
        /// Raises the control's <see cref="Click"/> event.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            //trigger page validation
            Page.Validate();

            //bubble event
            if (this.ItemSubmitted != null) ItemSubmitted(this);
        }


        /// <summary>
        /// Checks whether the button was clicked or not.
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            //image buttons are being submitted with their coordinates
            string x = postCollection[this.UniqueID + ".x"];
            string y = postCollection[this.UniqueID + ".y"];

            return (x != null && y != null && x.Length > 0 && y.Length > 0);
        }

        #endregion
    }
}
