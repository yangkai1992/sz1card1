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
    public class BaseToolBarButtonHorizontal<T> : BaseAjaxControl<T> where T : class
    {
        [Browsable(true)]
        [Description("图标样式")]
        public string IconCss
        {
            get;
            set;
        }
        protected override string CallingScript
        {
            get
            {
                return string.Format("document.getElementById('{0}').disabled=true;document.getElementById('{0}').style.color=\"#ccc\";", ClientID);
            }
        }

        protected override string CalledScript
        {
            get
            {
                return string.Format("document.getElementById('{0}').disabled=false;document.getElementById('{0}').style.color=\"black\"", ClientID);
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write("<a href=\"javascript:;\">\n");
            if (!Enabled)
            {
                writer.Write(string.Format("<span id=\""+ClientID+"\" class=\"{0}\" disabled=\"true\" style=\"color:#ccc\" onclick=\"if(!this.attributes.disabled){2}&&{2}();return false;\">{1}</span>", IconCss, Text, string.IsNullOrEmpty(RequestScript) ? "null" : RequestScript));
            }
            else
            {
                if (EnableCallServer)
                {
                    writer.Write(string.Format("<span id=\"" + ClientID + "\" class=\"{0}\" onclick=\"AjaxControlRequest({5}id:'{2}',request:{3},callback:{4}{6});return false;\">{1}</span>", IconCss, Text, ClientID, string.IsNullOrEmpty(RequestScript) ? "null" : RequestScript, string.IsNullOrEmpty(CallbackScript) ? "null" : CallbackScript, "{", "}"));
                }
                else
                {
                    writer.Write(string.Format("<span id=\"" + ClientID + "\" class=\"{0}\"  onclick=\"{2}&&{2}();return false;\">{1}</span>", IconCss, Text, string.IsNullOrEmpty(RequestScript) ? "null" : RequestScript));
                }
            }
            writer.Write("</a>\n");
        }
    }

    [ToolboxData("<{0}:ToolBarButtonHorizontal runat=\"server\"/>")]
    public class ToolBarButtonHorizontal : BaseToolBarButtonHorizontal<Dictionary<string, string>>
    {
    }
}
