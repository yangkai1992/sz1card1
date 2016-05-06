using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Management_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (account.Text == "yangkai" && passworld.Text == "123456")
        {
            HttpCookie cookie = new HttpCookie("Token");
            cookie.Value = account.Text+ passworld.Text;
            Response.Cookies.Add(cookie);
            Response.Redirect("main.aspx");
        }
    }
}