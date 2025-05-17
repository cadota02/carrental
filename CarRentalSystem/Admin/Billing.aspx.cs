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
    public partial class Billing : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                LoadCartItems();
                LoadChargeDropdown();
                LoadCustomers(); // Bind dropdownlist
                txtInvoiceNo.Text = GenerateInvoiceNo();
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                if (Request.QueryString["bookid"] != null)
                {
                    string bookid = Request.QueryString["bookid"].ToString();
                    hd_bookid.Value = bookid;
                    Get_bookingHasBill(hd_bookid.Value);
                 

                }
                    if (Request.QueryString["id"] != null)
                {

                    hd_invid.Value = Request.QueryString["id"].ToString();
                    get_invinfo(hd_invid.Value);

                }
            }
        }
        private void autocharge_booking(string bookid, string carname, string formattedatebook, string qty, string prices)
        {
            invoiceAddEdit();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "INSERT INTO billitems (billid, serviceid,name,description,qty, price, datelog) VALUES (@invid,@serviceid, @chargename, '',@qty, @price, @datelog)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@invid", hd_invid.Value); // Assuming 0 for unsaved invoices
                    cmd.Parameters.AddWithValue("@serviceid", "0");
                    cmd.Parameters.AddWithValue("@chargename", carname + " booked (" +formattedatebook + "  " + qty + " day/s)");
                    cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(prices));
                    cmd.Parameters.AddWithValue("@datelog", DateTime.Now);
                    cmd.Parameters.AddWithValue("@qty", Convert.ToInt32(qty));
                    int stat = cmd.ExecuteNonQuery();
                    if (stat > 0)
                    {
                        get_invinfo(hd_invid.Value);
                      //  AlertNotify.ShowMessage(this, "Item added successfully!", "Success", AlertNotify.MessageType.Success);
                      // ScriptManager.RegisterStartupScript(this, this.GetType(), "Pops2", "$('#AddChargeModal').modal('hide')", true);
                    }
                }
            }

        }
        public void Get_bookingHasBill(string bookid)
        {
            //try
            //{
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                String cb = "select * from bill where bookingid=@id ";
                MySqlCommand cmd = new MySqlCommand(cb);
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@id", bookid);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    ddlCustomer.Enabled = false;
                  hd_invid.Value  = rdr["billid"].ToString();
                    get_invinfo(hd_invid.Value);
                }
               else
                {
                    get_bookingcharge(bookid);
                }
            }
        }
         public void get_bookingcharge(string bookid)
        {
            //try
            //{
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                String cb = "select * from vw_booking where booking_id=@id ";
                MySqlCommand cmd = new MySqlCommand(cb);
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@id", bookid);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {

                    hd_clientid.Value = rdr["userid"].ToString();
                    ddlCustomer.SelectedValue = rdr["userid"].ToString();
                    decimal rate = 0;
                    decimal.TryParse(rdr["RatePerDay"].ToString(), out rate);
                    int days = Convert.ToInt32( rdr["duration_days"].ToString());
                    string datename = rdr["formatted_date"].ToString();
                    string carname = rdr["car_name"].ToString();
                  autocharge_booking(bookid, carname, datename, days.ToString(), rate.ToString());


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
        private void get_invinfo(string invid)
        {
            string query = @"SELECT billno, billdate, clientid, remarks, cash, `change`, billid
                     FROM bill 
                     WHERE billid = @billid";

            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@billid", invid);
                    con.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {


                            hd_invid.Value = reader["billid"] != DBNull.Value ? reader["billid"].ToString() : "0";
                            txtInvoiceNo.Text = reader["billno"] != DBNull.Value ? reader["billno"].ToString() : "";
                            txtDate.Text = reader["billdate"] != DBNull.Value ? Convert.ToDateTime(reader["billdate"]).ToString("yyyy-MM-dd") : "";
                            ddlCustomer.SelectedValue = reader["clientid"] != DBNull.Value ? reader["clientid"].ToString() : "";
                            txtRemarks.Text = reader["remarks"] != DBNull.Value ? reader["remarks"].ToString() : "";
                            txtCash.Text = reader["cash"] != DBNull.Value ? Convert.ToDecimal(reader["cash"]).ToString("0.00") : "0.00";
                            txtChange.Text = reader["change"] != DBNull.Value ? Convert.ToDecimal(reader["change"]).ToString("0.00") : "0.00";
                            computechange();

                            btnPrint.Visible = true;
                            btnPrint.NavigateUrl = "~/BillPrint?billid=" + hd_invid.Value;
                        }
                    }
                }
            }

            LoadCartItems(); // Optional: Load cart items into GridView
        }
        protected void btnAddCharge_Click(object sender, EventArgs e)
        {
            clearcart();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop1", "openModal();", true);
        }
        public void clearcart()
        {
            hd_cartid.Value = "0";
            txtQuantity.Text = "";
            txtPrice.Text = "";
            txtRemarks.Text = "";
            txtSearch.Text = "";
            ddlChargeName.SelectedIndex = 0;
        }
        public void ClearFields()
        {
            txtCash.Text = "0";
            txtChange.Text = "0";
            txtInvoiceNo.Text = GenerateInvoiceNo();
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");



        }
        private void LoadChargeDropdown()
        {
            using (var con = new MySqlConnection(connStr))
            {
                con.Open();
                string query = "SELECT service_id, name FROM services WHERE is_active = 1";
                using (var cmd = new MySqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        ddlChargeName.Items.Clear();
                        ddlChargeName.Items.Add(new ListItem("-- Select Charge --", ""));
                        while (reader.Read())
                        {
                            ddlChargeName.Items.Add(new ListItem(reader["name"].ToString(), reader["name"].ToString()));
                        }
                    }
                }
            }
        }
        private void LoadCartItems()
        {
            using (var con = new MySqlConnection(connStr))
            {
                con.Open();
                string query = "SELECT billtemid, name,description, price, qty, (price *qty) as amount, billid,serviceid FROM billitems WHERE billid = @billid "; // Assuming 0 for unsaved invoices
                if (txtSearch.Text.Trim() != "")
                {
                    query += @"and
                       (name LIKE @search ) ";
                }
                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@billid", hd_invid.Value);
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        gvCart.DataSource = dt;
                        gvCart.DataBind();

                        // Footer
                        int itemCount = dt.Rows.Count;
                        decimal total = 0;
                        foreach (DataRow row in dt.Rows)
                            total += Convert.ToDecimal(row["price"]) * Convert.ToDecimal(row["qty"]);


                        hd_tamount.Value = total.ToString();
                        txtcost.Text = total.ToString("N2");
                        lblFooter.Text = $"Total Items: {itemCount} | Total: ₱{total:N2}";
                    }
                }
            }
        }


        string GenerateInvoiceNo()
        {
            // Auto-generate invoice number (example logic)
            return "BILL" + DateTime.Now.Ticks.ToString().Substring(10);
        }

        void LoadCustomers()
        {
            ddlCustomer.Items.Clear();
            ddlCustomer.Items.Add(new ListItem("Walk-in", "0"));


            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT DISTINCT client_name, userid FROM bookings ORDER BY client_name ASC";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string fullName = reader["client_name"].ToString();
                          string customerid = reader["userid"].ToString();
                        ddlCustomer.Items.Add(new ListItem(fullName, customerid)); // Set both text and value to FullName
                    }
                }
                catch (Exception ex)
                {
                    AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
                    // Handle error (log it, show message, etc.)
                }
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


                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        String cb = "Delete from billitems where billtemid = " + hd_idselect.Value + "";
                        cmd.CommandText = cb;
                        cmd.Connection = conn;

                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (result >= 1)
                        {

                            AlertNotify.ShowMessage(this, "Successfully Deleted!", "Success", AlertNotify.MessageType.Success);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modalPopUp_Delete').modal('hide')", true);
                            LoadCartItems();

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
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn_select = (LinkButton)sender;
                GridViewRow item = (GridViewRow)btn_select.NamingContainer;
                HiddenField hd_idselect = (HiddenField)item.FindControl("hd_id");
                HiddenField hd_name = (HiddenField)item.FindControl("hd_name");


                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * from billitems WHERE billtemid = @id", con);
                    cmd.Parameters.AddWithValue("@id", hd_idselect.Value);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {



                        hd_cartid.Value = dr["billtemid"] != DBNull.Value ? dr["billtemid"].ToString() : "0";

                        ddlChargeName.SelectedValue = dr["name"] != DBNull.Value ? dr["name"].ToString() : "";

                        txtPrice.Text = dr["price"] != DBNull.Value ? Convert.ToDecimal(dr["price"]).ToString() : "0.00";
                        txtQuantity.Text = dr["qty"] != DBNull.Value ? Convert.ToDecimal(dr["qty"]).ToString() : "0";
                        compute_totalamt();
                    }
                    con.Close();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop2", "$('#AddChargeModal').modal('show')", true);
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "EditModal", "openModal(" + hd_idselect.Value + ");", true);
            }
            catch (Exception ex)
            {

                AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            invoiceAddEdit();
        }
        public void invoiceAddEdit()
        {
            // Save invoice and cart items
            using (var con = new MySqlConnection(connStr))
            {
                con.Open();
                MySqlTransaction trx = con.BeginTransaction();

                try
                {
                    // Insert Invoice
                    if (hd_invid.Value == "0")
                    {
                        string invoiceNo = txtInvoiceNo.Text.Trim();
                        string query = "INSERT INTO bill (bookingid,billno, billdate, clientid, remarks, cash, `change`) " +
                                       "VALUES (@bookingid, @invoiceno, @invoicedate, @customerid, @remarks, @cash, @change); " +
                                       "SELECT LAST_INSERT_ID();";
                        int insertedInvoiceId = 0;
                        using (var cmd = new MySqlCommand(query, con, trx))
                        {
                            cmd.Parameters.AddWithValue("@invoiceno", invoiceNo);
                            cmd.Parameters.AddWithValue("@invoicedate", txtDate.Text);
                            cmd.Parameters.AddWithValue("@bookingid", hd_bookid.Value);
                            cmd.Parameters.AddWithValue("@customerid", ddlCustomer.SelectedValue);
                            cmd.Parameters.AddWithValue("@remarks", txtRemarks.Text);
                            cmd.Parameters.AddWithValue("@cash", string.IsNullOrWhiteSpace(txtCash.Text) ? "0" : txtCash.Text);
                            cmd.Parameters.AddWithValue("@change", string.IsNullOrWhiteSpace(txtChange.Text) ? "0" : txtChange.Text);

                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                insertedInvoiceId = Convert.ToInt32(result);
                                hd_invid.Value = insertedInvoiceId.ToString();
                                AlertNotify.ShowMessage(this, "Billing submitted successfully!", "Success", AlertNotify.MessageType.Success);
                                btnPrint.Visible = true;
                                btnPrint.NavigateUrl = "~/BillPrint?billid=" + insertedInvoiceId.ToString();
                            }
                            else
                            {
                                AlertNotify.ShowMessage(this, "Failed to submit", "Warning", AlertNotify.MessageType.Warning);
                            }
                        }


                    }
                    else //UPDATE
                    {
                        int invoiceId = Convert.ToInt32(hd_invid.Value);
                        string invoiceNo = txtInvoiceNo.Text.Trim();

                        string updateQuery = @"UPDATE bill 
                           SET billdate = @billdate,
                               clientid = @clientid,
                               remarks = @remarks,
                               cash = @cash,
                               `change` = @change
                           WHERE billid = @billid";

                        using (var cmd = new MySqlCommand(updateQuery, con, trx))
                        {

                            cmd.Parameters.AddWithValue("@billdate", txtDate.Text);
                            cmd.Parameters.AddWithValue("@clientid", ddlCustomer.SelectedValue);
                            cmd.Parameters.AddWithValue("@remarks", txtRemarks.Text);
                            cmd.Parameters.AddWithValue("@cash", string.IsNullOrWhiteSpace(txtCash.Text) ? "0" : txtCash.Text);
                            cmd.Parameters.AddWithValue("@change", string.IsNullOrWhiteSpace(txtChange.Text) ? "0" : txtChange.Text);
                            cmd.Parameters.AddWithValue("@billid", invoiceId);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                AlertNotify.ShowMessage(this, "Billing updated successfully!", "Success", AlertNotify.MessageType.Success);
                            }
                            else
                            {
                                AlertNotify.ShowMessage(this, "Failed to update billing", "Warning", AlertNotify.MessageType.Warning);
                            }
                        }
                    }
                    trx.Commit();

                }
                catch (Exception ex)
                {
                    trx.Rollback();
                    //  Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                    AlertNotify.ShowMessage(this, ex.Message, "Error", AlertNotify.MessageType.Error);
                }
            }
        }
        protected void btnSaveItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlChargeName.SelectedValue) || string.IsNullOrEmpty(txtQuantity.Text))
            {
                // Display validation message
                AlertNotify.ShowMessage(this, "Please Choose item or input quantity!", "Warning", AlertNotify.MessageType.Warning);
                return;
            }
            if (hd_invid.Value == "0")
            {
                invoiceAddEdit();
            }

            using (var con = new MySqlConnection(connStr))
            {
                con.Open();
                if (hd_cartid.Value == "0") //add cart
                {
                    string query = "INSERT INTO billitems (billid, name,description,qty, price, datelog) VALUES (@invid, @chargename, '',@qty, @price, @datelog)";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@invid", hd_invid.Value); // Assuming 0 for unsaved invoices
                        cmd.Parameters.AddWithValue("@chargename", ddlChargeName.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@datelog", DateTime.Now);
                        cmd.Parameters.AddWithValue("@qty", Convert.ToInt32(txtQuantity.Text));
                        int stat = cmd.ExecuteNonQuery();
                        if (stat > 0)
                        {
                            AlertNotify.ShowMessage(this, "Item added successfully!", "Success", AlertNotify.MessageType.Success);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pops2", "$('#AddChargeModal').modal('hide')", true);
                        }
                    }
                }
                else //update cart
                {
                    string updateQuery = @"UPDATE billitems 
                           SET name = @chargename,
                               price = @price,
                               qty = @qty,
                               datelog = @datelog
                           WHERE billtemid = @billtemid";

                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@chargename", ddlChargeName.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@qty", Convert.ToInt32(txtQuantity.Text));
                        cmd.Parameters.AddWithValue("@datelog", DateTime.Now);
                        cmd.Parameters.AddWithValue("@billtemid", Convert.ToInt32(hd_cartid.Value));

                        int stat = cmd.ExecuteNonQuery();
                        if (stat > 0)
                        {
                            AlertNotify.ShowMessage(this, "Item updated successfully!", "Success", AlertNotify.MessageType.Success);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pops3", "$('#AddChargeModal').modal('hide')", true);
                        }
                        else
                        {
                            AlertNotify.ShowMessage(this, "Update failed!", "Warning", AlertNotify.MessageType.Warning);
                        }
                    }
                }
            }

            // Refresh the charges grid or cart display
            LoadCartItems();
        }
        protected void ddlChargeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlChargeName.SelectedValue))
            {
                using (var con = new MySqlConnection(connStr))
                {
                    con.Open();
                    string query = "SELECT price FROM services WHERE name = @chargename and is_active=1";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@chargename", ddlChargeName.SelectedValue);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            txtPrice.Text = Convert.ToDecimal(result).ToString("N2");
                        }
                    }
                }
            }
            else
            {
                txtPrice.Text = string.Empty;
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCartItems();
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            compute_totalamt();
        }
        void compute_totalamt()
        {
            decimal price = 0;
            int qty = 0;

            // Parse values from the textboxes
            decimal.TryParse(txtPrice.Text, out price);
            int.TryParse(txtQuantity.Text, out qty);

            decimal total = price * qty;

            // Set the total to the textbox
            txtTotalAmount.Text = total.ToString("F2");
        }
        protected void txtCash_TextChanged(object sender, EventArgs e)
        {
            computechange();
        }
        void computechange()
        {
            decimal totalAmount = 0;
            decimal cashTendered = 0;

            decimal.TryParse(hd_tamount.Value, out totalAmount);
            decimal.TryParse(txtCash.Text, out cashTendered);

            // Compute change
            decimal change = cashTendered - totalAmount;

            // Prevent negative change (optional)
            if (change < 0)
            {
                change = 0;
            }

            // Display change in txtChange textbox
            txtChange.Text = change.ToString(""); // Format to 2 decimal places
        }
    }
}