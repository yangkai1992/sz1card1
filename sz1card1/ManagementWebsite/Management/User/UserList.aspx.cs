using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using sz1card1.Management.Logic;
using sz1card1.Management.Data.Entities;

public partial class Management_UserList : BasePage
{
    private UserLogic userLogic = null;
    private UserLogic GetUserLogic()
    {
        if (userLogic == null)
        {
            userLogic = new UserLogic();
        }
        return userLogic;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserLogic userLogic = new UserLogic();
            ddlUerGroup.AppendDataBoundItems = true;
            ddlUerGroup.DataSource = userLogic.GetUserGroupList();
            ddlUerGroup.DataBind();
          
        }
    }
    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbDelete_Command(object sender, CommandEventArgs e)
    {
        string message = "删除失败!";
        try
        {
            bool statu = GetUserLogic().DeleteUser(e.CommandArgument.ToString(), out message);
            if (statu)
            {
                sz1card1.Common.Log.LoggingService.Info(string.Format("{0}删除了{1}用户!~", UserAccount, e.CommandArgument.ToString()));
                gvUserList.DataBind();
            }
            else
            {
                sz1card1.Common.Log.LoggingService.Error(message);
                ClientScript.RegisterStartupScript(GetType(), "error", string.Format("alert('{0}');", message), true);
            }

        }
        catch (Exception ex)
        {
            sz1card1.Common.Log.LoggingService.Warn(message, ex);
            ClientScript.RegisterStartupScript(GetType(), "error", string.Format("alert('{0}');", message), true);
        }
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btsearch_Click(object sender, EventArgs e)
    {
        gvUserList.PageIndex = 0;
        gvUserList.DataBind();

    }

    /// <summary>
    /// 数据行绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton linbutn = e.Row.Cells[0].FindControl("lbDelete") as LinkButton;
            if (e.Row.Cells[9].Text == "False")
            {
                e.Row.Cells[9].Text = "正常";
            }
            else
            {
                e.Row.Cells[9].Text = "<span style='Color:Red'>锁定</span>";
            }
            if (e.Row.Cells[1].Text == "admin")
            {               
                linbutn.Enabled = false;
                linbutn.OnClientClick = "return prompt();";
            }
        }
    }

    protected void dataUserList_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        string where = "1=1";
        if (tbuserAcount.Text.Trim() != "")
        {
            where += string.Format(" and Account like '%{0}%'", tbuserAcount.Text.Trim());
        }
        if (tbuserName.Text.Trim() != "")
        {
            where += string.Format(" and TrueName like '%{0}%'", tbuserName.Text.Trim());
        }
        if (this.ddlUerGroup.SelectedValue.Trim() != "")
        {
            where += string.Format(" and UserGroupGuid='{0}'", ddlUerGroup.SelectedValue);
        }
        if (this.ddlIsLocked.SelectedValue.Trim() != "")
        {
            where += string.Format(" and IsLocked='{0}'", ddlIsLocked.SelectedValue.Trim());
        }
        if (tbMobile.Text.Trim() != "")
        {
            where += string.Format(" and Mobile like '%{0}%'", tbMobile.Text.Trim());
        }
        if (tbEmail.Text.Trim() != "")
        {
            where += string.Format(" and Email like '%{0}%'", tbEmail.Text.Trim());
        }
        e.InputParameters[0] = where;
    }

    /// <summary>
    /// 锁定/解锁 用户
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void barHeader_ItemPostBack(sz1card1.Common.UI.ToolbarItem item)
    {
        bool statu = true;
        string message = string.Empty;
        try
        {
            foreach (GridViewRow row in gvUserList.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    HtmlInputCheckBox chk = (HtmlInputCheckBox)row.Cells[0].FindControl("isCheck");
                    if (chk.Checked)
                    {
                        if (item.ItemId == "tbUnlock")
                        {
                            if (Convert.ToBoolean(chk.Value))
                            {
                                statu = GetUserLogic().ChangeUserState(row.Cells[1].Text, false, out message);
                            }
                            if (statu)
                            {
                                continue;
                            }
                            break;
                        }
                        if (item.ItemId == "tbLock")
                        {
                            if (!Convert.ToBoolean(chk.Value))
                            {
                                statu = GetUserLogic().ChangeUserState(row.Cells[1].Text, true, out message);
                            }
                            if (statu)
                            {
                                continue;
                            }
                            break;
                        }
                    }
                }
            }
            if (statu)
            {
                gvUserList.DataBind();
            }
            else
            {
                sz1card1.Common.Log.LoggingService.Error(message);
                ClientScript.RegisterStartupScript(GetType(), "error", string.Format("alert('{0}');", message), true);
            }
        }
        catch (Exception ex)
        {
            sz1card1.Common.Log.LoggingService.Warn(message, ex);
            ClientScript.RegisterStartupScript(GetType(), "error", string.Format("alert('{0}');", message), true);
        }

    }
}
