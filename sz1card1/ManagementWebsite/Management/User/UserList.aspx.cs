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
                gvUserList.PageIndex = 0;
                gvUserList.RecordCount = 1000;
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
            List<ManagementDataModel.Models.User> list = new List<ManagementDataModel.Models.User>();
            ManagementDataModel.Models.User user = null;
            for (int i = 0; i < 1000; i++)
            {
                user = new ManagementDataModel.Models.User()
                {
                    Guid = Guid.NewGuid(),
                    Account = i.ToString(),
                    TrueName = i.ToString() + i.ToString(),
                };
                list.Add(user);
            }
            list = list.OrderByDescending<ManagementDataModel.Models.User, string>(x => x.Account).Skip(gvUserList.PageIndex * gvUserList.PageSize).Take<ManagementDataModel.Models.User>(gvUserList.PageSize).ToList();

            gvUserList.DataSource = list;
            gvUserList.VirtualItemCount = 1000;
          
            gvUserList.DataBind();
        }

        protected void gvUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

    }
}