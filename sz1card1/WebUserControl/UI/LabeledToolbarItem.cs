using System;
using System.ComponentModel;
using System.Drawing.Design;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace sz1card1.Common.UI
{
    public abstract class LabeledToolbarItem : ToolbarItem
    {

        #region properties

        /// <summary>
        /// Text of an additional label.
        /// </summary>
        [NotifyParentProperty(true)]
        [Category("ToolbarItem Label")]
        [DefaultValue("")]
        [Description("Optional text for an additional label")]
        [Localizable(true)]
        public string LabelText
        {
            get
            {
                string text = (string)this.ViewState["LabelText"];
                return text == null ? String.Empty : text;
            }
            set { ViewState["LabelText"] = value; }
        }


        /// <summary>
        /// Whether the label shall be aligned on the right or left
        /// hand side of the item's main object (e.g. an image).
        /// </summary>
        [NotifyParentProperty(true)]
        [Category("ToolbarItem Label")]
        [DefaultValue(TextAlign.Right)]
        [Description("Alignment of the label text")]
        public TextAlign LabelTextAlign
        {
            get
            {
                object obj = this.ViewState["ItemTextAlign"];
                return obj == null ? TextAlign.Right : (TextAlign)obj;
            }
            set { ViewState["ItemTextAlign"] = value; }
        }


        /// <summary>
        /// Whether the label shall be aligned on the right or left
        /// hand side of the item's main object (e.g. an image).
        /// </summary>
        [NotifyParentProperty(true)]
        [Category("ToolbarItem Label")]
        [DefaultValue(VerticalAlign.Middle)]
        [Description("Vertical alignment of the label to the main item.")]
        public VerticalAlign LabelTextVerticalAlign
        {
            get
            {
                object obj = this.ViewState["LabelTextVerticalAlign"];
                return obj == null ? VerticalAlign.Middle : (VerticalAlign)obj;
            }
            set { ViewState["LabelTextVerticalAlign"] = value; }
        }



        /// <summary>
        /// A CSS style to assign to the label.
        /// </summary>
        [NotifyParentProperty(true)]
        [Category("ToolbarItem Label")]
        [DefaultValue("")]
        [Description("CSS class of the item label.")]
        public string LabelCssClass
        {
            get
            {
                string text = (string)this.ViewState["LabelCssClass"];
                return text == null ? String.Empty : text;
            }
            set { ViewState["LabelCssClass"] = value; }
        }

        #endregion


        #region rendering

        /// <summary>
        /// Renders the label within HTML <c>span</c> tags and optional
        /// CSS class settings.
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderLabel(HtmlTextWriter writer)
        {
            //don't render anything if there is no label text...
            if (this.LabelText == String.Empty) return;

            //enter vertical alignment - default is middle
            string valign = "middle";
            if (this.LabelTextVerticalAlign == VerticalAlign.Top)
                valign = "top";
            else if (this.LabelTextVerticalAlign == VerticalAlign.Bottom)
                valign = "bottom";

            //render style
            writer.AddStyleAttribute("vertical-align", valign);

            if (this.LabelCssClass != String.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.LabelCssClass);
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(this.LabelText);
            writer.RenderEndTag();
        }


        /// <summary>
        /// Renders the label within HTML <c>span</c> tags and optional
        /// CSS class settings.
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderLabel(HtmlTextWriter writer, string OnClientClick)
        {
            //don't render anything if there is no label text...
            if (this.LabelText == String.Empty) return;

            //enter vertical alignment - default is middle
            string valign = "middle";
            if (this.LabelTextVerticalAlign == VerticalAlign.Top)
                valign = "top";
            else if (this.LabelTextVerticalAlign == VerticalAlign.Bottom)
                valign = "bottom";

            //render style
            writer.AddStyleAttribute("vertical-align", valign);
            writer.AddStyleAttribute("cursor", "pointer");

            if (this.LabelCssClass != String.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.LabelCssClass);
            }

            if (OnClientClick != string.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("document.getElementsByName('{0}')[0].click();",this.UniqueID));
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(this.LabelText);
            writer.RenderEndTag();
        }

        #endregion

    }
}
