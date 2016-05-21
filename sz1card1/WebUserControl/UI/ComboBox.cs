/* Version: 1.2.0.0
 * 
 * [summary]:
 *   - it is an AJAX server control.
 *   - it can be rendered as a dropdownlist or combobox.
 *   - it fully supports custoized stylesheet.
 *   - it supports auto postback event.
 *   - it can be used associated with ASP.NET UpdatePanel.
 *   - it can search and select item when user is typing on the dropdownlist.
 *   - it provides a lot of client-side javascript functions to control itself.
 * 
 * [pre-requirement]:
 *   - .NET Framework: 2.0
 *   - as an AJAX server control, ASP.NET ScriptManager must be included in your page.
 * 
 * [how to]:
 *   - add items to it.
 *     the same as ASP.NET DropDownList. (<asp:ListItem...>, DataBind(), etc.)
 *   - change it to dropdownlist or combobox.
 *     using the "RenderMode" property.
 *   - change the scroll behavior.
 *     using the "ScrollArrowStepLength" and "ScrollArrowStepInterval" property.
 *   - change the width of the popup dropdownlist.
 *     using the "OffsetWidth" property.
 *   - change the position of the dropdownlist.
 *     using the "OffsetX" and "OffsetY" property.
 *   - change the maximum height of the popup dropdownlist.
 *     using the "MaxHeight" property. (scroll bar will be displayed if its height exceeds the "MaxHeight")
 *   - change the appearance.
 *     using the "StyleSheetUrl" and "CssClass" property. (see also: [help] in ComboBox.css file)
 *   - subscribe OnChange event from server-side.
 *     using the "ComboBoxChanged" event.
 *   - execute your script from client-side when selected index or text changed.
 *     using the "OnClientChange" property.
 *   - control it from client-side.
 *     using the client-side javascript function. (see also: [help] in ComboBox.js file)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;

#region [ web resource ]

[assembly: System.Web.UI.WebResource("WebUserControl.UI.Resources.Common.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("WebUserControl.UI.Resources.ComboBox.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("WebUserControl.UI.Resources.ComboBox.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("WebUserControl.UI.Resources.ComboBox_arrow_down.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("WebUserControl.UI.Resources.ComboBox_arrow_up.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("WebUserControl.UI.Resources.ComboBox_scrollbar.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("WebUserControl.UI.Resources.ComboBox_arrow_down_disabled.gif", "image/gif")]

#endregion

namespace WebUserControl.UI
{
    [ToolboxBitmap(typeof(ComboBox), "WebUserControl.UI.Resources.ComboBox.ico")]
    [ToolboxData("<{0}:ComboBox runat=server></{0}:ComboBox>"), DefaultProperty("RenderMode"), DefaultEvent("ComboBoxChanged")]
    public class ComboBox : ListControl, INamingContainer, IScriptControl, IPostBackDataHandler, IPostBackEventHandler
    {
        #region [ constant ]

        private const string _textBoxStagingId = "txtStaging";
        private const string _textBoxTextId = "txtText";
        private const string _buttonDropDownId = "btnDropDown";
        private const string _divPopupId = "divPopup";
        private const string _divItemsContainerId = "divItemsContainer";
        private const string _tableItemsId = "tblItems";
        private const string _divScrollId = "divScroll";
        private const string _divArrowUpId = "divArrowUp";
        private const string _divArrowDownId = "divArrowDown";
        private const string _divScrollBarId = "divScrollBar";
        private const string _buttonPostBack = "btnPostBack";

        #endregion

        #region [ property ]

        [DefaultValue(0), Category("Behavior"), Description("Indicates whether this control will be rendered as a dropdownlist or a combobox.")]
        public ComboBoxMode RenderMode
        {
            get
            {
                return ViewState["RenderMode"] == null ? ComboBoxMode.DropDownList : (ComboBoxMode)ViewState["RenderMode"];
            }
            set
            {
                ViewState["RenderMode"] = value;
            }
        }

        [DefaultValue(1), Category("Behavior"), Description("Indicates the length (in pixel) of each step that the scroll bar will move when you click the scroll arrow button.")]
        public int ScrollArrowStepLength
        {
            get
            {
                return ViewState["ScrollArrowStepLength"] == null ? 1 : (int)ViewState["ScrollArrowStepLength"];
            }
            set
            {
                ViewState["ScrollArrowStepLength"] = value;
            }
        }

        [DefaultValue(1), Category("Behavior"), Description("Indicates the interval (in millisecond) of each step that the scroll bar will move when you click the scroll arrow button.")]
        public int ScrollArrowStepInterval
        {
            get
            {
                return ViewState["ScrollArrowStepInterval"] == null ? 1 : (int)ViewState["ScrollArrowStepInterval"];
            }
            set
            {
                ViewState["ScrollArrowStepInterval"] = value;
            }
        }

        [DefaultValue(""), Category("Behavior"), Description("The client-side script that is executed on a client-side OnChange.")]
        public virtual string OnClientChange
        {
            get
            {
                return ViewState["OnClientChange"] == null ? string.Empty : (string)ViewState["OnClientChange"];
            }
            set
            {
                ViewState["OnClientChange"] = value;
            }
        }

        [DefaultValue(false), Category("Behavior"), Description("Whether the text in the ComboBox can be changed or not.")]
        public virtual bool ReadOnly
        {
            get
            {
                return ViewState["ReadOnly"] == null ? false : (bool)ViewState["ReadOnly"];
            }
            set
            {
                ViewState["ReadOnly"] = value;
            }
        }

        [DefaultValue(0), Category("Layout"), Description("The offset width for the dropdownlist based on the Width property.")]
        public virtual int OffsetWidth
        {
            get
            {
                return ViewState["OffsetWidth"] == null ? 0 : (int)ViewState["OffsetWidth"];
            }
            set
            {
                ViewState["OffsetWidth"] = value;
            }
        }

        [DefaultValue(0), Category("Layout"), Description("The offset x-axis of the dropdownlist.")]
        public virtual int OffsetX
        {
            get
            {
                return ViewState["OffsetX"] == null ? 0 : (int)ViewState["OffsetX"];
            }
            set
            {
                ViewState["OffsetX"] = value;
            }
        }

        [DefaultValue(0), Category("Layout"), Description("The offset y-axis of the dropdownlist.")]
        public virtual int OffsetY
        {
            get
            {
                return ViewState["OffsetY"] == null ? 0 : (int)ViewState["OffsetY"];
            }
            set
            {
                ViewState["OffsetY"] = value;
            }
        }

        [DefaultValue(""), Category("Layout"), Description("Scroll bar will be applied once the height of dropdownlist reachs the MaxHeight")]
        public virtual Unit MaxHeight
        {
            get
            {
                return ViewState["MaxHeight"] == null ? Unit.Empty : (Unit)ViewState["MaxHeight"];
            }
            set
            {
                ViewState["MaxHeight"] = value;
            }
        }

        [DefaultValue("ComboBox_default"), Category("Appearance"), Description("CssClass.")]
        public override string CssClass
        {
            get
            {
                return string.IsNullOrEmpty(base.CssClass) ? "ComboBox_default" : base.CssClass;
            }
            set
            {
                base.CssClass = value;
            }
        }

        [Browsable(true), DefaultValue(""), Category("Appearance")]
        public override string Text
        {
            get
            {
                return ViewState["Text"] == null ? string.Empty : (string)ViewState["Text"];
            }
            set
            {
                ViewState["Text"] = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Description("You can sepecify your own style sheet instead of the default one.")]
        public virtual string StyleSheetUrl
        {
            get
            {
                return ViewState["StyleSheetUrl"] == null ? string.Empty : (string)ViewState["StyleSheetUrl"];
            }
            set
            {
                ViewState["StyleSheetUrl"] = value;
            }
        }

        #endregion

        #region [ render ]

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                // Display a dropdownlist when this control is in design mode.
                if (this.Width != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());
                if (this.Height != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Select);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                // A hidden text box which hosts the post back data.
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_textBoxStagingId));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_textBoxStagingId));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, this.SelectedIndex.ToString());
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();

                // A text box displays the selected text or user inputted text.
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_textBoxTextId));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_textBoxTextId));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                if (this.SelectedIndex > -1) writer.AddAttribute(HtmlTextWriterAttribute.Value, this.SelectedItem.Text);
                else if (this.Text != string.Empty) writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Text);
                if (this.Enabled)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "text_BCB86A76");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "text_disabled_BCB86A76");
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
                }
                if (this.RenderMode == ComboBoxMode.DropDownList)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "default");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("$find('{0}').show();", this.ClientID));
                }
                else
                {
                    if (this.ReadOnly) writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onchange, string.Format("$find('{0}').onChanged();", this.ClientID));
                    writer.AddAttribute("onfocus", string.Format("$find('{0}').hidePopup();", this.ClientID));
                }
                if (this.Width != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());
                if (this.Height != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();

                // A fake button (text box actuality) with a drop down arrow which is next to the text box
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_buttonDropDownId));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_buttonDropDownId));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                if (this.Enabled)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "arrow_BCB86A76");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "arrow_disabled_BCB86A76");
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
                }
                writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("$find('{0}').show();", this.ClientID));
                if (this.Height != Unit.Empty) writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_divPopupId));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_divPopupId));
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "popup_BCB86A76");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
                writer.AddAttribute("onselectstart", "return false;");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                {
                    // dropdownlist
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_divItemsContainerId));
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_divItemsContainerId));
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "container_BCB86A76");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_tableItemsId));
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_tableItemsId));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0px");
                        writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0px");
                        writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0px");
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                        if (this.Items.Count == 0) writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                        writer.RenderBeginTag(HtmlTextWriterTag.Table);
                        {
                            for (int i = 0; i < this.Items.Count; i++)
                            {
                                if (i == this.SelectedIndex) writer.AddAttribute(HtmlTextWriterAttribute.Class, "selected_BCB86A76");

                                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("$find('{0}').onSelected(this);", this.ClientID));

                                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                                {
                                    writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
                                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                    writer.Write(this.Items[i].Text);
                                    writer.RenderEndTag();

                                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                    writer.Write(this.Items[i].Value);
                                    writer.RenderEndTag();
                                }
                                writer.RenderEndTag();
                            }
                        }
                        writer.RenderEndTag();

                        // render an empty item when no items.
                        if (this.Items.Count == 0)
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0px");
                            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0px");
                            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0px");
                            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                            writer.RenderBeginTag(HtmlTextWriterTag.Table);
                            {
                                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                                {
                                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                    writer.Write("&nbsp;");
                                    writer.RenderEndTag();
                                }
                                writer.RenderEndTag();
                            }
                        }
                    }
                    writer.RenderEndTag();

                    // scroll bar
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_divScrollId));
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_divScrollId));
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "scroll_BCB86A76");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_divArrowUpId));
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_divArrowUpId));
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "arrow_up_BCB86A76");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.RenderEndTag();

                        writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_divArrowDownId));
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_divArrowDownId));
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "arrow_down_BCB86A76");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.RenderEndTag();

                        writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_divScrollBarId));
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_divScrollBarId));
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "scrollbar_BCB86A76");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.RenderEndTag();
                    }
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();

                string script = this.GetOnClientChangeScript();
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetClientID(_buttonPostBack));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.GetUniqueID(_buttonPostBack));
                if (script != string.Empty) writer.AddAttribute(HtmlTextWriterAttribute.Onclick, script);
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.RegisterCssReferences();
                ScriptManager.GetCurrent(this.Page).RegisterScriptControl(this);
                ScriptManager.GetCurrent(this.Page).RegisterScriptDescriptors(this);
                this.Page.RegisterRequiresPostBack(this);
            }

            if (this.SelectedIndex > -1) this.Text = this.SelectedItem.Text;

            base.OnPreRender(e);
        }

        protected virtual void RegisterCssReferences()
        {
            bool bRegistered = false;
            string styleSheetId = "WebUserControl.UI_ComboBox_StyleSheet_BCB86A76";
            if (this.StyleSheetUrl != string.Empty) styleSheetId += "_" + this.StyleSheetUrl.GetHashCode().ToString().Replace("-", string.Empty);

            foreach (Control ctrl in this.Page.Header.Controls)
                if (ctrl.ID == styleSheetId)
                    bRegistered = true;

            if (!bRegistered)
            {
                HtmlLink link = new HtmlLink();
                link.ID = styleSheetId;
                link.Attributes.Add("type", "text/css");
                link.Attributes.Add("rel", "stylesheet");
                if (this.StyleSheetUrl == string.Empty)
                {
                    link.Attributes.Add("href", this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "WebUserControl.UI.Resources.ComboBox.css"));
                }
                else
                {
                    link.Attributes.Add("href", this.StyleSheetUrl.Replace("~",
                        HttpContext.Current.Request.ApplicationPath.Length == 1 ? string.Empty : HttpContext.Current.Request.ApplicationPath));
                }
                this.Page.Header.Controls.Add(link);
            }
        }

        protected virtual string GetClientID(string id)
        {
            return string.Format("{0}_{1}", this.ClientID, id);
        }

        protected virtual string GetUniqueID(string id)
        {
            return string.Format("{0}${1}", this.ClientID, id);
        }

        protected virtual string GetOnClientChangeScript()
        {
            string script = string.Empty;

            script += this.EnsureEndWithSemiColon(this.OnClientChange);

            if (this.AutoPostBack) script += Page.ClientScript.GetPostBackEventReference(this, string.Empty);

            return script;
        }

        protected virtual string EnsureEndWithSemiColon(string value)
        {
            if (value != null)
            {
                int length = value.Length;
                if ((length > 0) && (value[length - 1] != ';'))
                {
                    return (value + ";");
                }
            }
            return value;
        }

        #endregion

        #region [ event ]

        [Category("Action"), Description("Fires when the selected index or the text box value has been changed.")]
        public event EventHandler ComboBoxChanged;

        protected virtual void OnComboBoxChanged(EventArgs e)
        {
            if (ComboBoxChanged != null)
            {
                ComboBoxChanged(this, e);
            }
        }

        #endregion

        #region IScriptControl Members

        public IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Babino.ComboBox", this.ClientID);
            descriptor.AddProperty("comboBoxId", this.ClientID);
            descriptor.AddProperty("textBoxStagingId", GetClientID(_textBoxStagingId));
            descriptor.AddProperty("textBoxTextId", GetClientID(_textBoxTextId));
            descriptor.AddProperty("buttonDropDownId", GetClientID(_buttonDropDownId));
            descriptor.AddProperty("divPopupId", GetClientID(_divPopupId));
            descriptor.AddProperty("divItemsContainerId", GetClientID(_divItemsContainerId));
            descriptor.AddProperty("tableItemsId", GetClientID(_tableItemsId));
            descriptor.AddProperty("divScrollId", GetClientID(_divScrollId));
            descriptor.AddProperty("divArrowUpId", GetClientID(_divArrowUpId));
            descriptor.AddProperty("divArrowDownId", GetClientID(_divArrowDownId));
            descriptor.AddProperty("divScrollBarId", GetClientID(_divScrollBarId));
            descriptor.AddProperty("buttonPostBackId", GetClientID(_buttonPostBack));
            descriptor.AddProperty("offsetX", this.OffsetX);
            descriptor.AddProperty("offsetY", this.OffsetY);
            descriptor.AddProperty("offsetWidth", this.OffsetWidth);
            descriptor.AddProperty("maxHeight", this.MaxHeight == Unit.Empty ? string.Empty : this.MaxHeight.Value.ToString());
            descriptor.AddProperty("selectedIndex", this.SelectedIndex);
            descriptor.AddProperty("scrollStepLen", this.ScrollArrowStepLength);
            descriptor.AddProperty("scrollStepInterval", this.ScrollArrowStepInterval);
            descriptor.AddProperty("renderMode", this.RenderMode.ToString());
            descriptor.AddProperty("enabled", this.Enabled);
            descriptor.AddProperty("readOnly", this.ReadOnly);

            yield return descriptor;
        }

        public IEnumerable<ScriptReference> GetScriptReferences()
        {
            ScriptReference[] references = new ScriptReference[2];

            references[0] = new ScriptReference("WebUserControl.UI.Resources.Common.js", this.GetType().Assembly.FullName);
            references[1] = new ScriptReference("WebUserControl.UI.Resources.ComboBox.js", this.GetType().Assembly.FullName);

            return references;
        }

        #endregion

        #region IPostBackDataHandler Members

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            bool raisePostDataChangedEvent = false;

            int index = int.Parse(postCollection[GetUniqueID(_textBoxStagingId)]);
            string text = postCollection[GetUniqueID(_textBoxTextId)];

            if (!this.AutoPostBack && (index != this.SelectedIndex || text != this.Text)) raisePostDataChangedEvent = true;

            this.SelectedIndex = index;
            if (this.SelectedIndex > -1)
                this.Text = this.SelectedItem.Text;
            else
                this.Text = text;

            return raisePostDataChangedEvent;
        }

        public void RaisePostDataChangedEvent()
        {
            this.OnComboBoxChanged(EventArgs.Empty);
        }

        #endregion

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            this.OnComboBoxChanged(EventArgs.Empty);
        }

        #endregion
    }

    public enum ComboBoxMode
    {
        DropDownList,
        ComboBox
    }
}
