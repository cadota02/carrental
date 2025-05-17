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
    public partial class Cars : System.Web.UI.Page
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
                    string sql = "select *, (case when is_available =1 then 'Available' else 'Not Available' end) as Status from cars where 1=1 ";

                    if (txt_search.Text.Trim() != "")
                    {
                        sql += @"and
                       (car_name LIKE @search 
                       OR description LIKE @search) ";
                    }
                    sql += "  order by car_id desc ";
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
            txtrate.Text = "";
            txtCarName.Text = "";
            txtContent.Text = "";
            txtContactPerson.Text = "";
            txtContactNo.Text = "";
            chkAvailable.Checked = false;
            imgPreview.ImageUrl = "";
            imgPreview.Visible = false;
            hd_carid.Value = "0"; // Optional: reset hidden field if you're using one for car_id
            lbl_header.Text = "Add Car";
          
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
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM cars WHERE car_id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lbl_header.Text = "Edit Car";
                        hd_carid.Value = id;
                        txtCarName.Text = dr["car_name"].ToString();
                        txtContent.Text = dr["description"].ToString();
                        txtContactPerson.Text = dr["contact_person"].ToString();
                        txtContactNo.Text = dr["contact_no"].ToString();
                        chkAvailable.Checked = Convert.ToBoolean(dr["is_available"]);
                        txtrate.Text = dr["RatePerDay"].ToString();
                        string imgPath = dr["image_path"].ToString();

                        if (!string.IsNullOrEmpty(imgPath))
                        {
                            imgPreview.ImageUrl = imgPath;
                            imgPreview.Visible = true;
                        }
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
                        String cb = "Delete from cars where car_id = " + hd_idselect.Value + "";
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
            Page.Response.Redirect("Cars");

        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#registerModal').modal('hide')", true);
        }
            protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string imagePath = "";
                
                if (fuImage.HasFile)
                {
                    string ext = Path.GetExtension(fuImage.FileName).ToLower();
                    if (ext != ".jpeg" && ext != ".png" && ext != ".jpg")
                    {
                        // Handle invalid file
                        AlertNotify.ShowMessage(this, "", "Invalid image format. Only .jpeg and .png files are allowed.", AlertNotify.MessageType.Warning);
                        return;
                    }

                    string filename = Guid.NewGuid() + ext;
                    string path = "~/uploads/" + filename;
                    string fullPath = Server.MapPath(path);
                    fuImage.SaveAs(fullPath);
                    imagePath = path;
                }
                else
                {
                    if(hd_carid.Value !="0")
                    {
                        imagePath = imgPreview.ImageUrl;
                    }
                }

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "";
                    if (hd_carid.Value == "0") // Insert
                    {
                        query = @"INSERT INTO cars (car_name, description, image_path, contact_person, contact_no, is_available, RatePerDay) 
                                        VALUES (@car_name, @desc, @img, @person, @no, @avail,@RatePerDay)";
                    }
                    else // Update
                    {
                        query = @"UPDATE cars SET car_name=@car_name, description=@desc, 
                        contact_person=@person, contact_no=@no, is_available=@avail," +
                           " image_path=@img, RatePerDay=@RatePerDay" +
                            " WHERE car_id=@id";
                     
                    }
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (hd_carid.Value != "0") // Insert
                        {
                            cmd.Parameters.AddWithValue("@id", hd_carid.Value);
                        }
                        cmd.Parameters.AddWithValue("@car_name", txtCarName.Text.Trim());
                        cmd.Parameters.AddWithValue("@desc", txtContent.Text);
                        cmd.Parameters.AddWithValue("@person", txtContactPerson.Text.Trim());
                        cmd.Parameters.AddWithValue("@no", txtContactNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@avail", chkAvailable.Checked ? 1 : 0);
                        cmd.Parameters.AddWithValue("@img", imagePath);
                        cmd.Parameters.AddWithValue("@RatePerDay", txtrate.Text);
                        cmd.ExecuteNonQuery();

                        AlertNotify.ShowMessage(this, "Successfully Saved!", "Success", AlertNotify.MessageType.Success);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#registerModal').modal('hide')", true);
                        reset();
                        bind_record(); 
                    }
                  
                }
            }
            catch(Exception ex)
            {
            //     throw new Exception("DB Error: " + ex.Message);
                ErrorLogger.WriteErrorLog(ex);
                AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
            }
           
        }
    }
}