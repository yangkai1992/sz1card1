using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ManagementWebsite.Management
{
    public partial class left : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // xmlDSMenu.Data = CurrentUser.UserGroup.MenuXml;
               // RepeaterDate1.DataBind();
            }
        }

        protected void LeftMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            XmlDataSource xmlDSSubMenu = new XmlDataSource();
            xmlDSSubMenu.EnableCaching = false;
            xmlDSSubMenu.Data = xmlDSMenu.Data;
            xmlDSSubMenu.XPath = @"siteMap/menu[" + (e.Item.ItemIndex + 1).ToString() + "]/subMenu/menu";
            ((Repeater)e.Item.FindControl("RepaterDate2")).DataSource = xmlDSSubMenu;
            ((Repeater)e.Item.FindControl("RepaterDate2")).DataBind();
        }
    }
}