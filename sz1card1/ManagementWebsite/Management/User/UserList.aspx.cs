using ManagementBLL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUserControl.UI;
namespace ManagementWebsite.Management.User
{
    public partial class UserList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }

            
        }


        protected void lbDelete_Command(object sender, CommandEventArgs e)
        {

        }

        protected void btsearch_Click(object sender, EventArgs e)
        {

        }

        protected void gvUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserList.PageIndex = e.NewPageIndex;
            BindData();
        }

        private void BindData()
        {
            int total;
            gvUserList.DataSource = UserBLL.GetUserList(gvUserList.PageIndex,gvUserList.PageSize,"",null,out total);
            gvUserList.VirtualItemCount = total;
            gvUserList.DataBind();
        }

        protected void gvUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

    }
}