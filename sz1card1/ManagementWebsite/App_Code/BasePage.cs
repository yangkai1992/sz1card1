using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;
using sz1card1.Management.Data;
using sz1card1.Management.Data.Entities;
using sz1card1.Common;

/// <summary>
///BasePage 的摘要说明
/// </summary>
public class BasePage : Page
{
    private string userAccount;
    private string permitionUrl = "";
    private User currentUser;

    public BasePage()
    {
    }


    /// <summary>
    /// 当前用户名
    /// </summary>
    protected string UserAccount
    {
        get
        {
            return userAccount;
        }
    }

    protected virtual string PermitonUrl
    {
        get
        {
            if (permitionUrl.Trim() == "")
            {
                permitionUrl = Request.Url.AbsolutePath.ToLower().Substring(Request.Url.AbsolutePath.ToLower().IndexOf("/management/") + 12);
            }
            return permitionUrl;
        }
    }
    /// <summary>
    /// 当前用户
    /// </summary>
    protected User CurrentUser
    {
        get
        {
            if (currentUser == null)
            {
                ManagementDataContext dataContext = new ManagementDataContext();
                currentUser = dataContext.Users.First<User>(u => u.Account == userAccount);
            }
            return currentUser;
        }
    }


    protected override void OnInit(System.EventArgs e)
    {
        base.OnInit(e);
        if (Request.Cookies["Token"] == null || string.IsNullOrEmpty(Request.Cookies["Token"].Value) || HttpContext.Current.Session[Request.Cookies["Token"].Value]==null)
        {
            Response.Write("<script>alert('登录超时请重新登录'); window.parent.parent.location.href='/management/login.aspx'</script> ");
            Response.End();
            return;
        }
        else
        {
            userAccount = HttpContext.Current.Session[Request.Cookies["Token"].Value].ToString();            
            ManagementDataContext dataContext = new ManagementDataContext();
            currentUser = dataContext.Users.First<User>(u => u.Account == userAccount);
            if ((userAccount + currentUser.Password).ToMD5() != Request.Cookies["Token"].Value)
            {
                Response.Write("<script>alert('登录超时请重新登录'); window.parent.parent.location.href='/management/login.aspx'</script> ");
                Response.End();
                return;
            }
        }
    }

    /// <summary>
    /// 判断某种权限在权限xml中是否存在
    /// </summary>
    /// <returns></returns>
    protected bool JustPermitionExist()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(CurrentUser.UserGroup.MenuXml);
        XmlNodeList nodelist = xmlDoc.SelectNodes(string.Format("//menu[translate(url,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='{0}']", PermitonUrl.ToLower()));
        if (nodelist.Count > 0)
            return true;
        return false;
    }
}
