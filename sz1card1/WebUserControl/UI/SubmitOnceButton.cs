using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace sz1card1.Common.UI
{
    public class SubmitOnceButton: System.Web.UI.WebControls.Button
    {
        /// <summary>
        /// 默认的构造函数。
        /// </summary>
        public SubmitOnceButton()
        {
            this.ViewState["afterSubmitText"] = "正在处理...";
            base.Text = "提交";
            this.ViewState["showMessageBox"] = false;
            this.ViewState["warningText"] = "确定要提交吗?";
        }

        /// <summary>
        /// 获取或设置单击按钮后，按钮上所显示的文本。
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("正在处理..."), Description("指示单击提交后，按钮上所显示的文本")]
        public string AfterSubmitText
        {
            get
            {
                string afterSubmitText = (string)this.ViewState["afterSubmitText"];
                if (afterSubmitText != null)
                {
                    return afterSubmitText;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                this.ViewState["afterSubmitText"] = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(false), Description("指示是否要显示一个提示框")]
        public bool ShowMessageBox
        {
            get
            {
                return (bool)this.ViewState["showMessageBox"];
            }
            set
            {
                this.ViewState["showMessageBox"] = value;
            }
        }


        [Bindable(true), Category("Appearance"), DefaultValue("确定要提交吗?"), Description("指示提示框内所包含的内容")]
        public string WarningText
        {
            get
            {
                return (string)this.ViewState["warningText"];
            }
            set
            {
                this.ViewState["warningText"] = value;
            }
        }

        /// <summary>
        /// AddAttributesToRender
        /// </summary>
        /// <param name="writer">HtmlTextWriter</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            System.Text.StringBuilder ClientSideEventReference = new System.Text.StringBuilder();
            string clientClick = this.OnClientClick;
            if (!string.IsNullOrEmpty(clientClick))
            {
                clientClick += ";";
            }
            if (((this.Page != null) && this.CausesValidation) && (this.Page.Validators.Count > 0))
            {
                ClientSideEventReference.Append(clientClick + "if (typeof(Page_ClientValidate) == 'function'){if (Page_ClientValidate(" + "\"" + this.ValidationGroup + "\"" + ") == false){return false;}}");
            }
            else
            {
                if (!string.IsNullOrEmpty(clientClick))
                {
                    ClientSideEventReference.Append(clientClick);
                }
            }

            if (this.ShowMessageBox)
            {
                ClientSideEventReference.Append("if (!confirm('" + this.WarningText + "')){return false}");
            }
            ClientSideEventReference.AppendFormat("this.value = '{0}';", (string)this.ViewState["afterSubmitText"]);
            ClientSideEventReference.Append("this.disabled = true;");
            ClientSideEventReference.Append(this.Page.ClientScript.GetPostBackEventReference(this, string.Empty));

            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, ClientSideEventReference.ToString(), true);
            base.AddAttributesToRender(writer);
        }
    }
}
