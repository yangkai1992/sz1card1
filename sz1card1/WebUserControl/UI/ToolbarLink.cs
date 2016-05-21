using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;

namespace WebUserControl.UI
{
    /// <summary>
    /// Renders a link with a simple image.
    /// </summary>
    [ToolboxData("<{0}:ToolbarLink runat=server></{0}:ToolbarLink>")]
    public class ToolbarLink : ToolbarImage
    {

        #region properties

        /// <summary>
        /// Target of the hyperlink.
        /// </summary>
        [Category("Toolbar")]
        [Description("Link target (_self, _blank, ...).")]
        [DefaultValue("")]
        public string LinkTarget
        {
            get
            {
                string url = (string)this.ViewState["LinkTarget"];
                return url == null ? String.Empty : url;
            }
            set { ViewState["LinkTarget"] = value; }
        }


        /// <summary>
        /// URL which is opended by the link.
        /// </summary>
        [Category("Toolbar")]
        [Description("URL to be opened by the link.")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        [Localizable(true)]
        public string NavigateUrl
        {
            get
            {
                string url = (string)this.ViewState["NavigateUrl"];
                return url == null ? String.Empty : url;
            }
            set
            {
                ViewState["NavigateUrl"] = value;
            }
        }

        #endregion


        #region initialization

        /// <summary>
        /// Inits the control.
        /// </summary>
        public ToolbarLink()
        {
        }


        #endregion


        #region rendering

        /// <summary>
        /// Renders the link item.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            //render js start if needed
            //if (this.RenderRollOverScripts)
                //RollOverHandler.RenderRollOverBeginTag(writer, this, ResolveUrl(RollOverImageUrl));

            //render <a...> begin tag
            this.RenderBeginTag(writer);

            //render content of the link (image and label)
            this.RenderContents(writer);

            //render closing </a> tag
            this.RenderEndTag(writer);

            //render js end tag
            //if (this.RenderRollOverScripts)
                //RollOverHandler.RenderRollOverEndTag(writer);
        }



        /// <summary>
        /// Renders the link's begin tag and its attributes.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (!RenderDisabled && this.NavigateUrl != String.Empty)
            {
                this.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }
        }


        /// <summary>
        /// Adds the links attributes to the link tag.
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            //only render a link if the item is not disabled
            if (!this.RenderDisabled && this.NavigateUrl != String.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, this.ResolveUrl(this.NavigateUrl));
                if (this.ToolTip != String.Empty)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, this.ToolTip);
                }

                if (this.LinkTarget.Length > 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Target, this.LinkTarget);
                }
            }
        }


        /// <summary>
        /// Renders the link's end tag.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (!RenderDisabled && this.NavigateUrl != String.Empty)
            {
                writer.RenderEndTag();
            }
        }


        /// <summary>
        /// Renders the the inner image and text.
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.LabelText != String.Empty && this.LabelTextAlign == TextAlign.Left)
            {
                this.RenderLabel(writer);
            }

            //render image, if available
            if (this.ImageUrl != String.Empty)
            {
                //render image
                base.AddAttributesToRender(writer);
                base.RenderBeginTag(writer);
                base.RenderContents(writer);
                base.RenderEndTag(writer);
            }

            if (this.LabelText != String.Empty && this.LabelTextAlign == TextAlign.Right)
            {
                this.RenderLabel(writer);
            }
        }

        #endregion

    }
}
