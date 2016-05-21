using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Collections;
using WebUserControl.Common;

namespace WebUserControl.UI
{
    /// <summary>
    /// 省地市三级联动
    /// </summary>
    [
        ToolboxData("<{0}:RegionSelector runat=\"server\"></{0}:RegionSelector>"),
        ValidationProperty("CountyID")
    ]
    public class RegionSelector : WebControl, ICallbackEventHandler, IPostBackDataHandler
    {
        private string result;
        /// <summary>
        /// 构造方法
        /// </summary>
        public RegionSelector()
        {
        }

        /// <summary>
        /// 省份编号
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取省份编号"),
        Category("Misc"),
        DefaultValue(""),
        ]
        public string ProvinceID
        {
            get
            {
                if (ViewState["ProvinceID"] == null)
                    ViewState.Add("ProvinceID", "");
                return ViewState["ProvinceID"].ToString();
            }
            set
            {
                ViewState["ProvinceID"] = value;
            }
        }

        /// <summary>
        /// 地区编号
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取地市编号"),
        Category("Misc"),
        DefaultValue(""),
        ]
        public string CityID
        {
            get
            {
                if (ViewState["CityID"] == null)
                    ViewState.Add("CityID", "");
                return ViewState["CityID"].ToString();
            }
            set
            {
                ViewState["CityID"] = value;
            }
        }

        /// <summary>
        /// 县区编号
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取县区编号"),
        Category("Misc"),
        DefaultValue(""),
        ]
        public string CountyID
        {
            get
            {
                if (ViewState["CountyID"] == null)
                    ViewState.Add("CountyID", "");
                return ViewState["CountyID"].ToString();
            }
            set
            {
                ViewState["CountyID"] = value;

            }
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        [Browsable(true),
        Category("Appearance"),
        DefaultValue("false"), Description("设置/获取是否为只读")]
        public bool ReadOnly
        {
            get
            {
                if (ViewState["ReadOnly"] == null)
                {
                    ViewState.Add("ReadOnly", false);
                }
                return (bool)ViewState["ReadOnly"];
            }
            set { ViewState["ReadOnly"] = value; }
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue("false"), Description("设置/获取是否采用JQuery的下拉框形式")]
        public bool IsUsedJQuery
        {
            get;
            set;
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue("true"), Description("设置/获取是否采用Ajax的三级联动方式")]
        public bool IsAjaxMode
        {
            get;
            set;
        }

        /// <summary>
        /// 设置或获取省份改变时客户端事件
        /// </summary>
        [Browsable(true),
        Category("Misc"),
        DefaultValue(""), Description("设置或获取省份改变时客户端事件")]
        public string OnClientProvinceChanged
        {
            get
            {
                if (ViewState["OnClientProvinceChanged"] == null)
                    ViewState.Add("OnClientProvinceChanged", "");
                return ViewState["OnClientProvinceChanged"].ToString();
            }
            set
            {
                ViewState["OnClientProvinceChanged"] = value;
            }
        }

        /// <summary>
        /// 设置或获取城市改变时客户端事件
        /// </summary>
        [Browsable(true),
        Category("Misc"),
        DefaultValue(""), Description("设置或获取城市改变时客户端事件")]
        public string OnClientCityChanged
        {
            get
            {
                if (ViewState["OnClientCityChanged"] == null)
                    ViewState.Add("OnClientCityChanged", "");
                return ViewState["OnClientCityChanged"].ToString();
            }
            set
            {
                ViewState["OnClientCityChanged"] = value;
            }
        }

        /// <summary>
        /// 设置或获取县区改变时客户端事件
        /// </summary>
        [Browsable(true),
        Category("Misc"),
        DefaultValue(""), Description("设置或获取县区改变时客户端事件")]
        public string OnClientCountyChanged
        {
            get
            {
                if (ViewState["OnClientCountyChanged"] == null)
                    ViewState.Add("OnClientCountyChanged", "");
                return ViewState["OnClientCountyChanged"].ToString();
            }
            set
            {
                ViewState["OnClientCountyChanged"] = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Span;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresPostBack(this);
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute("value", CityID);
            base.AddAttributesToRender(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string script;
            if (!Page.ClientScript.IsClientScriptBlockRegistered("regionselect"))
            {
                if (IsAjaxMode)
                {
                    script = "function regionSelect(obj){\n";
                    script += "var para;\n";
                    script += string.Format("if(obj.id=='{0}_Province'){1}\n", ClientID, "{");
                    if (OnClientProvinceChanged != "")
                    {
                        script += OnClientProvinceChanged + "\n";
                    }
                    script += "para='Province@'+obj.value;\n";
                    script += Page.ClientScript.GetCallbackEventReference(this, "para", "regionSelectCallback", "null");
                    script += "}\n";
                    script += string.Format("else if(obj.id=='{0}_City'){1}\n", ClientID, "{");
                    if (OnClientCityChanged != "")
                    {
                        script += OnClientCityChanged + "\n";
                    }
                    script += "para='City@'+obj.value;\n";
                    script += Page.ClientScript.GetCallbackEventReference(this, "para", "regionSelectCallback", "null");
                    script += "}\n";
                    script += "else{\n";
                    script += string.Format("document.getElementById('{0}').value=obj.value;", ClientID);
                    if (OnClientCountyChanged != "")
                    {
                        script += OnClientCountyChanged + "\n";
                    }
                    script += "}\n";
                    script += "}\n";
                }
                else
                {
                    script = string.Format("var provinces=eval('{0}');\n", GetAllProvice().ToJson());
                    script += string.Format("var cities=eval('{0}');\n", GetAllCity().ToJson());
                    script += string.Format("var counties=eval('{0}');\n", GetAllCounty().ToJson());
                    script += "function regionSelect(obj){\n";
                    script += string.Format("if(obj.id=='{0}_Province'){1}\n", ClientID, "{");
                    script += string.Format("document.getElementById('{0}_City').length=0;\n", ClientID);
                    script += string.Format("var defaultItem=new Option('请选择...','');\n");
                    script += string.Format("document.getElementById('{0}_City').options.add(defaultItem);\n", ClientID);
                    script += string.Format("document.getElementById('{0}_County').length=0;\n", ClientID);
                    script += string.Format("var defaultItem2=new Option('请选择...','');\n");
                    script += string.Format("document.getElementById('{0}_County').options.add(defaultItem2);\n", ClientID);
                    script += string.Format("document.getElementById('{0}').value='';", ClientID);
                    script += string.Format("var provinceId=document.getElementById('{0}_Province').value;\n", ClientID);
                    script += "var cityItems;\n";
                    script += string.Format("for(var i=0;i<cities.length;i++){0}\n", "{");
                    script += string.Format("if(cities[i].pid==provinceId){0}\n", "{");
                    script += "cityItems=cities[i].cities;\n";
                    script += "break;\n";
                    script += "}\n";
                    script += "}\n";
                    script += string.Format("for(var i=0;i<cityItems.length;i++){0}\n", "{");
                    script += string.Format("document.getElementById('{0}_City').options.add(new Option(cityItems[i].name,cityItems[i].id));\n", ClientID);
                    script += "}\n";
                    if (OnClientProvinceChanged != "")
                    {
                        script += OnClientProvinceChanged + "\n";
                    }
                    script += "}\n";
                    script += string.Format("else if(obj.id=='{0}_City'){1}\n", ClientID, "{");
                    script += string.Format("document.getElementById('{0}_County').length=0;\n", ClientID);
                    script += string.Format("var defaultItem=new Option('请选择...','');\n");
                    script += string.Format("document.getElementById('{0}_County').options.add(defaultItem);\n", ClientID);
                    script += string.Format("document.getElementById('{0}').value='';", ClientID);
                    script += string.Format("var cityId=document.getElementById('{0}_City').value;\n", ClientID);
                    script += "var countyItems;\n";
                    script += string.Format("for(var i=0;i<counties.length;i++){0}\n", "{");
                    script += string.Format("if(counties[i].cid==cityId){0}\n", "{");
                    script += "countyItems=counties[i].counties;\n";
                    script += "break;\n";
                    script += "}\n";
                    script += "}\n";
                    script += string.Format("for(var i=0;i<countyItems.length;i++){0}\n", "{");
                    script += string.Format("document.getElementById('{0}_County').options.add(new Option(countyItems[i].name,countyItems[i].id));\n", ClientID);
                    script += "}\n";
                    if (OnClientCityChanged != "")
                    {
                        script += OnClientCityChanged + "\n";
                    }
                    script += "}\n";
                    script += "else{\n";
                    script += string.Format("document.getElementById('{0}').value=document.getElementById('{0}_County').value;", ClientID);
                    if (OnClientCountyChanged != "")
                    {
                        script += OnClientCountyChanged + "\n";
                    }
                    script += "}\n";
                    script += "}\n";
                }
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "regionSelect", script, true);
            }
            if (IsAjaxMode)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("regionselectcallback"))
                {

                    script = "function regionSelectCallback(result){\n";
                    script += "var arr=result.split('@');\n";
                    script += "document.getElementById(arr[0]).outerHTML=arr[1];\n";
                    script += "if(arr[0].indexOf('City')>-1){\n";
                    script += string.Format("document.getElementById('{0}_County').options.length=0;\n", ClientID);
                    script += "var op=new Option();\n";
                    script += "op.text='请选择...';\n";
                    script += "op.value='';\n";
                    script += string.Format("document.getElementById('{0}_County').options[0]=op;\n", ClientID);
                    script += string.Format("document.getElementById('{0}').value='';", ClientID);
                    script += "}\n";
                    script += "}\n";
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "regionselectcallback", script, true);
                }
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);
            writer.Write(GetProvinceHtml());
            writer.Write("&nbsp;");
            writer.Write(GetCityHtml());
            writer.Write("&nbsp;");
            writer.Write(GetCountyHtml());
        }

        private IEnumerable GetAllProvice()
        {
            return DataUtil.GetProvinces();
        }

        private IEnumerable GetAllCity()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebUserControl.Data.Region.xml"))
            {
                TextReader reader = new StreamReader(stream);
                var allItems = from e in XElement.Load(reader).Elements().Elements()
                               select new
                               {
                                   id = e.Attribute("id").Value,
                                   name = e.Attribute("name").Value,
                                   pid = e.Parent.Attribute("id").Value
                               };
                var items = from p in allItems
                            group p by p.pid into g
                            select new
                            {
                                pid = g.Key,
                                cities = from c in allItems
                                         where c.pid == g.Key
                                         select new
                                         {
                                             id = c.id,
                                             name = c.name
                                         }
                            };
                return items;
            }
        }

        private IEnumerable GetAllCounty()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebUserControl.Data.Region.xml"))
            {
                TextReader reader = new StreamReader(stream);
                var allItems = from p in XElement.Load(reader).Elements().Elements().Elements()
                               select new
                               {
                                   id = p.Attribute("id").Value,
                                   name = p.Attribute("name").Value,
                                   cid = p.Parent.Attribute("id").Value
                               };
                var items = from p in allItems
                            group p by p.cid into g
                            select new
                            {
                                cid = g.Key,
                                counties = from c in allItems
                                           where c.cid == g.Key
                                           select new
                                           {
                                               id = c.id,
                                               name = c.name
                                           }
                            };
                return items;
            }
        }

        /// <summary>
        /// 输出省份html
        /// </summary>
        /// <returns></returns>
        private string GetProvinceHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<select id=\"{0}_Province\" name=\"{0}$Province\" {1} onchange=\"regionSelect(this);\"{2}>", this.ClientID, ReadOnly ? "disabled=\"disabled\"" : "", IsUsedJQuery ? " class=\"default\"" : ""));
            sb.Append("<option value=\"\">请选择...</option>");
            if (!DesignMode)
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebUserControl.Data.Province.xml"))
                {
                    TextReader reader = new StreamReader(stream);
                    var items = from p in XElement.Load(reader).Elements()
                                select new
                                {
                                    Text = p.Element("Name").Value,
                                    Value = p.Element("ID").Value
                                };
                    foreach (var item in items)
                    {
                        if (!Page.IsCallback)
                        {
                            if (item.Value == ProvinceID)
                            {
                                sb.Append(string.Format("<option selected='selected' value='{0}'>{1}</option>", item.Value, item.Text));
                            }
                            else
                            {
                                sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Text));
                            }
                        }
                        else
                        {
                            sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Text));
                        }
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 输出城市html
        /// </summary>
        /// <returns></returns>
        private string GetCityHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<select id=\"{0}_City\" name=\"{0}$City\" {1} onchange=\"regionSelect(this);\"{2}>", this.ClientID, ReadOnly ? "disabled=\"disabled\"" : "", IsUsedJQuery ? " class=\"default\"" : ""));
            sb.Append("<option value=\"\">请选择...</option>");
            if (!DesignMode)
            {
                if (ProvinceID != "")
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebUserControl.Data.City.xml"))
                    {
                        TextReader reader = new StreamReader(stream);
                        var items = from c in XElement.Load(reader).Elements()
                                    where c.Element("ProvinceID").Value == ProvinceID
                                    select new
                                    {
                                        Text = c.Element("Name").Value,
                                        Value = c.Element("ID").Value
                                    };
                        foreach (var item in items)
                        {
                            if (!Page.IsCallback)
                            {
                                if (item.Value == CityID)
                                {
                                    sb.Append(string.Format("<option selected='selected' value='{0}'>{1}</option>", item.Value, item.Text));
                                }
                                else
                                {
                                    sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Text));
                                }
                            }
                            else
                            {
                                sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Text));
                            }
                        }
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 输出县区html
        /// </summary>
        /// <returns></returns>
        private string GetCountyHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<select id=\"{0}_County\" name=\"{0}$County\" {1} onchange=\"regionSelect(this);\"{2}>", this.ClientID, ReadOnly ? "disabled=\"disabled\"" : "", IsUsedJQuery ? " class=\"default\"" : ""));
            sb.Append("<option value=\"\">请选择...</option>");
            if (!DesignMode)
            {
                if (CityID != "")
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebUserControl.Data.County.xml"))
                    {
                        TextReader reader = new StreamReader(stream);
                        var items = from c in XElement.Load(reader).Elements()
                                    where c.Element("CityID").Value == CityID
                                    select new
                                    {
                                        Text = c.Element("Name").Value,
                                        Value = c.Element("ID").Value
                                    };
                        foreach (var item in items)
                        {
                            if (!Page.IsCallback)
                            {
                                if (item.Value == CountyID)
                                {
                                    sb.Append(string.Format("<option selected='selected' value='{0}'>{1}</option>", item.Value, item.Text));
                                }
                                else
                                {
                                    sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Text));
                                }
                            }
                            else
                            {
                                sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Text));
                            }
                        }
                    }
                }
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            string[] arr = eventArgument.Split('@');
            switch (arr[0])
            {
                case "Province":
                    ProvinceID = arr[1];
                    result = ClientID + "_City@" + GetCityHtml();
                    break;
                case "City":
                    CityID = arr[1];
                    result = ClientID + "_County@" + GetCountyHtml();
                    break;
                default:
                    break;
            }
        }

        public string GetCallbackResult()
        {
            return result;
        }

        public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            ProvinceID = postCollection[ClientID + "$Province"];
            CityID = postCollection[ClientID + "$City"];
            CountyID = postCollection[ClientID + "$County"];
            return true;
        }

        public virtual void RaisePostDataChangedEvent()
        {
        }
    }
}
