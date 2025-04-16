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

namespace CarRentalSystem.Admin
{
    public partial class Account : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!this.Page.User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                else
                {
                    bind_record();
                }
            }
        }
        public void bind_record()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = "select ID,Username,Firstname,Lastname,Middlename,UserPosition,Role,Email,IsApproved, " +
                        "CONCAT(Firstname, ', ', LEFT(Middlename, 1), '. ', Lastname) AS Fullname, " +
                        "CASE WHEN IsApproved = 1 THEN 'Active' ELSE 'Inactive' END AS Status " +
                        "from users where 1=1 ";

                    if (txt_search.Text.Trim() != "")
                    {
                        sql += @"and
                       (Username LIKE @search 
                       OR Firstname LIKE @search 
                       OR Lastname LIKE @search 
                       OR Middlename LIKE @search 
                       OR Role LIKE @search) ";
                    }
                    sql += "  order by ID desc ";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@search", "%" + txt_search.Text + "%");
                    cmd.Connection = conn;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gv_masterlist.DataSource = dt;
                        gv_masterlist.DataBind();

                    }
                }
                conn.Close();
                // lbl_item.Text = gb.footerinfo_gridview(gv_masterlist).ToString();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gv_masterlist.PageIndex = e.NewPageIndex;
            this.bind_record();
        }
        protected void btn_add_Click(object sender, EventArgs e)
        {
            reset();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop1", "openModal();", true);
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "closeModal();", true);
         // ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#registerModal').modal('show')", true);
        }
        public void reset()
        {
            hd_userid.Value = "0";
            txtUsername.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtMiddleName.Text = "";
            txtPosition.Text = "";
            txtAddress.Text = "";
            txtContactNo.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            dprole.SelectedIndex = 0;
            dpstatus.SelectedIndex = 0;
            rfvPassword.Enabled = true;
            rfvConfirmPassword.Enabled = true;
            bind_record();
            txt_search.Focus();
        }
        protected void btn_select_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn_select = (LinkButton)sender;
                GridViewRow item = (GridViewRow)btn_select.NamingContainer;
                HiddenField hd_idselect = (HiddenField)item.FindControl("hd_id");
                HiddenField hd_name = (HiddenField)item.FindControl("hd_name");
                HiddenField hd_status = (HiddenField)item.FindControl("hd_status");

                get_data(hd_idselect.Value);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#registerModal').modal('show')", true);
              //  ScriptManager.RegisterStartupScript(this, this.GetType(), "EditModal", "openModal(" + hd_idselect.Value + ");", true);
            }
            catch (Exception ex)
            {

                AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
            }
        }
        public void get_data(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    String cb = "select * from users where ID=" + id + " ";
                    MySqlCommand cmd = new MySqlCommand(cb);
                    cmd.Connection = conn;
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        hd_userid.Value = id;
                        txtUsername.Text = rdr["Username"].ToString();
                        txtFirstName.Text = rdr["Firstname"].ToString();
                        txtLastName.Text = rdr["Lastname"].ToString();
                        txtMiddleName.Text = rdr["Middlename"].ToString();
                        txtPosition.Text = rdr["UserPosition"].ToString();
                        txtAddress.Text = rdr["Address"].ToString();
                        txtContactNo.Text = rdr["ContactNo"].ToString();
                        txtEmail.Text = rdr["Email"].ToString();
                        dprole.Text = rdr["Role"].ToString();
                        dpstatus.SelectedValue = rdr["isApproved"].ToString();
                        rfvPassword.Enabled = false;
                        rfvConfirmPassword.Enabled = false;

                    }
                    rdr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
           
               AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);

            }

        }
        protected void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn_select = (LinkButton)sender;
                GridViewRow item = (GridViewRow)btn_select.NamingContainer;

                HiddenField hd_idselect = (HiddenField)item.FindControl("hd_id");
                HiddenField hd_name = (HiddenField)item.FindControl("hd_name");


                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        String cb = "Delete from users where ID = " + hd_idselect.Value + "";
                        cmd.CommandText = cb;
                        cmd.Connection = conn;

                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (result >= 1)
                        {

                            AlertNotify.ShowMessage(this, "Successfully Deleted!", "Success", AlertNotify.MessageType.Success);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modalPopUp_Delete').modal('hide')", true);
                            bind_record();

                        }
                    }
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
               

            }

        }
        protected void btnAction_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string userId = btn.CommandArgument; // Get UserID from CommandArgument

            // Example: Toggle status in database
            string query = "UPDATE users SET IsApproved = CASE WHEN IsApproved = 1 THEN 0 ELSE 1 END WHERE ID = @UserID";

            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            // Refresh GridView after update
            bind_record();
        }
        protected void btn_filter_Click(object sender, EventArgs e)
        {
            bind_record();
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Account");

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid) // Ensure all required fields are filled
            {
                string firstname = txtFirstName.Text.Trim();
                string lastname = txtLastName.Text.Trim();
                string middlename = txtMiddleName.Text.Trim();
                string userposition = txtPosition.Text;
                string username = txtUsername.Text.Trim();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text.Trim());
                string address = txtAddress.Text.Trim();
                string contactNo = txtContactNo.Text.Trim();
                string email = txtEmail.Text.Trim();
                string role = dprole.SelectedValue; // Default role based on position
                string status = dpstatus.SelectedValue;
                int isApproved = int.Parse(status); // Approval pending


                if (hd_userid.Value == "0") // INSERT IF NOT SELECT ID
                {

                    using (MySqlConnection con = new MySqlConnection(connString))
                    {
                        try
                        {
                            con.Open();

                            string checkUserQuery = "SELECT COUNT(*) FROM users WHERE Username = @username;";
                            using (MySqlCommand checkCmd = new MySqlCommand(checkUserQuery, con))
                            {
                                checkCmd.Parameters.AddWithValue("@username", username);
                                int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());
                                con.Close();
                                if (userExists > 0)
                                {
                                    txtUsername.Text = "";
                                    txtUsername.Focus();
                                    AlertNotify.ShowMessage(this,"Username already exist!", "Warning", AlertNotify.MessageType.Warning);
                                }
                                else
                                {
                                    con.Open();
                                    string query = "INSERT INTO users (Firstname, Lastname, Middlename, Userposition, Username, PasswordHash, Address, ContactNo, Email, Role, IsApproved) " +
                                           "VALUES (@firstname, @lastname, @middlename, @userposition, @username, @password, @address, @contactNo, @email, @role, @isApproved)";

                                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                                    {
                                        cmd.Parameters.AddWithValue("@firstname", firstname);
                                        cmd.Parameters.AddWithValue("@lastname", lastname);
                                        cmd.Parameters.AddWithValue("@middlename", middlename);
                                        cmd.Parameters.AddWithValue("@userposition", userposition);
                                        cmd.Parameters.AddWithValue("@username", username);
                                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                                        cmd.Parameters.AddWithValue("@address", address);
                                        cmd.Parameters.AddWithValue("@contactNo", contactNo);
                                        cmd.Parameters.AddWithValue("@email", email);
                                        cmd.Parameters.AddWithValue("@role", role);
                                        cmd.Parameters.AddWithValue("@isApproved", isApproved);

                                        cmd.ExecuteNonQuery();

                                        reset();
                                        AlertNotify.ShowMessage(this, "Successfully Created!", "Success", AlertNotify.MessageType.Success);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pops", "$('#registerModal').modal('hide')", true);

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
                          
                        }
                    }
                }
                else // UPDATE IF HAS ID
                {
                    using (MySqlConnection con = new MySqlConnection(connString))
                    {
                        try
                        {
                            con.Open();

                            string checkUserQuery = "SELECT COUNT(*) FROM users WHERE Username = @username AND ID !=@ID;";
                            using (MySqlCommand checkCmd = new MySqlCommand(checkUserQuery, con))
                            {
                                checkCmd.Parameters.AddWithValue("@username", username);
                                checkCmd.Parameters.AddWithValue("@ID", hd_userid.Value);
                                int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());
                                con.Close();
                                if (userExists > 0)
                                {
                                    txtUsername.Text = "";
                                    txtUsername.Focus();
                                   
                                    AlertNotify.ShowMessage(this, "Username already exist!", "Warning", AlertNotify.MessageType.Warning);
                                }
                                else
                                {
                                    con.Open();
                                    string haspassword = (txtPassword.Text.Length > 0) ? "PasswordHash = @password," : "";

                                    string query = "UPDATE users SET Firstname = @firstname, Lastname = @lastname, Middlename = @middlename, " +
                                        "Userposition = @userposition, Username = @username, " + haspassword + " Address = @address, " +
                                        "ContactNo = @contactNo, Email = @email, Role = @role, isApproved=@isApproved " +
                                        "WHERE ID = @userId";

                                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                                    {
                                        cmd.Parameters.AddWithValue("@userId", hd_userid.Value);
                                        cmd.Parameters.AddWithValue("@firstname", firstname);
                                        cmd.Parameters.AddWithValue("@lastname", lastname);
                                        cmd.Parameters.AddWithValue("@middlename", middlename);
                                        cmd.Parameters.AddWithValue("@userposition", userposition);
                                        cmd.Parameters.AddWithValue("@username", username);
                                        if (txtPassword.Text.Length > 0) { cmd.Parameters.AddWithValue("@password", hashedPassword); }

                                        cmd.Parameters.AddWithValue("@address", address);
                                        cmd.Parameters.AddWithValue("@contactNo", contactNo);
                                        cmd.Parameters.AddWithValue("@email", email);
                                        cmd.Parameters.AddWithValue("@role", role);
                                        cmd.Parameters.AddWithValue("@isApproved", isApproved);

                                        cmd.ExecuteNonQuery();
                                        reset();
                                        AlertNotify.ShowMessage(this, "Successfully Updated!", "Success", AlertNotify.MessageType.Success);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pops", "$('#registerModal').modal('hide')", true);

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
                        }
                    }

                }
            }
        }

    }
}