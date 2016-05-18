using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sz1card1.Common;
using sz1card1.Common.UI;
using sz1card1.Management.Logic;
using sz1card1.Management.Data.Entities;

public partial class Management_UpdateUserPwd : BasePage
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

    }
    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string message = string.Empty;
        try
        {
            bool statu = GetUserLogic().UpdatePassword(CurrentUser.Account, this.tbPwd.Text, this.tbnewPwd.Text, out message);
            if (statu)
            {
                sz1card1.Common.Log.LoggingService.Info(string.Format("{0}修改了{1}账户密码!~", UserAccount, UserAccount));
                if (!string.IsNullOrEmpty(Request.QueryString["way"]))
                {
                     ClientScript.RegisterStartupScript(GetType(), "success", "window.parent.ClosePopupDiv('success');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "succeed", string.Format("alert('{0}');", message), true);
                }
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
