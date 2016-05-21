using System;
using System.Web.UI;
using System.ComponentModel;

namespace WebUserControl.UI
{
    /// <summary>
    /// Renders a simple label.
    /// </summary>
    [ToolboxData("<{0}:ToolbarLabel runat=server></{0}:ToolbarLabel>")]
    public class ToolbarLabel : ToolbarItem
    {

        [Description("Rendered text")]
        [Category("Toolbar")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                string text = (string)this.ViewState["ItemText"];
                return text == null ? String.Empty : text;
            }
            set { ViewState["ItemText"] = value; }
        }


        /// <summary>
        /// Empty default constructor.
        /// </summary>
        public ToolbarLabel()
            : base(HtmlTextWriterTag.Span)
        {
        }


        /// <summary>
        /// Renders the item's <see cref="Text"/> to the client.
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(this.Text);
            base.RenderContents(writer);
        }

    }
}
