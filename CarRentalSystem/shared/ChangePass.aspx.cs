using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using MySql.Data.MySqlClient;

namespace CarRentalSystem.shared
{
    public partial class ChangePass : System.Web.UI.Page
    {

        public static int userid = 0;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Dynamically set master page
            string username = Page.User.Identity.Name.ToString();
            var result = DBUserdefualt.GetUserIdAndRole(username);
            userid = result.userId;
            string role = result.role;

            if (role == "Client")
                MasterPageFile = "~/SiteCustomer.Master";
            else
                MasterPageFile = "~/SiteAdmin.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                Page.Response.Redirect("Login");

            }
        }
        public string con = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            int userId = userid;
            string oldPass = txtOldPassword.Text.Trim();
            string newPass = txtNewPassword.Text.Trim();
            string confirmPass = txtConfirmPassword.Text.Trim();
            if (userId == 0)
            {

                AlertNotify.ShowMessage(this, "User failed to authenticate.", "Warning", AlertNotify.MessageType.Warning);
                return;
            }
            if (newPass != confirmPass)
            {
                AlertNotify.ShowMessage(this, "New password and confirm password do not match.", "Warning", AlertNotify.MessageType.Warning);

                return;
            }



            using (MySqlConnection conn = new MySqlConnection(con))
            {
                conn.Open();

                // Get the stored password hash
                string selectQuery = "SELECT PasswordHash FROM users WHERE ID = @id";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@id", userId);

                object result = selectCmd.ExecuteScalar();
                if (result == null)
                {
                    //  lblMessage.Text = "User not found.";
                    AlertNotify.ShowMessage(this, "User not found.", "Warning", AlertNotify.MessageType.Warning);
                    return;
                }

                string dbPass = result.ToString();
                if (!BCrypt.Net.BCrypt.Verify(oldPass, dbPass))
                {

                    AlertNotify.ShowMessage(this, "Incorrect old password.", "Warning", AlertNotify.MessageType.Warning);
                    return;
                }

                // Hash the new password
                string hashedNewPass = BCrypt.Net.BCrypt.HashPassword(newPass);

                // Update the new password in the database
                string updateQuery = "UPDATE users SET PasswordHash = @newPass WHERE ID = @id";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@newPass", hashedNewPass);
                updateCmd.Parameters.AddWithValue("@id", userId);

                updateCmd.ExecuteNonQuery();

                AlertNotify.ShowMessage(this, "Password changed successfully.", "Success", AlertNotify.MessageType.Success);
            }
        }
    }
}