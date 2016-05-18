using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///扩展FckEditor截图功能
/// </summary>
public class Snapshot : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string fileName = context.Request.Files[0].FileName;
        string path = context.Server.MapPath("~/Upload/Snapshot/" + fileName);
        context.Request.Files[0].SaveAs(path);
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
