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

namespace CarRentalSystem.Admin
{
    public partial class Services : System.Web.UI.Page
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
                    string sql = "select *, (case when is_active =1 then 'Active' else 'Inactive' end) as Status from services where 1=1 ";

                    if (txt_search.Text.Trim() != "")
                    {
                        sql += @"and
                       (name LIKE @search 
                          OR description LIKE @search
                          OR route_origin LIKE @search
                          OR route_destination LIKE @search
                          OR contactperson LIKE @search) ";
                    }
                    sql += "  order by service_id desc ";
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
            hd_serviceid.Value = "0";

            // Clear text fields
            txtServiceName.Text = "";
            txtDescription.Text = "";
            txtRouteOrigin.Text = "";
            txtRouteDestination.Text = "";
            txtContactPerson.Text = "";
            txtPrice.Text = "";

            // Uncheck checkbox
            chkActive.Checked = false;
            lbl_header.Text = "Add Service";

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
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM services WHERE service_id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                 
                        lbl_header.Text = "Edit Service";
                        hd_serviceid.Value = id;

                        txtServiceName.Text = dr["name"].ToString();
                        txtDescription.Text = dr["description"].ToString();
                        txtRouteOrigin.Text = dr["route_origin"].ToString();
                        txtRouteDestination.Text = dr["route_destination"].ToString();
                        txtContactPerson.Text = dr["contactperson"].ToString();
                        txtPrice.Text = dr["price"].ToString();

                        chkActive.Checked = Convert.ToBoolean(dr["is_active"]);

                    }
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
                        String cb = "Delete from services where service_id = " + hd_idselect.Value + "";
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

        protected void btn_filter_Click(object sender, EventArgs e)
        {
            bind_record();
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("services");

        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#registerModal').modal('hide')", true);
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
            
            
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "";
                    if (hd_serviceid.Value == "0") // Insert
                    {
                        query = @"INSERT INTO services 
                          (name, description, route_origin, route_destination, contactperson, price, is_active) 
                          VALUES (@name, @desc, @origin, @dest, @contact, @price, @active)";
                    }
                    else // Update
                    {
                        query = @"UPDATE services SET 
                          name=@name, description=@desc, route_origin=@origin, route_destination=@dest, 
                          contactperson=@contact, price=@price, is_active=@active" +
                          " WHERE service_id=@id";

                    }
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (hd_serviceid.Value != "0") // Insert
                        {
                            cmd.Parameters.AddWithValue("@id", hd_serviceid.Value);
                        }
                        cmd.Parameters.AddWithValue("@name", txtServiceName.Text.Trim());
                        cmd.Parameters.AddWithValue("@desc", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@origin", txtRouteOrigin.Text.Trim());
                        cmd.Parameters.AddWithValue("@dest", txtRouteDestination.Text.Trim());
                        cmd.Parameters.AddWithValue("@contact", txtContactPerson.Text.Trim());
                        cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text.Trim()));
                        cmd.Parameters.AddWithValue("@active", chkActive.Checked ? 1 : 0);

                        cmd.ExecuteNonQuery();

                        AlertNotify.ShowMessage(this, "Successfully Saved!", "Success", AlertNotify.MessageType.Success);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#registerModal').modal('hide')", true);
                        reset();
                        bind_record();
                    }

                }
            }
            catch (Exception ex)
            {
                //     throw new Exception("DB Error: " + ex.Message);
                ErrorLogger.WriteErrorLog(ex);
                AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
            }

        }
    }
}