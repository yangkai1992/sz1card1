using System;
using System.Collections.Specialized;
using System.Linq;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using WebUserControl.Enum;

namespace WebUserControl.UI
{
    [ValidationProperty("SelectedValue")]
    public class XmlDropDownList : WebControl, IPostBackDataHandler
    {
        public event EventHandler SelectedValueChanged;

        [Browsable(true)]
        [Category("数据")]
        [Description("获取或设置Xml数据文件相对路径。")]
        public string XmlUrl
        {
            get
            {
                if (ViewState["XmlUrl"] == null)
                    ViewState.Add("XmlUrl", string.Empty);
                return (string)ViewState["XmlUrl"];
            }
            set
            {
                ViewState["XmlUrl"] = value;
            }
        }

        [Browsable(true)]
        [Category("数据")]
        [Description("获取或设置Xml预定义类型。")]
        [DefaultValue(XmlTypes.None)]
        public XmlTypes XmlType
        {
            get
            {
                if (ViewState["XmlType"] == null)
                    ViewState.Add("XmlType", XmlTypes.None);
                return (XmlTypes)ViewState["XmlType"];
            }
            set
            {
                ViewState["XmlType"] = value;
            }
        }

        [Browsable(true)]
        [Category("数据")]
        [Description("获取或设置为列表项提供文本内容的数据源字段。")]
        [DefaultValue("Name")]
        public string DataTextField
        {
            get
            {
                if (ViewState["DataTextField"] == null)
                    ViewState.Add("DataTextField", "Name");
                return (string)ViewState["DataTextField"];
            }
            set
            {
                ViewState["DataTextField"] = value;
            }
        }

        [Browsable(true)]
        [Category("数据")]
        [Description("获取或设置为各列表项提供值的数据源字段。")]
        [DefaultValue("ID")]
        public string DataValueField
        {
            get
            {
                if (ViewState["DataValueField"] == null)
                    ViewState.Add("DataValueField", "ID");
                return (string)ViewState["DataValueField"];
            }
            set
            {
                ViewState["DataValueField"] = value;
            }
        }

        [Browsable(true)]
        [Category("数据")]
        [Description("获取列表控件中选定项的值，或选择列表控件中包含指定值的项。")]
        public string SelectedValue
        {
            get
            {
                if (ViewState["SelectedValue"] == null)
                    ViewState.Add("SelectedValue", string.Empty);
                return (string)ViewState["SelectedValue"];
            }
            set
            {
                ViewState["SelectedValue"] = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Select;
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute("name", this.UniqueID);    
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);
            writer.Write("<option value=''>请选择...</option>");
            if (!DesignMode)
            {
                StreamReader reader;
                if (XmlType != XmlTypes.None)
                {
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("WebUserControl.Data.{0}.xml", XmlType));
                    reader = new StreamReader(stream);
                }
                else
                {
                    if (XmlUrl != string.Empty)
                    {
                        reader = new StreamReader(HttpContext.Current.Server.MapPath(XmlUrl));
                    }
                    else
                    {
                        throw new ArgumentNullException("XmlUrl");
                    }
                }

                var items = from p in XElement.Load((TextReader)reader).Elements()
                            select new
                            {
                                Text = p.Element(DataTextField).Value,
                                Value = p.Element(DataValueField).Value
                            };
                foreach (var item in items)
                {
                    if (item.Value == SelectedValue)
                    {
                        writer.Write(string.Format("<option selected='selected' value='{0}'>{1}</option>", item.Value, item.Text));
                    }
                    else
                    {
                        writer.Write(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Text));
                    }
                }
                reader.Dispose();
            }
        }

        public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string[] values = postCollection.GetValues(postDataKey);
            if (values != null && SelectedValue != values[0])
            {
                SelectedValue = values[0];
                return true;
            }
            return false;
        }

        public virtual void RaisePostDataChangedEvent()
        {
            if (SelectedValueChanged != null)
            {
                SelectedValueChanged(this, EventArgs.Empty);
            }
        }
    }
}