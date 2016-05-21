using System;
using System.ComponentModel;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Web.UI.Design;
using System.Drawing.Design;
using System.Web;

namespace WebUserControl.UI
{
    /// <summary>
    /// 权限选择器
    /// </summary>
    [
        Designer(typeof(System.Web.UI.Design.WebControls.CompositeControlDesigner)),
        ToolboxData("<{0}:RightsSelector runat=\"server\"></{0}:RightsSelector>")
    ]
    public class RightsSelector : CompositeControl
    {
        private CheckBoxList chkRightsList;

        public RightsSelector()
        {
        }

        /// <summary>
        /// 重复数
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取重复数"),
        Category("Misc"),
        DefaultValue("4"),
        ]
        public int RepeatColumns
        {
            get
            {
                if (ViewState["RepeatColumns"] == null)
                    ViewState.Add("RepeatColumns", 4);
                return Convert.ToInt32(ViewState["RepeatColumns"]);
            }
            set
            {
                ViewState["RepeatColumns"] = value;
            }
        }

        /// <summary>
        /// Xml源字符串
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取Xml源字符串"),
        Category("Misc"),
        DefaultValue(""),
        ]
        public string XmlSource
        {
            get
            {
                if (ViewState["XmlSource"] == null)
                    ViewState.Add("XmlSource", string.Empty);
                return ViewState["XmlSource"].ToString();
            }
            set
            {
                ViewState["XmlSource"] = value;
            }
        }

        /// <summary>
        /// Xml源文件
        /// </summary>
        [
        Browsable(true),
        Description("设置/获取Xml源文件"),
        Category("Misc"),
        Editor(typeof(UrlEditor), typeof(UITypeEditor))
        ]
        public string XmlSourceFile
        {
            get
            {
                if (ViewState["XmlSourceFile"] == null)
                    ViewState.Add("XmlSourceFile", string.Empty);
                return ViewState["XmlSourceFile"].ToString();
            }
            set
            {
                ViewState["XmlSourceFile"] = value;
            }
        }

        /// <summary>
        /// 选中的Xml字符串
        /// </summary>
        [Browsable(false)]
        public string SelectedXml
        {
            get
            {
                ViewState["SelectedXml"] = GetSelectedXml();
                return ViewState["SelectedXml"].ToString();
            }
            set
            {
                ViewState["SelectedXml"] = value;
            }
        }

        private string GetSelectedXml()
        {
            if (XmlSource == string.Empty && XmlSourceFile == string.Empty)
            {
                throw new Exception("未找到xml源数据!");
            }
            XmlDocument xmlDoc = new XmlDocument();
            if (XmlSource != string.Empty)
            {
                xmlDoc.LoadXml(XmlSource);
            }
            else
            {
                xmlDoc.Load(HttpContext.Current.Server.MapPath(XmlSourceFile));
            }
            XPathNavigator rootNav = xmlDoc.CreateNavigator();
            XPathNavigator menuNav;
            for (int i = 0; i < chkRightsList.Items.Count; i++)
            {
                if (!chkRightsList.Items[i].Selected)
                {
                    menuNav = rootNav.SelectSingleNode("siteMap/menu/subMenu/menu[title='" + chkRightsList.Items[i].Text + "' and url='" + chkRightsList.Items[i].Value + "']");
                    rootNav.MoveTo(menuNav);
                    rootNav.DeleteSelf();
                    rootNav.MoveToRoot();
                }
            }
            rootNav.MoveToFirstChild();
            if (rootNav.MoveToFirstChild())
            {
                while (rootNav.MoveToNext())
                {
                    menuNav = rootNav.SelectSingleNode("subMenu");
                    if (menuNav.HasChildren)
                    {
                        continue;
                    }
                    else
                    {
                        rootNav.DeleteSelf();
                        rootNav.MoveToFirstChild();
                    }
                }
                rootNav.MoveToRoot();
            }
            return xmlDoc.OuterXml;
        }

        public void SetSelectedXml()
        {
            if (ViewState["SelectedXml"] != null && ViewState["SelectedXml"].ToString() != string.Empty)
            {
                chkRightsList.SelectedIndex = -1;
                using (TextReader tr = new StringReader(ViewState["SelectedXml"].ToString()))
                {
                    using (XmlReader xr = XmlReader.Create(tr))
                    {
                        string title = string.Empty;
                        string url = string.Empty;
                        while (xr.Read())
                        {
                            if (xr.IsStartElement("title"))
                            {
                                title = xr.ReadElementString();
                            }
                            if (xr.IsStartElement("url"))
                            {
                                url = xr.ReadElementString();
                            }
                            if (title != string.Empty && url != string.Empty)
                            {
                                for (int i = 0; i < chkRightsList.Items.Count; i++)
                                {
                                    if (chkRightsList.Items[i].Text == title && chkRightsList.Items[i].Value == url)
                                    {
                                        chkRightsList.Items[i].Selected = true;
                                        break;
                                    }
                                }
                                title = string.Empty;
                                url = string.Empty;
                            }
                        }
                        xr.Close();
                    }
                }
            }
        }

        private XmlReader GetSourceReader()
        {
            TextReader tr;
            if (DesignMode)
            {
                tr = new StringReader("<siteMap><menu><title>用户管理</title><subMenu><menu><title>部门级别</title><url>User/UserGroup.aspx</url><description></description></menu><menu><title>用户列表</title><url>User/UserList.aspx</url><description></description></menu></subMenu></menu></siteMap>");
            }
            else
            {
                if (XmlSource == string.Empty && XmlSourceFile == string.Empty)
                {
                    throw new Exception("未找到xml源数据!");
                }
                if (XmlSource != string.Empty)
                {
                    tr = new StringReader(XmlSource);
                }
                else
                {
                    tr = new StreamReader(HttpContext.Current.Server.MapPath(XmlSourceFile));
                }
            }
            return XmlReader.Create(tr);
        }

        protected override void CreateChildControls()
        {
            chkRightsList = new CheckBoxList();
            chkRightsList.RepeatColumns = RepeatColumns;
            chkRightsList.RepeatDirection = RepeatDirection.Horizontal;
            using (XmlReader xr = GetSourceReader())
            {
                string title = string.Empty;
                string url = string.Empty;
                while (xr.Read())
                {
                    if (xr.IsStartElement("title"))
                    {
                        title = xr.ReadElementString();
                    }
                    if (xr.IsStartElement("url"))
                    {
                        url = xr.ReadElementString();
                    }
                    if (title != string.Empty && url != string.Empty)
                    {
                        chkRightsList.Items.Add(new ListItem(title, url));
                        title = string.Empty;
                        url = string.Empty;
                    }
                }
                xr.Close();
            }
            base.Controls.Add(chkRightsList);
            base.CreateChildControls();
            SetSelectedXml();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.EnsureChildControls();
            base.Render(writer);
        }
    }

}
