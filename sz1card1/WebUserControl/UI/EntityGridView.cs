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
        private string _defaultSortColumnName = string.Empty;
        private SortDirection _defaultSortDirection = SortDirection.Ascending;
        private int _pageSelectorPageSizeInterval = 10;
        private System.Drawing.Color rowMouseOverColor;
        private string _statisticsText = "";
        private PagerButtons _pagerButtons;
        private TextBox txtbox = new TextBox();
        #endregion

        /// <summary>
        /// PageSizeChanged event raised whenever the Page Size Changes
        /// </summary>
        public event EventHandler PageSizeChanged;

        public event GridViewPageEventHandler PageIndexChang; 

        #region Properties

        /// <summary>
        /// Gets / Sets Page Selector PageSize Interval
        /// </summary>
        [
        Description("Gets / Sets Page Selector PageSize Interval"),
        Category("Misc"),
        DefaultValue("10"),
        ]
        public int PageSelectorPageSizeInterval
        {
            get { return _pageSelectorPageSizeInterval; }
            set { _pageSelectorPageSizeInterval = value; }
        }

        /// <summary>
        /// 获取/设置统计文本
        /// </summary>
        [
        Description("获取/设置统计文本"),
        Category("Misc"),
        DefaultValue(""),
        ]
        public string StatisticsText
        {
            get
            {
                return _statisticsText;
            }
            set
            {
                _statisticsText = value;
            }
        }

        

        /// <summary>
        /// Sets / Gets Default Sort Column Name
        /// </summary>
        [
        Description("Sets / Gets Default Sort Column Name"),
        Category("Behavior"),
        DefaultValue(""),
        ]
        public string DefaultSortColumnName
        {
            get
            {
                return (_defaultSortColumnName == string.Empty) ? GetDefaultSortColumn() : _defaultSortColumnName;
            }
            set { _defaultSortColumnName = value; }
        }

        /// <summary>
        /// Setting the default sort order direction 
        /// </summary>
        [
        Description("Default Sort Order Direction"),
        Category("Misc"),
        Editor("System.Web.UI.WebControls.SortDirection", typeof(System.Web.UI.WebControls.SortDirection)),
        DefaultValue("SortDirection.Ascending"),
        ]
        public SortDirection DefaultSortDirection
        {
            get { return _defaultSortDirection; }
            set { _defaultSortDirection = value; }
        }

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
        /// Enable/Disable MultiColumn Sorting.
        /// </summary>
        [
        Description("Whether Sorting On more than one column is enabled"),
        Category("Behavior"),
        DefaultValue("false"),
        ]
        public bool AllowMultiColumnSorting
        {
            get
            {
                object o = ViewState["EnableMultiColumnSorting"];
                return (o != null ? (bool)o : false);
            }
            set
            {
                AllowSorting = true;
                ViewState["EnableMultiColumnSorting"] = value;
            }
        }

        /// <summary>
        /// 总共多少条记录
        /// </summary>
        public int RecordCount
        {
            get
            {
                object o = ViewState["RecordCount"];
                return o == null ? 0 : Convert.ToInt32(o);
            }
            set
            {
                ViewState["RecordCount"] = value;
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

        /// <summary>
        /// Get or Set Image location to be used to display Ascending Sort order.
        /// </summary>
        [
        Description("Image to display for Ascending Sort"),
        Category("Misc"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        DefaultValue(""),
        ]
        public string SortAscImageUrl
        {
            get
            {
                object o = ViewState["SortImageAsc"];
                return (o != null ? o.ToString() : "");
            }
            set
            {
                ViewState["SortImageAsc"] = value;
            }
        }
        /// <summary>
        /// Get or Set Image location to be used to display Ascending Sort order.
        /// </summary>
        [
        Description("Image to display for Descending Sort"),
        Category("Misc"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        DefaultValue(""),
        ]
        public string SortDescImageUrl
        {
            get
            {
                object o = ViewState["SortImageDesc"];
                return (o != null ? o.ToString() : "");
            }
            set
            {
                ViewState["SortImageDesc"] = value;
            }
        }

        /// <summary>
        /// Total Records Count being assigned the value in the DataSource_Selected event
        /// </summary>
        public int RecordsCount
        {
            get
            {
                if (ViewState["_recordsCount"] != null)
                    return (int)ViewState["_recordsCount"];
                else
                    return 0;
            }
            set
            {
                ViewState["_recordsCount"] = value;
            }
        }
        #endregion

        #region Life Cycle

        /// <summary>
        /// Occurs when the server control is initialized.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!string.IsNullOrEmpty(this.DataSourceID))
            {
                DataSourceControl dsc = (DataSourceControl)this.Parent.FindControl(string.Format("{0}", this.DataSourceID));

                System.Reflection.EventInfo eventInfo = dsc.GetType().GetEvent("Selected");
                System.Delegate d = System.Delegate.CreateDelegate(eventInfo.EventHandlerType, this, "dsc_Selected");

                eventInfo.AddEventHandler(dsc, d);
            }          
        }

        /// <summary>
        /// Retrieves data from the underlying data storage by calling the method that is identified by the 
        /// SelectMethod property with the parameters in the SelectParameters collection.        
        /// </summary>
        /// <remarks>
        /// Occurs when a Select operation has completed.
        /// </remarks>
        void dsc_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecordsCount = e.AffectedRows;
        }

        /// <summary>
        /// Retrieves data from the underlying data storage by calling the method that is identified by the 
        /// SelectMethod property with the parameters in the SelectParameters collection.       
        /// </summary>
        /// <remarks>
        /// Occurs when a Select operation has completed.
        /// </remarks>
        void dsc_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows == -1 && e.ReturnValue is int)
            {
                e.AffectedRows = (int)e.ReturnValue;
            }
            RecordsCount = e.AffectedRows;
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
        /// Occurs when the hyperlink to sort a column is clicked, but before the EntityGridView control handles the sort operation. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            if (AllowMultiColumnSorting)
                e.SortExpression = GetSortExpression(e);

            base.OnSorting(e);
        }

        /// <summary>
        /// Occurs when a row is created in a GridView control. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (SortExpression != String.Empty)
                    DisplaySortOrderImages(SortExpression, e.Row);
            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "text-align:center;height:40px;");

            }
            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                e.Row.Controls.Clear();
                PagerSettings.FirstPageText = "首页";
                PagerSettings.PreviousPageText = "前一页";
                PagerSettings.NextPageText = "下一页";
                PagerSettings.LastPageText = "尾页";
                PagerSettings.Mode = PagerButtons;
                TableCell tc = GeneratePage();
                tc.ColumnSpan = this.Columns.Count;
                e.Row.Controls.Add(tc);
            }
        }

        #endregion

        private TableCell GeneratePage()
        {
            bool generateNumber = false;
            if (PagerSettings.Mode == PagerButtons.Numeric || PagerSettings.Mode == PagerButtons.NumericFirstLast)
            {
                generateNumber = true;
            }
            TableCell tableCell = new TableCell();
            tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            if (RecordCount > 0)
            {
                tableCell.Controls.Add(new LiteralControl("总条数：&nbsp;"));
                tableCell.Controls.Add(new LiteralControl(RecordCount.ToString()));
                tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;每页条数：&nbsp;&nbsp;"));
                tableCell.Controls.Add(new LiteralControl(PageSize.ToString()));
                tableCell.Controls.Add(new LiteralControl("&nbsp;当前页：&nbsp;&nbsp;"));
            }
            tableCell.Controls.Add(new LiteralControl((PageIndex + 1).ToString()));
            tableCell.Controls.Add(new LiteralControl("/"));
            tableCell.Controls.Add(new LiteralControl(PageCount.ToString()));
            tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"));
            LinkButton btnFrist = new LinkButton();
            LinkButton btnPrev = new LinkButton();
            LinkButton btnNext = new LinkButton();
            LinkButton btnLast = new LinkButton();
            if (!String.IsNullOrEmpty(PagerSettings.FirstPageImageUrl))
            {
                btnFrist.Text = "<img src='" + ResolveUrl(PagerSettings.FirstPageImageUrl) + "' border='0'/>";
            }
            else
            {
                btnFrist.Text = PagerSettings.FirstPageText;
            }
            btnFrist.Click += btnFrist_Click;
            btnFrist.Font.Underline = false;
            if (!String.IsNullOrEmpty(PagerSettings.PreviousPageImageUrl))
            {
                btnPrev.Text = "<img src='" + ResolveUrl(PagerSettings.PreviousPageImageUrl) + "' border='0'/>";
            }
            else
            {
                btnPrev.Text = PagerSettings.PreviousPageText;
            }
            btnPrev.Click += btnPrev_Click;
            btnPrev.Font.Underline = false;
            if (!String.IsNullOrEmpty(PagerSettings.NextPageImageUrl))
            {
                btnNext.Text = "<img src='" + ResolveUrl(PagerSettings.NextPageImageUrl) + "' border='0'/>";
            }
            else
            {
                btnNext.Text = PagerSettings.NextPageText;
            }
            btnNext.CommandName = "asd";
            btnNext.Click += btnNext_Click;
            btnNext.Font.Underline = false;
            if (!String.IsNullOrEmpty(PagerSettings.LastPageImageUrl))
            {
                btnLast.Text = "<img src='" + ResolveUrl(PagerSettings.LastPageImageUrl) + "' border='0'/>";
            }
            else
            {
                btnLast.Text = PagerSettings.LastPageText;
            }
            btnLast.Click += btnLast_Click;
            btnLast.Font.Underline = false;
            if (PageIndex <= 0)
            {
                btnFrist.Enabled = btnPrev.Enabled = false;
                btnFrist.ForeColor = System.Drawing.Color.Gray;
                btnPrev.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                btnFrist.Enabled = btnPrev.Enabled = true;
            }
            tableCell.Controls.Add(btnFrist);
            tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            tableCell.Controls.Add(btnPrev);
            tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            if (generateNumber)
            {
                int rightCount = (int)(PagerSettings.PageButtonCount / 2);
                int leftCount = PagerSettings.PageButtonCount % 2 == 0 ? rightCount - 1 : rightCount;
                for (int i = 0; i < PageCount; i++)
                {
                    if (PageCount > PagerSettings.PageButtonCount)
                    {
                        if (i < PageIndex - leftCount && PageCount - 1 - i > PagerSettings.PageButtonCount - 1)
                        {
                            continue;
                        }
                        else if (i > PageIndex + rightCount && i > PagerSettings.PageButtonCount - 1)
                        {
                            continue;
                        }
                    }
                    if (i == PageIndex)
                    {
                        tableCell.Controls.Add(new LiteralControl("<span style='color:red;font-weight:bold'>" + (i + 1).ToString() + "</span>"));
                    }
                    else
                    {
                        LinkButton lb = new LinkButton();
                        lb.Text = (i + 1).ToString();
                        lb.Click += lb_Click;
                        tableCell.Controls.Add(lb);
                    }
                    tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                }
            }
            if (PageIndex >= PageCount - 1)
            {
                btnNext.Enabled = btnLast.Enabled = false;
                btnNext.ForeColor = System.Drawing.Color.Gray;
                btnLast.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                btnNext.Enabled = btnLast.Enabled = true;
            }
            tableCell.Controls.Add(btnNext);
            tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            tableCell.Controls.Add(btnLast);
            tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            txtbox.Width = 40;
            tableCell.Controls.Add(txtbox);
            tableCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            LinkButton go = new LinkButton();
            go.Text = "转到";

            go.Click += go_Click;
            tableCell.Controls.Add(go);
            return tableCell;
        }

        void go_Click(object sender, EventArgs e)
        {
            int currentPage;
            if (int.TryParse(txtbox.Text, out currentPage))
            {
                if (currentPage > PageCount)
                    currentPage = PageCount;
                if (currentPage < 0)
                    currentPage = 1;
                GridViewPageEventArgs eventArgs = new GridViewPageEventArgs(currentPage);
                PageIndexChang(this, eventArgs);
            }

        }

        void lb_Click(object sender, EventArgs e)
        {
            LinkButton lb = sender as LinkButton;
            int index = int.Parse(lb.Text);
            GridViewPageEventArgs eventArgs = new GridViewPageEventArgs(index);
            PageIndexChang(this, eventArgs);
        }

        void btnLast_Click(object sender, EventArgs e)
        {
            GridViewPageEventArgs eventArgs = new GridViewPageEventArgs(PageCount-1);
            PageIndexChang(this, eventArgs);
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            GridViewPageEventArgs eventArgs = new GridViewPageEventArgs(PageIndex+1);
            PageIndexChang(this, eventArgs);
        }

        void btnPrev_Click(object sender, EventArgs e)
        {
            GridViewPageEventArgs eventArgs = new GridViewPageEventArgs(PageIndex-1);
            PageIndexChang(this, eventArgs);
        }

        void btnFrist_Click(object sender, EventArgs e)
        {
           GridViewPageEventArgs eventArgs=new GridViewPageEventArgs(0);
           PageIndexChang(this, eventArgs);
        }

        #region Help Methods
        /// <summary>
        /// Determine the first column in the column collection that has SortExpression value set, and using this column as a default sort column
        /// </summary>
        /// <returns></returns>
        protected string GetDefaultSortColumn()
        {
            string SortColumnName = string.Empty;

            for (int i = 0; i < this.Columns.Count; i++)
            {
                SortColumnName = this.Columns[i].SortExpression;
                if (SortColumnName != string.Empty)
                {
                    break;
                }
            }

            return SortColumnName;
        }

        private void DisplayPageSizeSelector(GridViewRow dgItem)
        {
            //TableCell pagerCell;
            //TableRow pagerRow;

            //int j = 0;
            //System.Web.UI.WebControls.DropDownList cboPageSize = new DropDownList();
            //cboPageSize.AutoPostBack = true;
            //cboPageSize.Width = new Unit(40, UnitType.Pixel);
            //((DropDownList)(cboPageSize)).SelectedIndexChanged += new EventHandler(this.cboPageSize_SelectedIndexChanged);

            //// -- limit the max page size to a 250 records
            //j = this.RecordsCount + _pageSelectorPageSizeInterval;
            //for (int i = _pageSelectorPageSizeInterval; i <= ((j > 250) ? 250 : j); )
            //{
            //    cboPageSize.Items.Add(i.ToString());
            //    i += _pageSelectorPageSizeInterval;
            //}

            //if (cboPageSize.Items.FindByText(this.PageSize.ToString()) != null)
            //{
            //    cboPageSize.Items.FindByText(this.PageSize.ToString()).Selected = true;
            //}

            //pagerRow = dgItem;
            //pagerCell = ((TableCell)(pagerRow.Controls[0]));
            //TableRow pagerTableRow = ((Table)pagerCell.Controls[0]).Rows[0];
            //TableCell cell = new TableCell();
            //cell.Text = string.Format("{0}: ", ShowPageText);
            //cell.Wrap = false;
            //cell.ApplyStyle(this.PagerStyle);
            //pagerTableRow.Cells.AddAt(0, cell);

            //cell = new TableCell();
            //if (StatisticsText == "")
            //{
            //    cell.Text = string.Format("&nbsp; ({1}: {0})", RecordsCount, TotalRecordsText);
            //}
            //else
            //{
            //    cell.Text = string.Format("&nbsp; ({1}: {0},{2})", RecordsCount, TotalRecordsText, StatisticsText);
            //}
            //cell.Wrap = false;
            //cell.ApplyStyle(this.PagerStyle);
            //pagerTableRow.Cells.Add(cell);

            //cell = new TableCell();
            //cell.Width = Unit.Percentage(100);
            //cell.ApplyStyle(this.PagerStyle);
            //pagerTableRow.Cells.Add(cell);
        }
        #endregion

        #region Controls Events

        #region cboPageSize_SelectedIndexChanged
        /// <summary> 
        /// Occurs when the value of the SelectedIndex property changes. 
        /// </summary> 
        private void cboPageSize_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // -- reset current page index to 0, that is nessessary so that there won't be a situation when gridview's current page index is off causing an exception
            this.PageIndex = 0;
            DropDownList cboPageSize = (DropDownList)sender;
            this.PageSize = int.Parse(cboPageSize.SelectedValue);

            if (PageSizeChanged != null) PageSizeChanged(sender, e);
        }
        #endregion

        #endregion

        #region Protected Methods
        /// <summary>
        ///  Get Sort Expression by Looking up the existing Grid View Sort Expression 
        /// </summary>
        protected string GetSortExpression(GridViewSortEventArgs e)
        {
            string[] sortColumns = null;
            string sortAttribute = SortExpression;

            //Check to See if we have an existing Sort Order already in the Grid View.	
            //If so get the Sort Columns into an array
            if (sortAttribute != String.Empty)
            {
                sortColumns = sortAttribute.Split(",".ToCharArray());
            }

            //if User clicked on the columns in the existing sort sequence.
            //Toggle the sort order or remove the column from sort appropriately

            if (sortAttribute.IndexOf(e.SortExpression) > 0 || sortAttribute.StartsWith(e.SortExpression))
                sortAttribute = ModifySortExpression(sortColumns, e.SortExpression);
            else
                sortAttribute += String.Concat(",", e.SortExpression, " ASC ");
            return sortAttribute.TrimStart(",".ToCharArray()).TrimEnd(",".ToCharArray());

        }
        /// <summary>
        ///  Toggle the sort order or remove the column from sort appropriately
        /// </summary>
        protected string ModifySortExpression(string[] sortColumns, string sortExpression)
        {

            string ascSortExpression = String.Concat(sortExpression, " ASC ");
            string descSortExpression = String.Concat(sortExpression, " DESC ");

            for (int i = 0; i < sortColumns.Length; i++)
            {

                if (ascSortExpression.Equals(sortColumns[i]))
                {
                    sortColumns[i] = descSortExpression;
                }

                else if (descSortExpression.Equals(sortColumns[i]))
                {
                    Array.Clear(sortColumns, i, 1);
                }
            }

            return String.Join(",", sortColumns).Replace(",,", ",").TrimStart(",".ToCharArray());

        }
        /// <summary>
        ///  Lookup the Current Sort Expression to determine the Order of a specific item.
        /// </summary>
        protected void SearchSortExpression(string[] sortColumns, string sortColumn, out string sortOrder, out int sortOrderNo)
        {
            sortOrder = "";
            sortOrderNo = -1;
            for (int i = 0; i < sortColumns.Length; i++)
            {
                if (sortColumns[i].StartsWith(sortColumn))
                {
                    sortOrderNo = i + 1;
                    if (AllowMultiColumnSorting)
                        sortOrder = sortColumns[i].Substring(sortColumn.Length).Trim();
                    else
                        sortOrder = ((SortDirection == SortDirection.Ascending) ? "ASC" : "DESC");
                }
            }
        }
        /// <summary>
        ///  Display a graphic image for the Sort Order along with the sort sequence no.
        /// </summary>
        protected void DisplaySortOrderImages(string sortExpression, GridViewRow dgItem)
        {
            string[] sortColumns = sortExpression.Split(",".ToCharArray());

            for (int i = 0; i < dgItem.Cells.Count; i++)
            {
                if (dgItem.Cells[i].Controls.Count > 0 && dgItem.Cells[i].Controls[0] is LinkButton)
                {
                    string sortOrder;
                    int sortOrderNo;
                    string column = ((LinkButton)dgItem.Cells[i].Controls[0]).CommandArgument;
                    SearchSortExpression(sortColumns, column, out sortOrder, out sortOrderNo);
                    if (sortOrderNo > 0)
                    {
                        string sortImgLoc = (sortOrder.Equals("ASC") ? SortAscImageUrl : SortDescImageUrl);

                        if (sortImgLoc != String.Empty)
                        {
                            Image imgSortDirection = new Image();
                            imgSortDirection.ImageUrl = sortImgLoc;
                            dgItem.Cells[i].Controls.Add(imgSortDirection);

                            if (AllowMultiColumnSorting)
                            {
                                Label lblSortOrder = new Label();
                                lblSortOrder.Font.Size = FontUnit.Small;
                                lblSortOrder.Text = sortOrderNo.ToString();
                                dgItem.Cells[i].Controls.Add(lblSortOrder);
                            }

                        }
                        else
                        {

                            Label lblSortDirection = new Label();
                            lblSortDirection.Font.Size = FontUnit.XSmall;
                            lblSortDirection.EnableTheming = false;
                            lblSortDirection.Text = (sortOrder.Equals("ASC") ? "&#9650;" : "&#9660;");
                            dgItem.Cells[i].Controls.Add(lblSortDirection);

                            if (AllowMultiColumnSorting)
                            {
                                Literal litSortSeq = new Literal();
                                litSortSeq.Text = sortOrderNo.ToString();
                                dgItem.Cells[i].Controls.Add(litSortSeq);

                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
    
}
