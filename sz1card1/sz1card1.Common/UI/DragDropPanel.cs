using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Drawing;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

[assembly: WebResource("sz1card1.Common.UI.Resources.close.gif", "image/gif")]
namespace sz1card1.Common.UI
{
    [
        ToolboxData("<{0}:DragDropPanel runat=\"server\"></{0}:DragDropPanel>"),
        Designer(typeof(SubContainerControlDesigner)),
        ParseChildren(false),
        PersistChildren(true)
    ]
    public class DragDropPanel : WebControl
    {
        private string title = string.Empty;
        private Unit width = new Unit(100, UnitType.Percentage);
        private Unit height = new Unit(320, UnitType.Pixel);
        private Color headerBgColor = Color.FromArgb(0xd3, 0xea, 0xef);
        private string headerOnClientClick = string.Empty;

        /// <summary>
        /// 面板标题
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取面板标题"),
        Category("Misc")
        ]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        /// <summary>
        /// 面板高度
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取面板高度"),
        Category("Misc")
        ]
        public override Unit Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public override Unit Width
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// 标题背景颜色
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取标题背景颜色"),
        Category("Misc")
        ]
        public Color HeaderBgColor
        {
            get
            {
                return headerBgColor;
            }
            set
            {
                headerBgColor = value;
            }
        }

        /// <summary>
        /// 点击标题客户端脚本
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取点击标题客户端脚本"),
        Category("Misc")
        ]
        public string HeaderOnClientClick
        {
            get
            {
                return headerOnClientClick;
            }
            set
            {
                headerOnClientClick = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.MarginTop, "3px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "#ccc");
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, width.ToString());
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "white");
            base.AddAttributesToRender(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(string.Format("<div id=\"{0}_h\" style=\"height:20px;border-right: #eaf0f6 1px solid; padding-right: 2px; padding-left: 2px;padding-bottom: 2px; margin: 0px; cursor: pointer; padding-top: 2px; border-bottom: #ccc 1px solid;background-color: {1}\">", ID, ColorTranslator.ToHtml(headerBgColor)));
            writer.Write("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%;height:20px\">");
            writer.Write(string.Format("<tr><td style=\"text-align:left;font-weight:bold;padding-left:3px;padding-top:3px\">{0}</td><td style=\"text-align:right;padding-right:3px\"><img src=\"{1}\" onclick=\"_IG_closePanel('{2}');\" alt=\"关闭\" style=\"cursor:pointer\"/></td></tr>", headerOnClientClick == string.Empty ? Title : "<a href=\"#\" onclick=\"" + headerOnClientClick + "\" style=\"text-decoration:underline\">" + Title + "</a>", Page.ClientScript.GetWebResourceUrl(this.GetType(), "sz1card1.Common.UI.Resources.close.gif"), ID));
            writer.Write("</table>");
            writer.Write("</div>");
            writer.Write("<div style=\"overflow:hidden\">");
            base.RenderContents(writer);
            writer.Write("</div>");
        }
    }
}
