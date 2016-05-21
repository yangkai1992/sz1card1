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
            List<ManagementDataModel.Models.User> list = new List<ManagementDataModel.Models.User>();
            ManagementDataModel.Models.User user = null;
            for (int i = 0; i < 10; i++)
            {
                user = new ManagementDataModel.Models.User()
                {
                    Guid = Guid.NewGuid(),
                    Account = i.ToString(),
                    TrueName = i.ToString() + i.ToString(),
                };
                list.Add(user);
            }

            gvUserList.DataSource = list;                           //UserBLL.GetUserList(0, 20, "");
            gvUserList.DataBind();
            
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