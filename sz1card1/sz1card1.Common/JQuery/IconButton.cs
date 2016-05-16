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
    public class BaseIconButton<T> : BaseAjaxControl<T> where T : class
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
                writer.Write(string.Format("<button id=\"{0}\" onclick=\"AjaxControlRequest({3}id:'{0}',request:{1},callback:{2}{4});return false;\"{5}>\n", ClientID, string.IsNullOrEmpty(RequestScript) ? "null" : RequestScript, string.IsNullOrEmpty(CallbackScript) ? "null" : CallbackScript, "{", "}", Enabled ? "" : " disabled=\"disabled\""));
            }
            else
            {
                writer.Write(string.Format("<button id=\"{0}\" onclick=\"{1}&&{1}();return false;\"{2}>\n",ClientID, string.IsNullOrEmpty(RequestScript) ? "null" : RequestScript, Enabled ? "" : " disabled=\"disabled\""));
            }
            writer.Write(string.Format("<span class=\"{0}\">{1}</span>", IconCss, Text));
            writer.Write("</button>");
        }
    }

    [ToolboxData("<{0}:IconButton runat=\"server\"/>")]
    public class IconButton : BaseIconButton<Dictionary<string, string>>
    {
    }
}
