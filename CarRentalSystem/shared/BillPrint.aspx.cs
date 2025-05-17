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

namespace CarRentalSystem.shared
{
    public partial class BillPrint : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string invid = Request.QueryString["billid"];
                if (!string.IsNullOrEmpty(invid))
                {
                    LoadInvoice(invid);
                }
            }
        }

        private void LoadInvoice(string invid)
        {

            string invoiceno = "";
            string dateinvoice = "";
            string customer = "";
            string cash = "";
            string change = "";
            string remarks = "";

            string query1 = "SELECT * FROM vw_bill WHERE billid = @billid";

            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query1, con))
                {
                    cmd.Parameters.AddWithValue("@billid", invid);
                    con.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            invoiceno = reader["billno"].ToString();
                            dateinvoice = Convert.ToDateTime(reader["billdate"]).ToString("MMMM dd, yyyy");
                            customer = reader["fullname"].ToString();
                            cash = reader["cash"].ToString();
                            change = reader["change"].ToString();
                            remarks = reader["remarks"].ToString();
                        }
                    }
                }
            }

            string html = @"<div style='text-align:center;'>
                        <p>Car Rental System</p>    
                     
                        <h4>Car Bill</h4></div>";
            html += "<p><b>Date: </b>" + dateinvoice + "</p>";
            html += "<p><b>Billing #: </b>" + invoiceno + "</p>";

            html += "<p><b>Customer: </b>" + customer + "</p>";
            html += "<p><b>Cash: </b>" + cash + "</p>";
            html += "<p><b>Change: </b>" + change + "</p>";
            html += "<p><b>Remarks: </b>" + remarks + "</p>";
            html += "<table border='1' width='100%' cellpadding='5' cellspacing='0'><tr><th>Item/Description</th><th>Qty</th><th>Price</th><th>Total</th></tr>";
            int countitems = 0;
            decimal amounttotal = 0;
            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                con.Open();
                string query = "SELECT *, (qty * price) as amount FROM billitems WHERE billid = @invid";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@invid", invid);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            countitems++;
                            amounttotal += Convert.ToDecimal(rdr["amount"]);
                            html += $"<tr><td>{rdr["name"]}</td><td>{rdr["qty"]}</td><td>{rdr["price"]:N2}</td><td>{rdr["amount"]:N2}</td></tr>";
                        }
                    }
                }
            }
            html += $"<tr><td style='colspan=2'><strong>Total Item/s:</strong> " + countitems.ToString("N0") + " </td><td></td><td></td><td><strong>₱" + amounttotal.ToString("N2") + "</strong></td></tr>";
            html += "</table>";
            //     html += "<br/><p><strong>Total Items:</strong> " + countitems.ToString("N0")+ " | <strong>Total:</strong> ₱" + amounttotal.ToString("N2") + "</p>";
            html += "<br/><p style='text-align: center; font-style: italic;'>Thank you for your payment. This serves as your official billing statement.</p>";
            litInvoiceHtml.Text = html;
        }
    }
}