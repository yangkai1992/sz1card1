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
    [ToolboxData("<{0}:ToolBarHorizontal runat=\"server\"></{0}:ToolBarHorizontal>")]
    [ParseChildren(false)]
    public class ToolBarHorizontal:WebControl
    {
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.Write("<div class=\"box_tool_min padding_top2 padding_bottom2 padding_right5\">\n");
            writer.Write("\t<div class=\"center\">\n");
            writer.Write("\t\t<div class=\"left\">\n");
            writer.Write("\t\t\t<div class=\"right\">\n");
            writer.Write("\t\t\t\t<div class=\"padding_top5 padding_left10\">\n");
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.Write("\t\t\t\t</div>\n");
            writer.Write("\t\t\t</div>\n");
            writer.Write("\t\t</div>\n");
            writer.Write("\t</div>\n");
            writer.Write("\t<div class=\"clear\"></div>\n");
            writer.Write("</div>\n");
        }
    }
}
