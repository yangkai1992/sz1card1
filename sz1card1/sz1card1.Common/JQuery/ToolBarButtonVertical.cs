using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Drawing.Design;
using System.Reflection;
using System.Web.UI.WebControls;

namespace sz1card1.Common.JQuery
{
    public class BaseToolBarButtonVertical<T> : BaseAjaxControl<T> where T : class
    {
        [DefaultValue("")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Category("Appearance")]
        public string ImageUrl
        {
            get;
            set;
        }

        protected override string CallingScript
        {
            get
            {
                return string.Format("document.getElementById('{0}').disabled=true;", ClientID);
            }
        }

        protected override string CalledScript
        {
            get
            {
                return string.Format("document.getElementById('{0}').disabled=false;", ClientID);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (EnableCallServer)
            {
                writer.Write(string.Format("<a href=\"javascript:;\"> <div class=\"navIconTool\" onclick=\"AjaxControlRequest({3}id:'{0}',request:{1},callback:{2}{4});return false;\">\n", ClientID, string.IsNullOrEmpty(RequestScript) ? "null" : RequestScript, string.IsNullOrEmpty(CallbackScript) ? "null" : CallbackScript, "{", "}"));
            }
            else
            {
                writer.Write(string.Format("<a href=\"javascript:;\"> <div class=\"navIconTool\" onclick=\"{0}&&{0}();return false;\">\n", string.IsNullOrEmpty(RequestScript) ? "null" : RequestScript));
            }
            writer.Write(string.Format("<img src=\"{0}\" /><br />{1}", this.ResolveUrl(ImageUrl), Text));
            writer.Write("</div></a>\n");
        }
    }

    [ToolboxData("<{0}:ToolBarButtonVertical runat=\"server\"/>")]
    public class ToolBarButtonVertical : BaseToolBarButtonVertical<Dictionary<string, string>>
    {
    }
}
