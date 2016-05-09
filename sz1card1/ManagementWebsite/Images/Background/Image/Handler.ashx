<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Linq;
using sz1card1.Common;
using sz1card1.Management.Data;
using sz1card1.Management.Data.Entities;

public class Handler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public HttpServerUtility Server;
    public void ProcessRequest(HttpContext context)
    {
        Server = context.Server;

        CreateImg(context);
    }

    public void CreateImg(HttpContext context)
    {
        bool result = false;
        string path = string.Empty;
        if (!string.IsNullOrEmpty(context.Request["imgPath"]))
        {
            path = Server.MapPath(context.Request["imgPath"] + ".jpg");
            sz1card1.Management.Logic.BusinessLogic logic = new sz1card1.Management.Logic.BusinessLogic();
            result = logic.GetBusinessInfo(context.Request["SID"].ToString().FromBase64String()).IsRealUsed;
        }
        else
            path = HttpContext.Current.Request.Url.Host.ToString() + "/Images/Wallpapers/desktop.jpg";
        System.Drawing.Image image = System.Drawing.Image.FromFile(path);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
        g.DrawImage(image, 0, 0, image.Width, image.Height);
        if (!result)
        {
            Font f = new Font("Tahoma", 25, FontStyle.Regular);
            Brush b = new SolidBrush(Color.Red);
            string addText = "试用版";
            g.DrawString(addText, f, b, image.Width - 300, 10);
            g.Dispose();
        }
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        context.Response.ClearContent();
        context.Response.ContentType = "image/Jpeg";
        context.Response.BinaryWrite(ms.ToArray());


    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}