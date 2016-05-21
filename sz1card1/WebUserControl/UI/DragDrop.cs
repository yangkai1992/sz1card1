using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: WebResource("WebUserControl.UI.Resources.DragDrop.js", "text/javascript")]
namespace WebUserControl.UI
{
    [
        ToolboxData("<{0}:DragDrop runat=\"server\"></{0}:DragDrop>"),
        Designer(typeof(SubContainerControlDesigner)),
        ParseChildren(typeof(DragDropPanel)),
        PersistChildren(true)
    ]
    public class DragDrop : WebControl
    {
        private int repeatColumns = 3;

        /// <summary>
        /// 重复数
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取重复数"),
        Category("Misc"),
        DefaultValue("3")
        ]
        public int RepeatColumns
        {
            get
            {
                return repeatColumns;
            }
            set
            {
                repeatColumns = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Table;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("DragDrop"))
            {
                Page.ClientScript.RegisterClientScriptInclude("DragDrop", Page.ClientScript.GetWebResourceUrl(this.GetType(), "WebUserControl.UI.Resources.DragDrop.js"));
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "InitDragDrop", "window.onload=function(){var _table = document.getElementById('"+ID+"');_IG_initDrag(_table);};",true);
            base.OnPreRender(e);
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
            base.AddAttributesToRender(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write("<tr>");
            for (int i = 0; i < repeatColumns; i++)
            {
                writer.Write(string.Format("<td style=\"padding:3px;width:{0};vertical-align:top\">", new Unit(100 / repeatColumns, UnitType.Percentage)));
                for (int j = i;j< DragDropPanels.Count; j+=repeatColumns)
                {
                    DragDropPanels[j].RenderControl(writer);
                }
                writer.Write("</td>");
            }
            writer.Write("</tr>");
        }

        protected override ControlCollection CreateControlCollection()
        {
            return new DragDropPanelCollection(this);
        }

        protected override void AddParsedSubObject(object obj)
        {
            DragDropPanel panel = obj as DragDropPanel;
            if (panel != null)
            {
                base.Controls.Add(panel);
            }
        }

        [PersistenceMode(PersistenceMode.InnerDefaultProperty), Browsable(false)]
        protected DragDropPanelCollection DragDropPanels
        {
            get
            {
                return (DragDropPanelCollection)this.Controls;
            }
        }
    }
}
