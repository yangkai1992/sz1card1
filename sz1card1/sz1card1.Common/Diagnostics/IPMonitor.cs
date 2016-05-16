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

namespace sz1card1.Common.Diagnostics
{
    public class IPMonitor : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            NameValueCollection ips = ConfigurationManager.GetSection("ipMonitor") as NameValueCollection;
            foreach (string key in ips.Keys)
            {
                if (Utility.GetIPAddress() == ips[key])
                {
                    string param="";
                    foreach(string k in application.Request.Params.Keys)
                    {
                        param = param + string.Format("{0}={1}&", k, application.Request.Params[k]);
                    }
                    sz1card1.Common.Log.LoggingService.Warn(string.Format("【发现非法IP访问】(IP:{0},url:{1},Parameters:{2})", ips[key], application.Request.RawUrl, param));                   
                    break;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
