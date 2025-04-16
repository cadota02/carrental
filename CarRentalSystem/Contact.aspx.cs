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
using System.IO;


namespace CarRentalSystem
{
    public partial class Contact : Page
    {
        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
           
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = @"INSERT INTO contact_us (full_name, email, subject, message, phone, status, created_at)
                                 VALUES (@full_name, @email, @subject, @message, @phone, 'new', NOW())";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@full_name", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@subject", txtSubject.Text.Trim());
                    cmd.Parameters.AddWithValue("@message", txtMessage.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        lblResult.Text = "Thank you for contacting us!";
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        lblResult.CssClass = "text-danger";
                        lblResult.Text = "Error: " + ex.Message;
                    }
                }
            }
        }

        private void ClearFields()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtSubject.Text = "";
            txtMessage.Text = "";
            txtPhone.Text = "";
        }
    }
}