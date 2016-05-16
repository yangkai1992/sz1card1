using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

[assembly: WebResource("sz1card1.Common.JQuery.Resources.loadmask.js", "text/javascript")]
[assembly: WebResource("sz1card1.Common.JQuery.Resources.json.js", "text/javascript")]
namespace sz1card1.Common.JQuery
{
    [ToolboxData("<{0}:SmartGrid runat=\"server\"></{0}:SmartGrid>")]
    public class SmartGrid : DataBoundControl, ICallbackEventHandler, INamingContainer
    {
        private bool useColor = true;
        private bool useHover = true;
        private bool useClick = true;
        private bool autoLoad = true;
        private bool useCheckBox = true;
        private bool useJumpPage = true;
        private int pageIndex = 0;
        private int pageSize = 10;
        private int totalCount = 0;
        private StringBuilder sbContentHtml = new StringBuilder();
        private Dictionary<string, string> parameters;
        private DataControlFieldCollection columns;
        private IEnumerable source;

        public event GridViewRowEventHandler RowDataBound;

        [Browsable(true)]
        [DefaultValue(false)]
        [Description("为true表示表格采用排序模式（表头外观会有一些变化）")]
        public bool SortMode
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [Description("为true表示表格采用表单布局模式（间距和对齐会有些变化，并取消变色）")]
        public bool FormMode
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [Description("也是一种表单布局模式，间距和对齐与formMode相同，不同的是表格无边框和背景色")]
        public bool TransMode
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Description("为false表示每行不会交替变色")]
        public bool UseColor
        {
            get
            {
                return useColor;
            }
            set
            {
                useColor = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Description("为false表示不会产生鼠标移入行变色效果")]
        public bool UseHover
        {
            get
            {
                return useHover;
            }
            set
            {
                useHover = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Description("为false表示不会产生点击单行变色效果")]
        public bool UseClick
        {
            get
            {
                return useClick;
            }
            set
            {
                useClick = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [Description("为true表示可以多行点击变色")]
        public bool UseMultColor
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [Description("为true表示采用radio单行点击变色模式。当表格第一列中有radio时默认为true")]
        public bool UseRadio
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Description("为true表示采用checkbox多行点击变色模式。当表格第一列中有checkbox时默认为true")]
        public bool UseCheckBox
        {
            get
            {
                return useCheckBox;
            }
            set
            {
                useCheckBox = value;
            }
        }


        [Browsable(true)]
        [DefaultValue(true)]
        [Description("为true表示采用直接跳转模式。默认显示直接跳转样式")]
        public bool UseJumpPage
        {
            get
            {
                return useJumpPage;
            }
            set
            {
                useJumpPage = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Description("是否启用分页")]
        public bool AllowPaging
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Description("是否自动加载数据")]
        public bool AutoLoad
        {
            get
            {
                return autoLoad;
            }
            set
            {
                autoLoad = value;
            }
        }

        [Browsable(false)]
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
        }

        [Browsable(true)]
        [DefaultValue(10)]
        [Description("每页条数")]
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        [Browsable(false)]
        public int StartIndex
        {
            get
            {
                return pageIndex * pageSize;
            }
        }


        [Browsable(false)]
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
        }

        [Browsable(true)]
        [Description("筛选条件表达式")]
        public string WhereExpression
        {
            get;
            set;
        }

        [Browsable(true)]
        [Description("排序表达式")]
        public string SortExpression
        {
            get;
            set;
        }

        [Browsable(true)]
        [Description("客户端分页加载完成后触发事件")]
        public string OnClientPaged
        {
            get;
            set;
        }

        public Dictionary<string, string> Parameters
        {
            get
            {
                return parameters;
            }
        }

        [MergableProperty(false)]
        [Browsable(false)]
        [Editor("System.Web.UI.Design.WebControls.DataControlFieldTypeEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public DataControlFieldCollection Columns
        {
            get
            {
                if (columns == null)
                    columns = new DataControlFieldCollection();
                return columns;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (base.IsBoundUsingDataSourceID)
            {
                ObjectDataSource dataSource = this.DataSourceObject as ObjectDataSource;
                if (dataSource != null)
                {
                    dataSource.Selected += delegate(object sender, ObjectDataSourceStatusEventArgs arg)
                    {
                        if (arg.OutputParameters["total"] != null)
                        {
                            totalCount = Convert.ToInt32(arg.OutputParameters["total"]);
                        }
                    };
                }
            }
        }

        public virtual void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (RowDataBound != null)
            {
                RowDataBound(sender, e);
            }
        }

        public override void DataBind()
        {
            base.DataBind();
            sbContentHtml.AppendLine("<div id=\"" + ClientID + "_Div\" style=\"width:100%;overflow:auto;\">");
            sbContentHtml.Append("<table id=\"" + ClientID + "_Table\" class=\"tableStyle\" sortMode=\"true\"" + (FormMode ? " formMode=\"true\"" : "") + (TransMode ? " transMode=\"true\"" : "") + (UseColor ? "" : " useColor=\"false\"") + (UseHover ? "" : " useHover=\"false\"") + (UseClick ? "" : " useClick=\"false\"") + (UseRadio ? " useRadio=\"true\"" : "") + (UseMultColor ? " useMultColor=\"true\"" : "") + (UseCheckBox ? " " : " useCheckBox=\"false\"") + ">\n");
            sbContentHtml.Append("\t<thead>\n");
            sbContentHtml.Append("\t\t<tr>\n");
            string sortColumn = "";
            bool asc = true;
            string[] arr = SortExpression.Split(' ');
            if (arr.Length == 2)
            {
                sortColumn = arr[0];
                asc = arr[1].ToLower() != "desc";
            }
            if (Columns != null)
            {
                foreach (DataControlField column in Columns)
                {
                    if (column is BoundField)
                    {
                        BoundField field = column as BoundField;
                        if (field.SortExpression == sortColumn)
                        {
                            sbContentHtml.Append(string.Format("\t\t\t<th{1}{2}>{0}</th>\n", string.IsNullOrEmpty(field.SortExpression) ? field.HeaderText : "<span id=\"" + ClientID + "_Header_" + field.DataField + "\" class=\"" + (asc ? "sort_up" : "sort_down") + "\" onclick=\"" + ClientID + ".sort('" + field.DataField + "','" + field.SortExpression + "');\">" + field.HeaderText + "</span>", field.ItemStyle.Width.Value == 0 ? "" : " width=\"" + field.ItemStyle.Width.Value.ToString() + "\"", field.Visible ? "" : " style=\"display:none\""));
                        }
                        else
                        {
                            sbContentHtml.Append(string.Format("\t\t\t<th{1}{2}>{0}</th>\n", string.IsNullOrEmpty(field.SortExpression) ? field.HeaderText : "<span id=\"" + ClientID + "_Header_" + field.DataField + "\" onclick=\"" + ClientID + ".sort('" + field.DataField + "','" + field.SortExpression + "');\">" + field.HeaderText + "</span>", field.ItemStyle.Width.Value == 0 ? "" : " width=\"" + field.ItemStyle.Width.Value.ToString() + "\"", field.Visible ? "" : " style=\" display:none\""));
                        }
                    }
                    else if (column is TemplateField)
                    {
                        TemplateField field = column as TemplateField;
                        if (field.SortExpression == sortColumn)
                        {
                            sbContentHtml.Append(string.Format("\t\t\t<th{1}{2}>{0}</th>\n", string.IsNullOrEmpty(field.SortExpression) ? field.HeaderText : "<span id=\"" + ClientID + "_Header_" + field.SortExpression + "\" class=\"" + (asc ? "sort_up" : "sort_down") + "\" onclick=\"" + ClientID + ".sort('" + field.SortExpression + "','" + field.SortExpression + "');\">" + field.HeaderText + "</span>", field.ItemStyle.Width.Value == 0 ? "" : " width=\"" + field.ItemStyle.Width.Value.ToString() + "\"", field.Visible ? "" : " style=\"display:none\""));
                        }
                        else
                        {
                            sbContentHtml.Append(string.Format("\t\t\t<th{1}{2}>{0}</th>\n", string.IsNullOrEmpty(field.SortExpression) ? field.HeaderText : "<span id=\"" + ClientID + "_Header_" + field.SortExpression + "\" onclick=\"" + ClientID + ".sort('" + field.SortExpression + "','" + field.SortExpression + "');\">" + field.HeaderText + "</span>", field.ItemStyle.Width.Value == 0 ? "" : " width=\"" + field.ItemStyle.Width.Value.ToString() + "\"", field.Visible ? "" : " style=\"display:none\""));
                        }
                    }
                }
            }
            sbContentHtml.Append("\t\t</tr>\n");
            sbContentHtml.Append("\t</thead>\n");
            sbContentHtml.Append("\t<tbody>\n");
            if (source != null)
            {
                foreach (object item in source)
                {
                    int i = 0;
                    GridViewRow tr = new GridViewRow(i, i, DataControlRowType.DataRow, DataControlRowState.Normal);
                    Controls.Add(tr);
                    tr.DataItem = item;
                    foreach (DataControlField column in Columns)
                    {
                        if (column is BoundField)
                        {
                            BoundField field = column as BoundField;
                            TableCell cell = new TableCell();
                            if (field.DataField != "")
                            {
                                cell.Text = DataBinder.GetPropertyValue(item, field.DataField, field.DataFormatString);
                            }
                            if (!field.Visible)
                            {
                                cell.Style.Add(HtmlTextWriterStyle.Display, "none");
                            }
                            switch (field.ItemStyle.HorizontalAlign)
                            {
                                case HorizontalAlign.Left:
                                    cell.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
                                    break;
                                case HorizontalAlign.Center:
                                    cell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                                    break;
                                case HorizontalAlign.Right:
                                    cell.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
                                    break;
                                default:
                                    break;
                            }
                            tr.Cells.Add(cell);
                        }
                        else if (column is TemplateField)
                        {
                            TemplateField field = column as TemplateField;
                            TemplateTd td = new TemplateTd();
                            field.ItemTemplate.InstantiateIn(td);
                            td.dataItem = item;
                            td.dataItemIndex = i;
                            td.displayIndex = i;
                            if (!field.Visible)
                            {
                                td.Style.Add(HtmlTextWriterStyle.Display, "none");
                            }
                            switch (field.ItemStyle.HorizontalAlign)
                            {
                                case HorizontalAlign.Left:
                                    td.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
                                    break;
                                case HorizontalAlign.Center:
                                    td.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                                    break;
                                case HorizontalAlign.Right:
                                    td.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
                                    break;
                                default:
                                    break;
                            }
                            tr.Cells.Add(td);
                            td.DataBind();
                        }
                    }
                    GridViewRowEventArgs args = new GridViewRowEventArgs(tr);
                    OnRowDataBound(this, args);
                    using (StringWriter sw = new StringWriter())
                    {
                        HtmlTextWriter htw = new HtmlTextWriter(sw);
                        tr.RenderControl(htw);
                        sbContentHtml.AppendLine(sw.ToString());
                    }
                    i++;
                }
            }
            sbContentHtml.Append("\t</tbody>\n");
            sbContentHtml.Append("</table>\n");
            sbContentHtml.Append("</div>\n");
            if (AllowPaging)
            {
                sbContentHtml.Append("\t<div class=\"pager\" style=\"text-align:right;\">\n");
                if (pageIndex == 0)
                {
                    sbContentHtml.Append("\t\t<span class=\"icon_page_first\" style=\"color:#ccc\">首页</span>\n");
                    sbContentHtml.Append("\t<span class=\"icon_page_prev\" style=\"color:#ccc\">上一页</span>\n");
                }
                else
                {
                    sbContentHtml.Append("\t\t<span class=\"icon_page_first hand\" onclick=\"" + ClientID + ".startPage(0);\">首页</span>\n");
                    sbContentHtml.Append("\t\t<span class=\"icon_page_prev hand\" onclick=\"" + ClientID + ".startPage(" + (pageIndex - 1).ToString() + ");\">上一页</span>\n");
                }
                int lastIndex = (TotalCount % PageSize == 0 ? TotalCount / PageSize - 1 : TotalCount / PageSize);
                int totalPage = (TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1);
                totalPage = totalPage == 0 ? 1 : totalPage;
                if (useJumpPage)
                {
                    sbContentHtml.Append(string.Format("\t\t<span  style=\"float:left;\" >(当前为第</span><input id=\"{2}\" onkeyup=\"value=this.value.replace(/\\D+/g,'')\" style=\"width:25px;height:13px;float:left\" type=\"text\" onkeypress=\"if(event.keyCode==13) {{" + ClientID + ".startPage(this.value.replace(/\\D+/g,'')-1);return false;}}\" value=\"{0}\" /><span  style=\"float:left;\" >页\t共{1}页)</span>\n", (pageIndex + 1).ToString(), totalPage.ToString(), ClientID + ".AutoID"));
                }
                if (pageIndex == lastIndex || lastIndex == -1)
                {
                    sbContentHtml.Append("\t\t<span class=\"icon_page_next\" style=\"color:#ccc\">下一页</span>\n");
                    sbContentHtml.Append("\t\t<span class=\"icon_page_last\" style=\"color:#ccc\">末页</span>\n");
                }
                else
                {
                    sbContentHtml.Append("\t\t<span class=\"icon_page_next hand\" onclick=\"" + ClientID + ".startPage(" + (pageIndex + 1).ToString() + ");\">下一页</span>\n");
                    sbContentHtml.Append("\t\t<span class=\"icon_page_last hand\" onclick=\"" + ClientID + ".startPage(" + lastIndex.ToString() + ");\">末页</span>\n");
                }
                sbContentHtml.Append(string.Format("\t\t共{0}条记录\n", TotalCount));
                sbContentHtml.Append("\t</div>\n");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsCallback)
            {
                if (!Page.ClientScript.IsClientScriptIncludeRegistered("json"))
                {
                    Page.ClientScript.RegisterClientScriptInclude("json", Page.ClientScript.GetWebResourceUrl(this.GetType(), "sz1card1.Common.JQuery.Resources.json.js"));
                }
                if (!Page.ClientScript.IsClientScriptIncludeRegistered("loadmask"))
                {
                    Page.ClientScript.RegisterClientScriptInclude("loadmask", Page.ClientScript.GetWebResourceUrl(this.GetType(), "sz1card1.Common.JQuery.Resources.loadmask.js"));
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("var {0}={1}", ClientID, "{"));
                sb.AppendLine(string.Format("\tid:'{0}',", ClientID));
                sb.AppendLine(string.Format("\tisLoad:{0},", AutoLoad.ToString().ToLower()));
                sb.AppendLine(string.Format("\twhereExpression:\"{0}\",", WhereExpression));
                sb.AppendLine(string.Format("\tsortExpression:'{0}',", SortExpression));
                sb.AppendLine("\tparameters:{},");
                sb.AppendLine(string.Format("\tpageIndex:{0},", PageIndex));
                sb.AppendLine(string.Format("\tstartPage:function(index){0}", "{"));
                sb.AppendLine("\t\tthis.pageIndex=index;");
                sb.AppendLine("\t\tthis.reload();");
                sb.AppendLine("\t},");
                sb.AppendLine(string.Format("\tload:function(){0}", "{"));
                sb.AppendLine(string.Format("\t\tif(!this.isLoad){0}", "{"));
                sb.AppendLine("\t\t\tthis.pageIndex=0;");
                sb.AppendLine(string.Format("\t\t\tthis.whereExpression=\"{0}\",", WhereExpression));
                sb.AppendLine(string.Format("\t\t\tthis.sortExpression='{0}',", SortExpression));
                sb.AppendLine("\t\t\tthis.reload();");
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t},");
                sb.AppendLine(string.Format("\tsort:function(data,order){0}", "{"));
                sb.AppendLine(string.Format("\t\tvar span=document.getElementById('{0}_Header_'+data);", ClientID));
                sb.AppendLine("\t\tif(span.className.indexOf('sort_up')>-1){this.sortExpression=order+' DESC';}else{this.sortExpression=order+' ASC';}");
                sb.AppendLine("\t\tthis.pageIndex=0;");
                sb.AppendLine("\t\tthis.reload();");
                sb.AppendLine("\t},");
                sb.AppendLine(string.Format("\tcallback:function(result,obj){0}", "{"));
                sb.AppendLine(string.Format("\t\t$('#{0}').unmask();", ClientID));
                sb.AppendLine(string.Format("\t\tdocument.getElementById('{0}').innerHTML=result;", ClientID));
                sb.AppendLine(string.Format("\t\tobj.isLoad=true;"));
                sb.AppendLine(string.Format("\t\t$('document').ready(function(){1}document.getElementById('{0}_Div').style.height=(document.getElementById('{0}').offsetHeight-35)+'px';{2});", ClientID, "{", "}"));
                sb.AppendLine(string.Format("\t\ttableRefresh('{0}_Table');", ClientID));
                sb.AppendLine(string.Format("\t\tvar obj=$('#scrollContent')[0]!=null?$('#scrollContent')[0]:$('#{0}')[0];", ClientID));
                sb.AppendLine(string.Format("\t\t$('#{0}_Div').height(obj.offsetHeight-28);", ClientID));
                sb.AppendLine("\t\tenableTooltips();");
                if (!string.IsNullOrEmpty(OnClientPaged))
                {
                    sb.AppendLine("\t\tif(" + OnClientPaged + "){");
                    sb.AppendLine(string.Format("\t\t\tobj.pageSize = {0};", PageSize));
                    sb.AppendLine("\t\t\t" + OnClientPaged + "(obj);");
                    sb.AppendLine("\t\t}");
                }
                sb.AppendLine("\t},");
                sb.AppendLine(string.Format("\treload:function(){0}", "{"));
                sb.AppendLine(string.Format("\t\t$('#{0}').mask('正在加载数据...');", ClientID));
                sb.AppendLine(string.Format("\t\tvar args=this.whereExpression+'$'+this.sortExpression+'$'+this.pageIndex+'$'+sz1card1.stringify(this.parameters);"));
                sb.AppendLine("\t\t" + Page.ClientScript.GetCallbackEventReference(this, "args", "this.callback", "this", "null", true) + ";");
                sb.AppendLine("\t}");
                sb.AppendLine("}");
                sb.AppendLine(string.Format("$('document').ready(function(){1}var obj=$('#scrollContent')[0]!=null?$('#scrollContent')[0]:$('#{0}')[0];$('#{0}_Div').height(obj.offsetHeight-{3});{2});", ClientID, "{", "}", AllowPaging ? "28" : "0"));
                if (!Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), ClientID))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), ClientID, sb.ToString(), true);
                }
                if (!Page.ClientScript.IsClientScriptBlockRegistered("ajaxfix"))
                {
                    string fixScript = @"function WebForm_CallbackComplete_SyncFixed() {
                                           for (var i = 0; i < __pendingCallbacks.length; i++) {
                                                callbackObject = __pendingCallbacks[ i ];
                                                if (callbackObject && callbackObject.xmlRequest && (callbackObject.xmlRequest.readyState == 4)) {
                                                     if (!__pendingCallbacks[ i ].async) {
                                                         __synchronousCallBackIndex = -1;
                                                     }
                                             __pendingCallbacks[i] = null;

                                               var callbackFrameID = '__CALLBACKFRAME' + i;
                                               var xmlRequestFrame = document.getElementById(callbackFrameID);
                                               if (xmlRequestFrame) {
                                                 xmlRequestFrame.parentNode.removeChild(xmlRequestFrame);
                                               }

                                               WebForm_ExecuteCallback(callbackObject);
                                              }
                                            }
                                          }";
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ajaxfix", fixScript, true);

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ajaxfixup", "if (typeof (WebForm_CallbackComplete) == 'function') {WebForm_CallbackComplete = WebForm_CallbackComplete_SyncFixed;}", true);
                }

            }
        }

        protected override void PerformDataBinding(IEnumerable data)
        {
            base.PerformDataBinding(data);
            source = data;
        }

        protected override void PerformSelect()
        {
            if ((Page.IsCallback || autoLoad))
            {
                base.PerformSelect();
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
            if (Height.IsEmpty)
            {
                this.Style.Add(HtmlTextWriterStyle.Height, "100%");
            }
            Attributes.Remove("title");
            base.AddAttributesToRender(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(sbContentHtml);
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            string[] array = eventArgument.Split('$');
            WhereExpression = array[0];
            SortExpression = array[1];
            pageIndex = Convert.ToInt32(array[2]);
            parameters = array[3].FromJson<Dictionary<string, string>>();
            DataBind();
        }

        public string GetCallbackResult()
        {
            return sbContentHtml.ToString();
        }
    }

    public class TemplateTd : TableCell, IDataItemContainer
    {
        internal object dataItem;
        internal int dataItemIndex;
        internal int displayIndex;

        public object DataItem
        {
            get
            {
                return dataItem;
            }
        }

        public int DataItemIndex
        {
            get
            {
                return dataItemIndex;
            }
        }

        public int DisplayIndex
        {
            get
            {
                return displayIndex;
            }
        }
    }
}
