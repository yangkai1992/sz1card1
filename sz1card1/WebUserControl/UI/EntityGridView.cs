#region Using directives
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

[assembly: WebResource("WebUserControl.UI.Resources.excel.gif", "image/gif")]
namespace WebUserControl.UI
{
    /// <summary>
    /// Inherits all features of instinct GridView Control, also provides multi-sort, control of page size, export data to excel, etc.
    /// </summary>
    /// <remarks>
    /// Multi-sort technic is used from here: http://aspalliance.com/666
    /// Excel export technic is used from here: http://www.codeproject.com/csharp/export2excel.asp
    /// </remarks>
    public class EntityGridView : GridView
    {
        #region class member variables
        private System.Drawing.Color rowMouseOverColor;
        private PagerButtons _pagerButtons;
        private TextBox txtbox = new TextBox();
        #endregion


        #region Properties
     
        /// <summary>
        /// 分页控件模式
        /// </summary>
        [DefaultValue(PagerButtons.NextPreviousFirstLast)]
        public PagerButtons PagerButtons
        {
            get { return _pagerButtons; }
            set { _pagerButtons = value; }
        }

        /// <summary>
        /// 总共多少条记录
        /// </summary>
        public int RecordCount
        {
            get
            {
                return VirtualItemCount;
            }
        }

        /// <summary>
        /// 页面索引
        /// </summary>
        public override int PageIndex
        {
            get
            {
                object o = ViewState["PageIndex"];
                return o == null ? 0 : Convert.ToInt32(o);
            }
            set
            {
                ViewState["PageIndex"] = value;
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public override int PageCount
        {
            get
            {
                int pageCount = RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1;
                return pageCount;
            }
        }

        [
        Description("Gets / Sets The BackgroundColor On Mouse Hovered"),
        Category("Misc")
        ]
        public System.Drawing.Color RowMouseOverColor
        {
            get
            {
                return rowMouseOverColor;
            }
            set
            {
                rowMouseOverColor = value;
            }
        }
        #endregion

        #region Life Cycle

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PagerSettings.FirstPageText = "首页";
            PagerSettings.PreviousPageText = "前一页";
            PagerSettings.NextPageText = "下一页";
            PagerSettings.LastPageText = "尾页";
            PagerSettings.Mode = PagerButtons;
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);
            if (RowMouseOverColor != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", string.Format("this.style.background='{0}';", System.Drawing.ColorTranslator.ToHtml(RowMouseOverColor)));
                    e.Row.Attributes.Add("onmouseout", "this.style.background='#ffffff';");
                }
            }
        }

        /// <summary>
        /// Occurs when a row is created in a GridView control. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "text-align:center;height:40px;");

            }
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                e.Row.Controls.Clear();
                TableCell tc = new DataPager(PagerSettings, PageIndex, RecordCount, PageSize);
                tc.ColumnSpan = this.Columns.Count;
                e.Row.Controls.Add(tc);
            }
        }

        #endregion        
    }
    
}
