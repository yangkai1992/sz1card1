using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using sz1card1.Common;
using sz1card1.Common.Communication;

[assembly: WebResource("sz1card1.Common.JQuery.Resources.json.js", "text/javascript")]
namespace sz1card1.Common.JQuery
{
    public abstract class BaseAjaxControl<T> : Control, ICallbackEventHandler, INamingContainer where T : class
    {
        private bool enabled = true;
        public event EventHandler<AjaxEventArgs<T>> AjaxRequest;
        private AjaxEventArgs<T> args = new AjaxEventArgs<T>();

        [Browsable(true)]
        [Description("按钮文本")]
        public string Text
        {
            get;
            set;
        }

        [Browsable(true)]
        [Description("点击按钮脚本")]
        public string RequestScript
        {
            get;
            set;
        }

        [Description("请求服务器前执行脚本")]
        protected  virtual  string CallingScript
        {
            get {return "" ;}
        }

        [Description("请求服务器后执行脚本")]
        protected  virtual string CalledScript
        {
            get{return "";}
        }
        //protected override bool UseSubmitBehavior
        //{
        //    get { return true; }
        //}

        [Browsable(true)]
        [Description("回调执行后响应脚本")]
        public string CallbackScript
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [Description("是否启用该控件")]
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        protected bool EnableCallServer
        {
            get
            {
                return AjaxRequest != null;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("json"))
            {
                Page.ClientScript.RegisterClientScriptInclude("json", Page.ClientScript.GetWebResourceUrl(this.GetType(), "sz1card1.Common.JQuery.Resources.json.js"));
            }
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ajaxclick"))
            {
                StringBuilder clickScript = new StringBuilder();
                clickScript.AppendLine("function AjaxControlRequest(opts){");
                clickScript.AppendLine("\tvar obj={};");
                clickScript.AppendLine("\tif(opts.request!=null){");
                clickScript.AppendLine("\t\tobj=opts.request();");
                clickScript.AppendLine("\t}");
                clickScript.AppendLine("\tif(obj!==false){");
                clickScript.AppendLine(string.Format("\t{0}", CallingScript));
                clickScript.AppendLine("\tvar json=sz1card1.stringify(obj);");
                clickScript.AppendLine("\t" + Page.ClientScript.GetCallbackEventReference("opts.id", "json", "AjaxControlCallBack", "opts", "null", true) + ";");
                clickScript.AppendLine("\t}");
                clickScript.AppendLine("}");
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ajaxclick", clickScript.ToString(), true);
            }
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ajaxcallback"))
            {
                StringBuilder callbackScript = new StringBuilder();
                callbackScript.AppendLine("function AjaxControlCallBack(result,opts){");
                callbackScript.AppendLine(string.Format("\t{0}", CalledScript));
                callbackScript.AppendLine("\tif(opts.callback!=null){");
                callbackScript.AppendLine("\t\tvar obj=sz1card1.parse(result);");
                callbackScript.AppendLine("\t\topts.callback(obj);");
                callbackScript.AppendLine("\t}");
                callbackScript.AppendLine("}");
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ajaxcallback", callbackScript.ToString(), true);
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered("ajaxfix"))
            {
                string fixScript = @"function WebForm_CallbackComplete_SyncFixed() {
                                           for (var i = 0; i < __pendingCallbacks.length; i++) {
                                                callbackObject = __pendingCallbacks[ i ];
                                                 if (callbackObject && callbackObject.xmlRequest && (callbackObject.xmlRequest.readyState == 1))
                                                {
                                                    try{ callbackObject.xmlRequest.setRequestHeader&&callbackObject.xmlRequest.setRequestHeader('X-Requested-With', 'XMLHttpRequest');}  catch(e) {}
                                                 }
                                                if (callbackObject && callbackObject.xmlRequest && (callbackObject.xmlRequest.readyState == 4)) {
                                                     if (!__pendingCallbacks[ i ].async) {
                                                         __synchronousCallBackIndex = -1;
                                                     }
                                                     if(callbackObject.xmlRequest.status == 300){
                                                          window.location.href = callbackObject.xmlRequest.getResponseHeader('Location');
                                                          return;
                                                     }
                                             __pendingCallbacks[i] = null;

                                               var callbackFrameID = '__CALLBACKFRAME' + i;
                                               var xmlRequestFrame = document.getElementById(callbackFrameID);
                                               if (xmlRequestFrame) {
                                                 xmlRequestFrame.parentNode.removeChild(xmlRequestFrame);
                                               }

                                               WebForm_ExecuteCallback(callbackObject);
                                              }
                                            }
                                          }";
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ajaxfix", fixScript, true);

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ajaxfixup", "if (typeof (WebForm_CallbackComplete) == 'function') {WebForm_CallbackComplete = WebForm_CallbackComplete_SyncFixed;}",true);
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered("ajaxfix2"))
            {
                string fix2Script = @"function WebForm_ExecuteCallback_SyncFixed(callbackObject) {
                                            var response = callbackObject.xmlRequest.responseText;
                                            if (response.charAt(0) == 's') {
                                                if ((typeof(callbackObject.eventCallback) != 'undefined') && (callbackObject.eventCallback != null)) {
                                                    callbackObject.eventCallback(response.substring(1), callbackObject.context);
                                                }
                                            }
                                            else if (response.charAt(0) == 'e') {
                                                if ((typeof(callbackObject.errorCallback) != 'undefined') && (callbackObject.errorCallback != null)) {
                                                    callbackObject.errorCallback(response.substring(1), callbackObject.context);
                                                }
                                                else{
                                                     console.log(response.substring(1));
//                                                     alert('服务器繁忙，请稍后重试！');
//                                                     self.location.reload();                                                 
                                                }
                                            }
                                            else {
                                                var separatorIndex = response.indexOf('|');
                                                if (separatorIndex != -1) {
                                                    var validationFieldLength = parseInt(response.substring(0, separatorIndex));
                                                    if (!isNaN(validationFieldLength)) {
                                                        var validationField = response.substring(separatorIndex + 1, separatorIndex + validationFieldLength + 1);
                                                        if (validationField != '') {
                                                            var validationFieldElement = theForm['__EVENTVALIDATION'];
                                                            if (!validationFieldElement) {
                                                                validationFieldElement = document.createElement('INPUT');
                                                                validationFieldElement.type = 'hidden';
                                                                validationFieldElement.name = '__EVENTVALIDATION';
                                                                theForm.appendChild(validationFieldElement);
                                                            }
                                                            validationFieldElement.value = validationField;
                                                        }
                                                        if ((typeof(callbackObject.eventCallback) != 'undefined') && (callbackObject.eventCallback != null)) {
                                                            callbackObject.eventCallback(response.substring(separatorIndex + validationFieldLength + 1), callbackObject.context);
                                                        }
                                                    }
                                                }
                                            }
                                        }";
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ajaxfix2", fix2Script, true);

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ajaxfixup2", "if (typeof (WebForm_ExecuteCallback) == 'function') {WebForm_ExecuteCallback = WebForm_ExecuteCallback_SyncFixed;}", true);
            }

        }

        public virtual void OnAjaxRequest(Object sender, AjaxEventArgs<T> e)
        {
            if (AjaxRequest != null)
                AjaxRequest(sender, e);
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            args.Request = eventArgument.FromJson<T>();
            OnAjaxRequest(this, args);
        }

        public string GetCallbackResult()
        {
            return args.Response.ToJson();
        }
    }

    [ToolboxData("<{0}:AjaxControl runat=\"server\"/>")]
    public class AjaxControl : BaseAjaxControl<Dictionary<string, string>>
    {
        protected override string CallingScript
        {
            get
            {
                return "";
            }
        }

        protected override string CalledScript
        {
            get
            {
                return "";
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            //不输入任何内容
        }
    }
}
