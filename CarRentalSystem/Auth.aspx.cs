using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.Security;

namespace CarRentalSystem
{
    public partial class Auth : System.Web.UI.Page
    {
        public string con = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        public MySqlCommand sqlcmd, cmd;
        public MySqlDataReader rdr;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                user_authenticate();
            }
        }
        public void user_authenticate()
        {

            using (MySqlConnection conn = new MySqlConnection(con))
            {
                using (MySqlCommand cmd = new MySqlCommand("Select * FROM users WHERE  Username = @usname"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@usname", LoginName1.Page.User.Identity.Name);

                    cmd.Connection = conn;
                    conn.Open();
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {

                        string usid = rdr["ID"].ToString();
                        Session["UserID"] = usid;
                        string role = rdr["Role"].ToString();
                        Session["UserRole"] = role;
                        if (role.Trim() == "Client")
                        {
                            Page.Response.Redirect("MyReservation");
                        }
                        else if (role.Trim() == "Admin" || role.Trim() == "Cashier")
                        {
                            Page.Response.Redirect("AdminHome");
                        }
                        else
                        {
                            FormsAuthentication.RedirectToLoginPage();
                            Page.Response.Redirect("Login");
                        }
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();
                        Page.Response.Redirect("Login");
                    }
                }

            }
        }
    }
}