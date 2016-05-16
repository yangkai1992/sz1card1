using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WebForms;

namespace sz1card1.Common
{
    public class ReportViewerMessages : IReportViewerMessages
    {
        public string BackButtonToolTip
        {
            get { return ("返回到父报表"); }
        }

        public string ChangeCredentialsText
        {
            get { return ("更改凭据"); }
        }

        public string ChangeCredentialsToolTip
        {
            get { return ("更改凭据"); }
        }

        public string CurrentPageTextBoxToolTip
        {
            get { return ("当前页"); }
        }

        public string DocumentMap
        {
            get { return ("文档结构图"); }
        }

        public string DocumentMapButtonToolTip
        {
            get { return (""); }
        }

        public string ExportButtonText
        {
            get { return ("导出"); }
        }

        public string ExportButtonToolTip
        {
            get { return ("导出"); }
        }

        public string ExportFormatsToolTip
        {
            get { return ("导出格式"); }
        }

        public string FalseValueText
        {
            get { return (""); }
        }

        public string FindButtonText
        {
            get { return ("查找"); }
        }

        public string FindButtonToolTip
        {
            get { return ("查找"); }
        }

        public string FindNextButtonText
        {
            get { return ("下一个"); }
        }

        public string FindNextButtonToolTip
        {
            get { return ("下一个"); }
        }

        public string FirstPageButtonToolTip
        {
            get { return ("第一页"); }
        }

        public string InvalidPageNumber
        {
            get { return ("页码错误"); }
        }

        public string LastPageButtonToolTip
        {
            get { return ("最后一页"); }
        }

        public string NextPageButtonToolTip
        {
            get { return ("下一页"); }
        }

        public string NoMoreMatches
        {
            get { return ("没有匹配项"); }
        }

        public string NullCheckBoxText
        {
            get { return (""); }
        }

        public string NullValueText
        {
            get { return ("没有可用的数据"); }
        }

        public string PageOf
        {
            get { return ("/"); }
        }

        public string ParameterAreaButtonToolTip
        {
            get { return ("显示/隐藏 参数区域"); }
        }

        public string PasswordPrompt
        {
            get { return ("密码"); }
        }

        public string PreviousPageButtonToolTip
        {
            get { return ("上一页"); }
        }

        public string PrintButtonToolTip
        {
            get { return ("打印"); }
        }

        public string ProgressText
        {
            get { return ("正在执行"); }
        }

        public string RefreshButtonToolTip
        {
            get { return ("刷新"); }
        }

        public string SearchTextBoxToolTip
        {
            get { return ("请输入搜索内容"); }
        }

        public string SelectAValue
        {
            get { return ("请选择"); }
        }

        public string SelectAll
        {
            get { return ("选择全部"); }
        }

        public string SelectFormat
        {
            get { return ("请选择格式"); }
        }

        public string TextNotFound
        {
            get { return ("没有找到该内容"); }
        }

        public string TodayIs
        {
            get { return ("今天是"); }
        }

        public string TrueValueText
        {
            get { return ("是"); }
        }

        public string UserNamePrompt
        {
            get { return ("用户名"); }
        }

        public string ViewReportButtonText
        {
            get { return ("查看报表"); }
        }

        public string ZoomControlToolTip
        {
            get { return ("显示比例"); }
        }

        public string ZoomToPageWidth
        {
            get { return (""); }
        }

        public string ZoomToWholePage
        {
            get { return (""); }
        }
    }
}
