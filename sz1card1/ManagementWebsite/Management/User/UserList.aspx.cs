using ManagementBLL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ManagementWebsite.Management.User
{
    public partial class UserList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gvUserList.DataSource = UserBLL.GetUserList(0, 20, "");
            gvUserList.DataBind();
            
        }

        protected void barHeader_ItemPostBack(sz1card1.Common.UI.ToolbarItem item)
        {

        }

        protected void lbDelete_Command(object sender, CommandEventArgs e)
        {

        }

        protected void gvUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btsearch_Click(object sender, EventArgs e)
        {

        }
    }
}