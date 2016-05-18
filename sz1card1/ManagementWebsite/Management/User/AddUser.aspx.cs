using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sz1card1.Management.Logic;
using sz1card1.Management.Data.Entities;
using System.Data;

public partial class Management_AddUser : BasePage
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
            DataTable userGroup = GetUserLogic().GetUserGroupList();
            if(CurrentUser.UserGroupGuid == new Guid("FFD19FCD-E49D-E311-9066-90B11C47E695")) //销售部-主管新增的用户只能是销售部业务权限
            {
                txtUserWeight.Text = "0";
                txtUserWeight.Enabled = false;
                DataRow[] rws = userGroup.Select("Guid in (CONVERT('B167ED77-6712-4121-9E11-1FBD8F802CA8', 'System.Guid'),CONVERT('9ACF4DD6-E5A1-4921-810B-8021EDF9667B', 'System.Guid'),CONVERT('17DF0011-6ACC-E211-81C9-90B11C47E696', 'System.Guid'),CONVERT('18DF0011-6ACC-E211-81C9-90B11C47E696', 'System.Guid'),CONVERT('1ADF0011-6ACC-E211-81C9-90B11C47E696', 'System.Guid'),CONVERT('1CDF0011-6ACC-E211-81C9-90B11C47E696', 'System.Guid'))");//非销售部业务
                foreach(DataRow rw in rws)
                {
                    userGroup.Rows.Remove(rw);
                }
                
            }
            ddlUerGroup.DataSource = userGroup;
            ddlUerGroup.DataBind();
        }

    }

    /// <summary>
    /// 添加用户
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btsubmit_Click(object sender, EventArgs e)
    {
        string message = "添加失败!";
        User user = new User();
        try
        {
            if (Convert.ToInt32(txtUserWeight.Text.Trim()) < Convert.ToInt32(txtUserWeightUsed.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(GetType(), "error", "已用权重不能大于用户权重！", true);
                return;
            }
            user.TrueName = tbName.Text;
            user.Account = tbAccount.Text;
            user.UserGroupGuid = new Guid(ddlUerGroup.SelectedValue);
            user.Password = tbPassword.Text;
            user.Meno = tbmeno.Value;
            user.Mobile = tbPhone.Text;
            user.Tel = txtTel.Text.Trim();
            user.AddTime = DateTime.Now;
            user.IsLocked = ddlLocked.SelectedIndex == 0 ? false : true;
            user.Email = tbEmail.Text;
            user.UserWeight = Convert.ToInt32(txtUserWeight.Text.Trim());
            user.UserWeightUsed = Convert.ToInt32(txtUserWeightUsed.Text.Trim());
            bool statu = GetUserLogic().AddUser(user, out message);
            if (statu)
            {
                sz1card1.Common.Log.LoggingService.Info(string.Format("{0}添加了{1}用户!~", UserAccount, tbName.Text));
                ClientScript.RegisterStartupScript(GetType(), "succed", "window.parent.AddUserResult('success');", true);
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

    protected override string PermitonUrl
    {
        get
        {
            return "User/UserList.aspx";
        }
    }
}
