using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

[assembly: WebResource("sz1card1.Common.UI.Resources.PopupDiv.js", "text/javascript")]
namespace sz1card1.Common.UI
{
    [
        ToolboxData("<{0}:PopupDiv runat=\"server\"></{0}:PopupDiv>"),
        Designer(typeof(SubContainerControlDesigner)),
        ParseChildren(false),
        PersistChildren(true)
    ]
    public class PopupDiv : WebControl
    {
        private Unit width = new Unit(240, UnitType.Pixel);
        private Unit height = new Unit(240, UnitType.Pixel);
        private string title = "明细";
        private bool isModal = true;
        private string message = "";
        private string imagePath = "~/Images/Dialog";

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

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

        [
        Browsable(true),
        Description("获取或设置窗体标题")
        ]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        [
        Browsable(true),
        Description("获取或设置是否灰化背景")
        ]
        public bool IsModal
        {
            get
            {
                return isModal;
            }
            set
            {
                isModal = value;
            }
        }

        [
        Browsable(true),
        Description("获取或设置提示信息")
        ]
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        [
        Browsable(true),
        Description("获取或设置图片路径")
        ]
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("PopupDiv"))
            {
                Page.ClientScript.RegisterClientScriptInclude("PopupDiv", Page.ClientScript.GetWebResourceUrl(this.GetType(), "sz1card1.Common.UI.Resources.PopupDiv.js"));
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "drag_" + ID, "window.Drag.init($('_draghandle__" + ID + "'), $('" + ID + "'));", true);
            base.OnPreRender(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (isModal)
            {
                writer.Write(string.Format("<div id='{0}bg' style='display:none;background-color:#cccccc; position: absolute; left: 0px; top: 0px; opacity:0.6;filter:ALPHA(opacity=60); width: 100%; height:100%; z-index: 960;'></div>\n", ID));
            }
            base.Render(writer);
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute("style", string.Format("position: absolute; display: none; z-index: 992; left: 0px; top: 0px;width:{0}px;height:{1}px", Width.Value, Height.Value));
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write("<table cellspacing='0' cellpadding='0' width='100%' border='0' onselectstart='stopEvent(event);' oncontextmenu='stopEvent(event);' style='-moz-user-select: none;height:100%'>");
            writer.Write("<tbody>");
            writer.Write(string.Format(@"<tr id='_draghandle__{0}' style='cursor: move;'>
                            <td height='33' width='13' style=""background-image: url({1}/dialog_lt.png) ! important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{1}/dialog_lt.png', sizingMethod='crop');"">
                                <div style='width: 13px;' />
                            </td>
                            <td height='33' style=""background-image: url({1}/dialog_ct.png) ! important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{1}/dialog_ct.png', sizingMethod='crop');"">
                                <div style='padding: 9px 0pt 0pt 4px; float: left; font-weight: bold; color: rgb(255, 255, 255);'>
                                    <img align='absmiddle' src='{1}/icon_dialog.gif' />
                                   <span id='span" + ID + "'>" + Title + @"</span></div>
                                <div drag='false' style='margin: 5px 0pt 0pt; position: relative; cursor: pointer; float: right;
                                    height: 17px; width: 28px; background-image: url({1}/dialog_closebtn.gif);' onclick=""PopupDiv.close('{0}');"" />
                            </td>
                            <td height='33' width='13' style=""background-image: url({1}/dialog_rt.png) ! important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{1}/dialog_rt.png', sizingMethod='crop');"">
                                <div style='width: 13px;' />
                            </td>
                           </tr>", ID, ResolveClientUrl(ImagePath)));
            writer.Write(string.Format(@"<tr drag='false' style='height:100%'>
                            <td width='13' style=""background-image: url({0}/dialog_mlm.png) ! important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}/dialog_mlm.png', sizingMethod='crop');"" />
                            <td valign='top' align='center'>
                            <table cellspacing='0' cellpadding='0' width='{1}' border='0' style='height:100%;background-color:White'>
                                <tbody>", ResolveClientUrl(ImagePath), Width.Value));
            if (Message != "")
            {
                writer.Write(string.Format(@" <tr style='height:30px'>
                                    <td>
                                        <table cellspacing='0' cellpadding='4' width='100%' border='0' style='background: rgb(234, 236, 233) url({0}/dialog_bg.jpg) no-repeat scroll right top;
                                            -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;'>
                                            <tbody>
                                                <tr>
                                                    <td height='30' width='25' align='right'>
                                                        <img height='16' width='16' src='{0}/window.gif' />
                                                    </td>
                                                    <td align='left' style='line-height: 14px;'>
                                                            {1}
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>", ResolveClientUrl(ImagePath), Message));
            }
            writer.Write(string.Format(@"     <tr style='height:{0}px !important;vertical-align:top'>
                                    <td valign='top' align='center' id='_{1}__content'>", Height.Value, ID));
            base.RenderContents(writer);
            writer.Write(@"         </td>
                                </tr>");
            writer.Write(string.Format(@"     </tbody>
                            </table>
                        </td>
                        <td width='13' style=""background-image: url({0}/dialog_mrm.png) ! important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}/dialog_mrm.png', sizingMethod='crop');"" />
                    </tr>
                    <tr>
                        <td height='13' width='13' style=""background-image: url({0}/dialog_lb.png) ! important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}/dialog_lb.png', sizingMethod='crop');"" />
                        <td style=""background-image: url({0}/dialog_cb.png) ! important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}/dialog_cb.png', sizingMethod='crop');"" />
                        <td height='13' width='13' style=""background-image: url({0}/dialog_rb.png)!important;background-image: none;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}/dialog_rb.png', sizingMethod='crop');"" />
                    </tr>
                </tbody>
            </table>", ResolveClientUrl(ImagePath)));
        }

        /// <summary>
        /// 呈现当前内容到弹出层
        /// </summary>
        public void Show()
        {
            if (!Page.ClientScript.IsStartupScriptRegistered("show"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "show_" + ID, "PopupDiv.show('" + ID + "');", true);
            }
        }

        /// <summary>
        /// 呈现指定页面到弹出层
        /// </summary>
        /// <param name="url">页面地址</param>
        public void ShowDialog(string url)
        {
            if (!Page.ClientScript.IsStartupScriptRegistered("showDialog"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showDialog_" + ID, string.Format("PopupDiv.showDialog('{0}','{1}');", ID, url), true);
            }
        }
    }
}