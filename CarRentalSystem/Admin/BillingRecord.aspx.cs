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
    public partial class BillingRecord : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCharges();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvCharges.PageIndex = e.NewPageIndex;
            this.LoadCharges();
        }
        protected void btn_filter_Click(object sender, EventArgs e)
        {
            LoadCharges();
        }
        protected void btn_reset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("BillingRecords");

        }
        private void LoadCharges()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    string sql = "select * from vw_bill where 1=1 ";

                    if (txt_search.Text.Trim() != "")
                    {
                        sql += @"and
                       (billno LIKE @search 
                            OR fullname LIKE @search
                            OR remarks LIKE @search) ";
                    }
                    sql += "  order by billid desc ";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@search", "%" + txt_search.Text + "%");
                    cmd.Connection = conn;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {

                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvCharges.DataSource = dt;
                        gvCharges.DataBind();

                    }
                }
                conn.Close();
                // lbl_item.Text = gb.footerinfo_gridview(gv_masterlist).ToString();

            }
        }
        protected void btn_select_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn_select = (LinkButton)sender;
                GridViewRow item = (GridViewRow)btn_select.NamingContainer;

                HiddenField hd_idselect = (HiddenField)item.FindControl("hd_id");

                Page.Response.Redirect("Billing?id=" + hd_idselect.Value);
            }
            catch (Exception ex)
            { }
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
                    using (MySqlTransaction trx = conn.BeginTransaction())
                    {
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = trx;

                                // First delete items from cart
                                string deleteCart = "DELETE FROM billitems WHERE billid = @id";
                                cmd.CommandText = deleteCart;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", hd_idselect.Value);
                                cmd.ExecuteNonQuery();

                                // Then delete invoice
                                string deleteInvoice = "DELETE FROM bill WHERE billid = @id";
                                cmd.CommandText = deleteInvoice;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@id", hd_idselect.Value);
                                int result = cmd.ExecuteNonQuery();

                                trx.Commit();

                                if (result >= 1)
                                {
                                    AlertNotify.ShowMessage(this, "Successfully Deleted!", "Success", AlertNotify.MessageType.Success);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modalPopUp_Delete').modal('hide')", true);
                                    LoadCharges();
                                }
                                else
                                {
                                    AlertNotify.ShowMessage(this, "No invoice deleted!", "Warning", AlertNotify.MessageType.Warning);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            trx.Rollback();
                            AlertNotify.ShowMessage(this, "Error: " + ex.Message, "Error", AlertNotify.MessageType.Error);
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