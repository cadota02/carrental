using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarRentalSystem
{
    public partial class SiteAdmin : System.Web.UI.MasterPage
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
                Session["usersid"] = userId.ToString();
                string role = result.role;
                if (role == "Client")
                {

                    Page.Response.Redirect("MyReservation");
                }
                else if (role == "Cashier")
                {
                  //  Show only billing
                    lihome.Visible = true;
                    liBilling.Visible = true;
                    limanagecar.Visible = false;
                    lischedule.Visible = false;
                    licontacts.Visible = false;
                    liaccount.Visible = false;
                 
                }
            }

        }
    }
}