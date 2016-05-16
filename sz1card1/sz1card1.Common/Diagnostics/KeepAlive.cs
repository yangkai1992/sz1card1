using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Text;
using sz1card1.Common;
using sz1card1.Common.Log;

namespace sz1card1.Common.Diagnostics
{
    public class KeepAlive:IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //LoggingService.Info(string.Format("收到心跳包(IP:{0},Host:{1}).", context.Request.ServerVariables["REMOTE_ADDR"], context.Request.Url.Host));
            context.Response.Write("success");
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
