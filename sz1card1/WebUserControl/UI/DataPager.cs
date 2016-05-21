using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using sz1card1.Common.Enum;

namespace sz1card1.Common.UI
{
    [Serializable]
    public class PageArgs : EventArgs
    {
        public PageArgs()
        {
        }

        private int pageIndex;
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }
    }

    [DefaultProperty("PageSize")]
    [ToolboxData("<{0}:DataPager runat=\"server\" />")]
    public class DataPager : WebControl, IPostBackEventHandler
    {
        private string pagedControlID = string.Empty;
        private PageMode pageMode = PageMode.QueryString;
        private PageStyle pageStyle = PageStyle.Classic;
        private string queryString = "page";
        public event EventHandler<PageArgs> PageChanged;

        private int totalCount = 0;
        private int pageIndex = 0;
        private int pageSize = 10;
        private bool isDataBind = false;

        public DataPager()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!DesignMode)
            {
                if (HttpContext.Current.Request.QueryString[QueryString] != null)
                {
                    PageIndex = int.Parse(HttpContext.Current.Request.QueryString[QueryString]) - 1;
                }
            }
        }

        [
        Browsable(true),
        Description("设置/获取此DataPager应分页的控件ID"),
        Category("Misc")
        ]
        public string PagedControlID
        {
            get
            {
                return pagedControlID;
            }
            set
            {
                pagedControlID = value;
            }
        }

        [
        Browsable(true),
        Description("设置/获取分页控件模式"),
        DefaultValue(PageMode.QueryString),
        Category("Misc")
        ]
        public PageMode PageMode
        {
            get
            {
                return pageMode;
            }
            set
            {
                pageMode = value;
            }
        }

        [
        Browsable(true),
        Description("设置/获取分页控件样式"),
        DefaultValue(PageStyle.Classic),
        Category("Misc")
        ]
        public PageStyle PageStyle
        {
            get
            {
                return pageStyle;
            }
            set
            {
                pageStyle = value;
            }
        }

        [
        Browsable(true),
        Description("设置/获取查询字符串名"),
        DefaultValue("page"),
        Category("Misc")
        ]
        public string QueryString
        {
            get
            {
                return queryString;
            }
            set
            {
                queryString = value;
            }
        }

        [
        Browsable(true),
        Description("设置/获取总记录条数"),
        Category("Misc"),
        DefaultValue("0")
        ]
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
            set
            {
                totalCount = value;
            }
        }

        [
        Browsable(true),
        Description("设置/获取当前页码"),
        Category("Misc"),
        DefaultValue("0")
        ]
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        [
        Browsable(true),
        Description("设置/获取每页条数"),
        Category("Misc"),
        DefaultValue("10")
        ]
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

        public virtual void OnPageChanged(Object sender, PageArgs e)
        {
            if (PageChanged != null)
                PageChanged(sender, e);
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form[this.UniqueID]))
            {
                PageIndex = Int32.Parse(HttpContext.Current.Request.Form[this.UniqueID].ToString()) - 1;
                DataBind();
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                DataBind();
            }
        }

        public override void DataBind()
        {
            base.DataBind();
            if (!isDataBind)
            {
                PageArgs args = new PageArgs();
                args.PageIndex = pageIndex;
                OnPageChanged(this, args);
                isDataBind = true;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (pageMode == PageMode.Postback)
            {
                string dopost = "<div>\n";
                dopost += "   <input type=\"hidden\" name=\"" + this.UniqueID + "\" id=\"__EVENTTARGET\" value=\"\" />\n";
                dopost += "</div>\n";
                dopost += "<script type=text/javascript>\n";
                dopost += "      function _doPost(page){\n";
                dopost += "          document.forms['" + this.Page.Form.UniqueID + "']." + this.UniqueID + ".value = page;\n";
                dopost += "          document.forms['" + this.Page.Form.UniqueID + "'].submit();\n";
                dopost += "      }\n";
                dopost += "</script>\n";
                if (!Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "_doPost"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "_doPost", dopost);
                }
            }
        }

        private string GetLinkScript()
        {
            string result = "";
            switch (pageMode)
            {
                case PageMode.QueryString:
                    result = "{0}={1}";
                    break;
                case PageMode.Postback:
                    result = "javascript:_doPost('{1}');";
                    break;
                default:
                    break;
            }
            return result;
        }

        private string GetSelectScript()
        {
            string result = "";
            switch (pageMode)
            {
                case PageMode.QueryString:
                    result = "window.location.href='{0}'+this.value;";
                    break;
                case PageMode.Postback:
                    result = "_doPost(this.value);";
                    break;
                default:
                    break;
            }
            return result;
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {

            string url = DesignMode ? "a.aspx" : Page.Request.RawUrl;
            if (url.IndexOf("?") > 0)
            {
                int index = url.IndexOf(QueryString + "=");
                if (index > 0)
                {
                    url = url.Substring(0, index);
                    url += QueryString;
                }
                else
                {
                    url += "&" + QueryString;
                }
            }
            else
            {
                url += "?" + QueryString;
            }
            int lastIndex = (TotalCount % PageSize == 0 ? TotalCount / PageSize - 1 : TotalCount / PageSize);
            //呈现第一页
            if (PageIndex == 0)
            {
                switch (PageStyle)
                {
                    case PageStyle.Classic:
                        writer.Write("<span class='disabled'> < </span>");
                        break;
                    case PageStyle.Standard:
                        writer.Write("<span class='disabled'> 首页 </span>");
                        writer.Write("<span class='disabled'>上一页</span>");
                        break;
                }
            }
            else
            {
                switch (PageStyle)
                {
                    case PageStyle.Classic:
                        writer.Write("<a href=\"" + string.Format(GetLinkScript(), url, PageIndex) + "\"> < </a>");
                        break;
                    case PageStyle.Standard:
                        writer.Write("<a href=\"" + string.Format(GetLinkScript(), url, 1) + "\"> 首页 </a>");
                        writer.Write("<a href=\"" + string.Format(GetLinkScript(), url, PageIndex) + "\">上一页</a>");
                        break;
                }
            }
            if (PageStyle == PageStyle.Classic)
            {
                //呈现数字页
                int start;
                if (PageIndex < 5)
                {
                    start = 0;
                }
                else if (PageIndex > TotalCount / PageSize - 5)
                {
                    start = TotalCount / PageSize - 8 >= 0 ? TotalCount / PageSize - 8 : 0;
                }
                else
                {
                    start = PageIndex - 4;
                }
                for (int i = start; i < start + 9 && i <= lastIndex; i++)
                {
                    if (i == PageIndex)
                    {
                        writer.Write(string.Format("<span class='current'>{0}</span>", i + 1));
                    }
                    else
                    {
                        writer.Write("<a href=\"" + string.Format(GetLinkScript(), url, i + 1) + "\"> " + (i + 1).ToString() + " </a>");
                    }
                }
            }
            //呈现最后一页
            if (PageIndex == lastIndex)
            {
                switch (PageStyle)
                {
                    case PageStyle.Classic:
                        writer.Write("<span class='disabled'> > </span>");
                        break;
                    case PageStyle.Standard:
                        writer.Write("<span class='disabled'>下一页</span>");
                        writer.Write("<span class='disabled'> 末页 </span>");
                        break;
                }

            }
            else
            {
                switch (PageStyle)
                {
                    case PageStyle.Classic:
                        writer.Write("<a href=\"" + string.Format(GetLinkScript(), url, PageIndex + 2) + "\"> > </a>");
                        break;
                    case PageStyle.Standard:
                        writer.Write("<a href=\"" + string.Format(GetLinkScript(), url, PageIndex + 2) + "\">下一页</a>");
                        writer.Write("<a href=\"" + string.Format(GetLinkScript(), url, lastIndex + 1) + "\"> 末页 </a>");
                        break;
                }
            }
            if (PageStyle == PageStyle.Standard)
            {
                string onchange = string.Format(GetSelectScript(), url);
                writer.Write(string.Format("&nbsp;页数：<select name='{0}_select' onchange={1}>", UniqueID, onchange));
                for (int i = 0; i <= lastIndex; i++)
                {
                    if (i == PageIndex)
                    {
                        writer.Write(string.Format("<option value='{0}' selected='true'>{0}/{1}</option>", i + 1, lastIndex + 1));
                    }
                    else
                    {
                        writer.Write(string.Format("<option value='{0}'>{0}/{1}</option>", i + 1, lastIndex + 1));
                    }
                }
                writer.Write("</select>");
            }
        }
    }
}
