﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.Security;

namespace CarRentalSystem.Customer
{
    public partial class CustomerHome : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        static int userIDS = 0;
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
                    userIDS = UserHelper.GetUserIdFromDatabase(Page.User.Identity.Name.ToString());
                    if (userIDS == -1)
                    {

                    }
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
                    string sql = "select * from vw_booking where userid=@userid ";

                    if (txt_search.Text.Trim() != "")
                    {
                        sql += @"and
                       (client_name LIKE @search 
                       OR description LIKE @search
                       OR car_name LIKE @search
                       OR client_remarks LIKE @search ) ";
                    }
                   
                    sql += "  order by dayscount desc ";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@userid", userIDS);
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
         
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "closeModal();", true);
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
                        String cb = "Delete from bookings where booking_id = " + hd_idselect.Value + "";
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
            Page.Response.Redirect("MyReservation");

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
                Page.Response.Redirect("AddReservation?id=" + hd_idselect.Value);
             //   get_data(hd_idselect.Value);
                //   ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop1", "openModal();", true);
              /// ScriptManager.RegisterStartupScript(this, this.GetType(), "EditModal", "openModal();", true);
            }
            catch (Exception ex)
            {

                AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
            }
        }
        public void get_data(string id)
        {
            //try
            //{
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                String cb = "select * from vw_booking where booking_id=" + id + " ";
                MySqlCommand cmd = new MySqlCommand(cb);
                cmd.Connection = conn;
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    //hd_appointmentid.Value = id;

                    //lblFullname.Text = rdr["client_name"].ToString();
                    //lblDateOfAppointment.Text = DateTime.Parse(rdr["booking_date"].ToString()).ToString("MMM d, yyyy");
                    //lblremarks.Text = rdr["client_remarks"].ToString();
                    //lblcarname.Text = rdr["car_name"].ToString();
                    //txtremarks.Text = rdr["admin_remarks"].ToString();
                    //dpstatus.SelectedValue = rdr["status"].ToString();
                }
                rdr.Close();
                conn.Close();
            }
            //}
            //catch (Exception ex)
            //{
            //    ShowMessage(ex.Message.ToString(), "");

            //}

        }


    }
}