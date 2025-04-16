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
    public partial class Login : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DBUserdefualt.CreateDefaultAdmin();

            }
        }
        protected void ValidateUser_Auth(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(connString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE Username=@user AND IsApproved=1", con);
                cmd.Parameters.AddWithValue("@user", Login2.UserName);
                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string dbPass = dr["PasswordHash"].ToString();
                    if (BCrypt.Net.BCrypt.Verify(Login2.Password, dbPass))
                    {

                        FormsAuthentication.RedirectFromLoginPage(Login2.UserName, Login2.RememberMeSet);
                    }
                    else
                    {
                        Login2.FailureText = "Username and/or password is incorrect.";

                    }
                }
                else
                {
                    Login2.FailureText = "Username and/or password is incorrect.";
                }
            }
        }
    }
}