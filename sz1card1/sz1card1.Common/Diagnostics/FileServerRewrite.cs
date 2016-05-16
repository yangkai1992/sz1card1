using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace sz1card1.Common.Diagnostics
{
    public class FileServerRewrite : IHttpModule
    {
        #region IHttpModule 成员

        void IHttpModule.Dispose()
        {
            throw new NotImplementedException();
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            if (application.Request.Url.ToString().Contains("group1"))
            {
                application.Response.Redirect("http://www.baidu.com");
            }
        }
        #endregion
    }
}
