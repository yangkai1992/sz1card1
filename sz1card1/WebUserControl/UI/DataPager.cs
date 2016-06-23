using System;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUserControl.Enum;

namespace WebUserControl.UI
{
    public class DataPager : TableCell
    {
        public const string FIRST_PAGE = "First";
        public const string PREV_PAGE = "Prev";
        public const string NEXT_PAGE = "Next";
        public const string LAST_PAGE = "Last";
        public const string PAGE_ARGUMENT = "Page";
        public const char ARGUMENT_SPLITTER = '$';
        private int _pageIndex;
        private int _recordCount;
        private int _pageSize;
        private int _pageCount;
        private PagerSettings _settings;
        TextBox txtbox = new TextBox();

        public DataPager(PagerSettings setting, int pageIndex, int recordCount, int pageSize)
        {
            _settings = setting;
            _pageIndex = pageIndex;
            _recordCount = recordCount;
            _pageSize = pageSize;
            _pageCount = _recordCount % _pageSize == 0 ? _recordCount / _pageSize : _recordCount / _pageSize + 1;
            GeneratePage();
        }

        private void GeneratePage()
        {
            if (_settings.Mode == PagerButtons.NextPrevious || _settings.Mode == PagerButtons.NextPreviousFirstLast)
            {
                GeneratePrevNextPage();
            }
            else if (_settings.Mode == PagerButtons.Numeric || _settings.Mode == PagerButtons.NumericFirstLast)
            {
                GenerateNumericPage();
            }
        }

        private void GeneratePrevNextPage()
        {
            GeneratePage(false);
        }
        private void GenerateNumericPage()
        {
            GeneratePage(true);
        }

        private void GeneratePage(bool generateNumber)
        {
            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            if (_recordCount > 0)
            {
                this.Controls.Add(new LiteralControl("总条数：&nbsp;"));
                this.Controls.Add(new LiteralControl(_recordCount.ToString()));
                this.Controls.Add(new LiteralControl("&nbsp;&nbsp;每页条数：&nbsp;&nbsp;"));
                this.Controls.Add(new LiteralControl(_pageSize.ToString()));
                this.Controls.Add(new LiteralControl("&nbsp;当前页：&nbsp;&nbsp;"));
            }
            this.Controls.Add(new LiteralControl((_pageIndex + 1).ToString()));
            this.Controls.Add(new LiteralControl("/"));
            this.Controls.Add(new LiteralControl(_pageCount.ToString()));
            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"));
            LinkButton btnFrist = new LinkButton();
            LinkButton btnPrev = new LinkButton();
            LinkButton btnNext = new LinkButton();
            LinkButton btnLast = new LinkButton();

            btnFrist.Text = "首页";
            
            btnFrist.CommandName = PAGE_ARGUMENT;
            btnFrist.CommandArgument = FIRST_PAGE;
            btnFrist.Font.Underline = false;

            btnPrev.Text = "前一页";
            
            btnPrev.CommandName = PAGE_ARGUMENT;
            btnPrev.CommandArgument = PREV_PAGE;
            btnPrev.Font.Underline = false;
            if (!String.IsNullOrEmpty(_settings.NextPageImageUrl))
            {
                btnNext.Text = "<img src='" + ResolveUrl(_settings.NextPageImageUrl) + "' border='0'/>";
            }
            else
            {
                btnNext.Text = _settings.NextPageText;
            }
            btnNext.CommandName = PAGE_ARGUMENT;
            btnNext.CommandArgument = NEXT_PAGE;
            btnNext.Font.Underline = false;
            if (!String.IsNullOrEmpty(_settings.LastPageImageUrl))
            {
                btnLast.Text = "<img src='" + ResolveUrl(_settings.LastPageImageUrl) + "' border='0'/>";
            }
            else
            {
                btnLast.Text = _settings.LastPageText;
            }
            btnLast.CommandName = PAGE_ARGUMENT;
            btnLast.CommandArgument = LAST_PAGE;
            btnLast.Font.Underline = false;
            if (this._pageIndex <= 0)
            {
                btnFrist.Enabled = btnPrev.Enabled = false;
                btnFrist.ForeColor = System.Drawing.Color.Gray;
                btnPrev.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                btnFrist.Enabled = btnPrev.Enabled = true;
            }
            this.Controls.Add(btnFrist);
            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            this.Controls.Add(btnPrev);
            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            if (generateNumber)
            {
                int rightCount = (int)(_settings.PageButtonCount / 2);
                int leftCount = _settings.PageButtonCount % 2 == 0 ? rightCount - 1 : rightCount;
                for (int i = 0; i < _pageCount; i++)
                {
                    if (_pageCount > _settings.PageButtonCount)
                    {
                        if (i < _pageIndex - leftCount && _pageCount - 1 - i > _settings.PageButtonCount - 1)
                        {
                            continue;
                        }
                        else if (i > _pageIndex + rightCount && i > _settings.PageButtonCount - 1)
                        {
                            continue;
                        }
                    }
                    if (i == _pageIndex)
                    {
                        this.Controls.Add(new LiteralControl("<span style='color:red;font-weight:bold'>" + (i + 1).ToString() + "</span>"));
                    }
                    else
                    {
                        LinkButton lb = new LinkButton();
                        lb.Text = (i + 1).ToString();
                        lb.CommandName = PAGE_ARGUMENT;
                        lb.CommandArgument = (i + 1).ToString();
                        this.Controls.Add(lb);
                    }
                    this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                }
            }
            if (this._pageIndex >= _pageCount - 1)
            {
                btnNext.Enabled = btnLast.Enabled = false;
                btnNext.ForeColor = System.Drawing.Color.Gray;
                btnLast.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                btnNext.Enabled = btnLast.Enabled = true;
            }
            this.Controls.Add(btnNext);
            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            this.Controls.Add(btnLast);
            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            txtbox.Width = 40;
            this.Controls.Add(txtbox);
            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            LinkButton go = new LinkButton();
            go.Text = "转到";

            go.Click += go_Click;
            this.Controls.Add(go);
        }

        void go_Click(object sender, EventArgs e)
        {
            LinkButton link = sender as LinkButton;            
            int currentPage;
            if (int.TryParse(txtbox.Text,out currentPage))
            {
                if (currentPage > _pageCount)
                    currentPage = _pageCount;
                if (currentPage < 0)
                    currentPage = 1;
                link.CommandName = PAGE_ARGUMENT;
                link.CommandArgument = currentPage.ToString();
            }            
        }
    }
}
