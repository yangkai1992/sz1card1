using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sz1card1.Management.Data.Entities;
using sz1card1.Management.Logic;

public partial class Management_User_EditGroup : BasePage
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
            if (!string.IsNullOrEmpty(Request.QueryString["groupGuid"]))
            {
                UserGroup user = GetUserLogic().GetUserGroupByid(new Guid(Request.QueryString["groupGuid"].ToString()));
                tbName.Text = user.GroupName;
                tbmeno.Value = user.Meno;
                rspopedom.SelectedXml = user.MenuXml;
                txtGroupWeight.Text = user.GroupWeight == null ? "0" : user.GroupWeight.ToString();
                txtGroupWeightUsed.Text = user.GroupWeightUsed == null ? "0" : user.GroupWeightUsed.ToString();
            }
        }
    }
    /// <summary>
    /// 保存设置好的权限
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btSubmit_Click(object sender, EventArgs e)
    {
        string message = "操作失败!";
        if (!string.IsNullOrEmpty(Request.QueryString["groupGuid"]))
        {
            try
            {
                if (Convert.ToInt32(txtGroupWeight.Text.Trim()) < Convert.ToInt32(txtGroupWeightUsed.Text.Trim()))
                {
                    ClientScript.RegisterStartupScript(GetType(), "error", "已用权重不能大于部门权重！", true);
                    return;
                }
                UserGroup user = new UserGroup();
                user.Meno = tbmeno.Value;
                user.Guid = new Guid(Request.QueryString["groupGuid"].ToString());
                user.GroupName = tbName.Text;
                //user.GroupWeight = Convert.ToInt32(txtGroupWeight.Text.Trim());
                //user.GroupWeightUsed = Convert.ToInt32(txtGroupWeightUsed.Text.Trim());
                user.MenuXml = rspopedom.SelectedXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Trim();
                bool statu = GetUserLogic().UpdateGroup(user, out message);
                if (statu)
                {
                    ClientScript.RegisterStartupScript(GetType(), "succed", "window.parent.ColseEditPopedom('ok');", true);
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

    protected override string PermitonUrl
    {
        get
        {
            return "User/UserGroup.aspx";
        }
    }
}
