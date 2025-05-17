using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarRentalSystem
{
    public partial class SiteCustomer : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                Page.Response.Redirect("Login");

            }
            else
            {
                string username = Page.User.Identity.Name.ToString();
                var result = DBUserdefualt.GetUserIdAndRole(username);
                int userId = result.userId;
                string role = result.role;
                Session["usersid"] = userId.ToString();
                if (role == "Admin")
                {

                    Page.Response.Redirect("AdminHome");
                }
            }
        }
    }
}