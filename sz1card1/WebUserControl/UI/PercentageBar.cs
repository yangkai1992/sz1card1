using System;
using System.IO;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace WebUserControl.UI
{
    [
        ToolboxData("<{0}:PercentageBar runat=\"server\"></{0}:PercentageBar>")
    ]
    public class PercentageBar : WebControl, IHttpHandler
    {
        private decimal percent = 0;
        private Unit width = new Unit(100,UnitType.Pixel);
        private Unit height = new Unit(20, UnitType.Pixel);

        public PercentageBar()
        {
        }

        [
        Browsable(true),
        Description("设置/获取百分比"),
        Category("Misc"),
        DefaultValue("0"),
        ]
        public decimal Percent
        {
            get
            {
                return percent;
            }
            set
            {
                percent = value;
            }
        }

        [
        Browsable(true),
        Description("设置/获取宽度"),
        Category("Misc"),
        DefaultValue("100"),
        ]
        public override Unit Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        [
        Browsable(true),
        Description("设置/获取高度"),
        Category("Misc"),
        DefaultValue("20"),
        ]
        public override Unit Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Img;
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute("src", string.Format("{0}?percent={1}&width={2}&height={3}", this.ResolveClientUrl("~/PercentageBarHandler.axd"), Percent,Width.Value,Height.Value));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["percent"] == "")
            {
                throw new ArgumentNullException("percent");
            }
            else
            {
                decimal percent = decimal.Parse(context.Request["percent"]);
                int width = int.Parse(context.Request["width"]);
                int height = int.Parse(context.Request["height"]);
                using (System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height))
                {
                    Graphics g = Graphics.FromImage(image);
                    g.DrawRectangle(new Pen(Color.Black, 1), 0, 0, width, height);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 1, width-1, height-2);
                    g.FillRectangle(new SolidBrush(Color.Blue), 0, 1, (int)(percent*width), height-2);
                    g.DrawString(string.Format("{0}%", (int)(percent*100)), new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Red), width/2-12, height/2-8);
                    g.Dispose();
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
                    context.Response.ClearContent();
                    context.Response.ContentType = "image/Gif";
                    context.Response.BinaryWrite(stream.ToArray());
                }
                context.Response.End();
            }
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
