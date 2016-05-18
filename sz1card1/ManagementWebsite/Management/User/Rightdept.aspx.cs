using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using sz1card1.Management.Data.Entities;
using sz1card1.Management.Data;

public partial class Management_User_Rightdept : BasePage
{
    XmlDocument xmlDoc;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadXmlTree();
        }
    }
    #region
    //将XML文档加载到树控件
    protected void LoadXmlTree()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(CurrentUser.UserGroup.MenuXml);//动态加载XML文档
        XmlNodeList xnl = xmlDoc.GetElementsByTagName("title");
        TreeNode tn = new TreeNode();
        tn.Text = "帮助文档 ";
        tn.Value = "-1 ";
        trHelpMenu.Nodes.Add(tn);//添加"区域"父结点
        //遍历区域下的所有子节点
        foreach (XmlNode xn in xnl)
        {
            TreeNode tnC = new TreeNode();
            tnC.Text = xn.InnerText.ToString();
            tnC.Value = xn.InnerText.ToString();
            XmlNodeList list = xn.SelectNodes(".//title");
            foreach (XmlNode node in list)
            {
                TreeNode nde = new TreeNode();
                nde.Text = node.InnerText.ToString();
                nde.Value = node.InnerText.ToString();
            }
            tn.ChildNodes.Add(tnC);
            trHelpMenu.DataBind();
        }
    }
    #endregion
    protected override string PermitonUrl
    {
        get
        {
            return "User/UserGroup.aspx";
        }
    }
}




