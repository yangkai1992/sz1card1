using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sz1card1.Common;

public partial class Management_Business_AddValueRecord : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 搜素
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Setsearch_Click(object sender, EventArgs e)
    {
        gvLogList.PageIndex = 0;
        gvLogList.DataBind();

    }
    protected void dataAddValue_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        string where = "1=1";

        if (!string.IsNullOrEmpty(txtAccount.Text.Trim()))
        {
            where += string.Format(" and ( RecommendAccount = '{0}' or PresenteeAccount = '{1}') ",
                   txtAccount.Text.Trim(), txtAccount.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txtName.Text.Trim()))
        {
            where += string.Format(" and ( RecommendName like '%{0}%' or PresenteeName like '%{1}%') ",
                txtName.Text.Trim(), txtName.Text.Trim());
        }

        if (tbstartTime.Value.Trim() != "")
        {
            where += string.Format(" and DateDiff(ss,'{0}',OperationTime) > 0 ", 
                Convert.ToDateTime(tbstartTime.Value.Trim() + " 00:00:00"));
        }
        if (tbendTime.Value.Trim() != "")
        {
            where += string.Format(" and DateDiff(ss,'{0}',OperationTime) < 0 ", 
                Convert.ToDateTime(tbendTime.Value.Trim() + " 23:59:59"));
        }
        if (!string.IsNullOrEmpty(recommenderName.Text.Trim()))
        {
            where += string.Format(" and ( RecommederName like '%{0}%') ", recommenderName.Text.Trim());
        }
        if (signingtime_start.Value.Trim() != "")
        {
            where += string.Format(" and DateDiff(ss,'{0}',SigningTime) > 0 ",
                Convert.ToDateTime(signingtime_start.Value.Trim() + " 00:00:00"));
        }
        if (signingtime_end.Value.Trim() != "")
        {
            where += string.Format(" and DateDiff(ss,'{0}',SigningTime) < 0 ",
                Convert.ToDateTime(signingtime_end.Value.Trim() + " 23:59:59"));
        }
        e.InputParameters[0] = where;
    }

}
