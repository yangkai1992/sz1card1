using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sz1card1.Management.Logic;
using sz1card1.Management.Data.Entities;

public partial class Management_UserMessage : BasePage
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
        User user = GetUserLogic().GetUserInfo(CurrentUser.Account);
        if (user != null)
        {
            tbAccount.Text = user.Account;
            tbEmail.Text = user.Email;
            tbName.Text = user.TrueName;
            tbType.Text = user.UserGroup.GroupName;
            if (user.IsLocked)
            {
                tbState.Text = "锁定";
            }
            else
            {
                tbState.Text = "正常";
            }
            tbmeno.Value = user.Meno;
        }
    }
}
