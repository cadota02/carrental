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
    public partial class Contacts : System.Web.UI.Page
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
                    string sql = "select * from contact_us where 1=1 ";

                    if (txt_search.Text.Trim() != "")
                    {
                        sql += @"and
                       (full_name LIKE @search 
                       OR email LIKE @search 
                       OR subject LIKE @search 
                       OR message LIKE @search 
                       OR phone LIKE @search
                       OR status LIKE @search) ";
                    }
                    sql += "  order by contact_id desc ";
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
                        String cb = "Delete from contact_us where contact_id = " + hd_idselect.Value + "";
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
          //  string userId = btn.CommandArgument; // Get UserID from CommandArgument
            HiddenField hdId = (HiddenField)row.FindControl("hd_id");
            // Example: Toggle status in database
            string query = @"UPDATE contact_us SET status = CASE 
                            WHEN status = 'new' THEN 'read'
                            WHEN status = 'read' THEN 'archived'
                            WHEN status = 'archived' THEN 'read'
                            ELSE 'new'
                        END WHERE contact_id = @id";

            using (MySqlConnection con = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", hdId.Value);
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
            Page.Response.Redirect("Contacts");

        }

   

    }
}