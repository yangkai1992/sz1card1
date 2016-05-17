using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ManagementWebsite.Management
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btlogin_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Main.aspx");
        }
    }
}