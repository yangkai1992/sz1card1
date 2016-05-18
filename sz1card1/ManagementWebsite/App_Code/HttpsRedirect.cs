using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using sz1card1.Common;
using sz1card1.Management.Data.Entities;
using sz1card1.Management.Logic;

/// <summary>
///HttpsRedirect 的摘要说明
/// </summary>
public class HttpsRedirect : IHttpModule
{
    public HttpsRedirect()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    #region IHttpModule 成员

    public void Dispose()
    {
    }

    public void Init(HttpApplication context)
    {
        context.BeginRequest += new EventHandler(context_BeginRequest);
        context.AcquireRequestState += context_AcquireRequestState;


    }

    /// <summary>
    /// 拦截器
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void context_AcquireRequestState(object sender, EventArgs e)
    {
        HttpApplication context = sender as HttpApplication;
        string url=context.Request.Url.AbsolutePath.ToLower();

        if (url.Contains("editgroup"))
        {
                string userAccount = HttpContext.Current.Session[context.Request.Cookies["Token"].Value].ToString();
                UserLogic userLogic = new UserLogic();
                User user = userLogic.GetUserInfo(userAccount);
                if (user.UserGroupGuid != new Guid("B167ED77-6712-4121-9E11-1FBD8F802CA8"))
                {
                    context.Response.Write("<script>alert('您无权访问此页面'); window.parent.parent.location.href='/management/main.aspx'</script> ");
                    context.Response.End();
                    return;
                }            
        }
        if (url.Contains("business/addvalue"))
        {
            string userAccount = HttpContext.Current.Session[context.Request.Cookies["Token"].Value].ToString();
            UserLogic userLogic = new UserLogic();
            User user = userLogic.GetUserInfo(userAccount);
            if (user.UserGroupGuid != new Guid("B167ED77-6712-4121-9E11-1FBD8F802CA8")
                && user.UserGroupGuid != new Guid("9ACF4DD6-E5A1-4921-810B-8021EDF9667B")
                && user.UserGroupGuid != new Guid("FFD19FCD-E49D-E311-9066-90B11C47E695")
                && user.UserGroupGuid != new Guid("17DF0011-6ACC-E211-81C9-90B11C47E696")
                && user.UserGroupGuid != new Guid("18DF0011-6ACC-E211-81C9-90B11C47E696"))
            {
                context.Response.Write("<script>alert('您无权访问此页面'); window.parent.parent.location.href='/management/main.aspx'</script> ");
                context.Response.End();
                return;
            }
        }
        if (url.Contains("agent/addagentvalue"))
        {
            string userAccount = HttpContext.Current.Session[context.Request.Cookies["Token"].Value].ToString();
            UserLogic userLogic = new UserLogic();
            User user = userLogic.GetUserInfo(userAccount);
            if (user.UserGroupGuid != new Guid("B167ED77-6712-4121-9E11-1FBD8F802CA8")
                && user.UserGroupGuid != new Guid("9ACF4DD6-E5A1-4921-810B-8021EDF9667B")
                && user.UserGroupGuid != new Guid("FFD19FCD-E49D-E311-9066-90B11C47E695")
                && user.UserGroupGuid != new Guid("17DF0011-6ACC-E211-81C9-90B11C47E696")
                && user.UserGroupGuid != new Guid("18DF0011-6ACC-E211-81C9-90B11C47E696"))
            {
                context.Response.Write("<script>alert('您无权访问此页面'); window.parent.parent.location.href='/management/main.aspx'</script> ");
                context.Response.End();
                return;
            }
        }
    }




    private void context_BeginRequest(object sender, EventArgs e)
    {
        HttpApplication context = sender as HttpApplication;
        //修复浏览器cookie被篡改的问题
        foreach (string key in context.Request.Cookies.AllKeys)
        {
            if (key.Contains(", "))
            {
                HttpCookie oldCookie = context.Request.Cookies[key];
                HttpCookie newCookie = new HttpCookie(key.Replace(", ", ""))
                {
                    Domain = oldCookie.Domain,
                    Expires = oldCookie.Expires,
                    HttpOnly = oldCookie.HttpOnly,
                    Path = oldCookie.Path,
                    Secure = oldCookie.Secure,
                    Value = oldCookie.Value,
                };
                context.Request.Cookies.Add(newCookie);
                break;
            }
        }
        //string localPath = context.Context.context.Request.Url.LocalPath.ToLower();
        //string url = context.Context.context.Request.Url.AbsoluteUri.ToLower();
        //string host = context.Context.context.Request.Url.Host;
        //string agentip = context.Context.context.Request["HTTP_X_FORWARDED_FOR"];
        //if ((localPath == "/login.aspx" || localPath == "/interface/help.aspx" || localPath == "/management/login.aspx" ||
        //    localPath == "/showbulletin.aspx" || localPath.Contains("/interface/addvalue") ||
        //    localPath.Contains("/interface/faq")) && context.context.Request.ServerVariables["HTTP_X_FORWARDED_PROTO"] != "https")
        //{
        //    if (host == "www.1card1.cn" && !string.IsNullOrEmpty(agentip) && !agentip.Contains(","))
        //    {
        //        context.Context.Response.Redirect(context.Context.context.Request.Url.AbsoluteUri.Replace("http:", "https:"));
        //        context.CompleteRequest();
        //    }
        //}
    }

    #endregion
}
