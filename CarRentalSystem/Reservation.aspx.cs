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

namespace CarRentalSystem
{
    public partial class Reservation : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                linkprint.Visible = false;
                CurrentPage = 0;
                LoadCars();
                txtBookingNo.Text = GenerateBookingNumber();
            }
        }
        protected void btnSelect_Command(object sender, CommandEventArgs e)
        {
            Button btn = (Button)sender;
            string carId = e.CommandArgument.ToString();
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;

            // Find the HiddenField inside the item
            HiddenField hdcarname = (HiddenField)item.FindControl("hdcarname");
            // Store carId in session or redirect with query string
            Session["selected_carid"] = carId;
            txtCar.Text = hdcarname.Value;
            AlertNotify.ShowMessage(this, "You selected car " + hdcarname.Value, "Information", AlertNotify.MessageType.Information);
        }
        private int PageSize = 3;

        private int CurrentPage
        {
            get
            {
                object o = ViewState["CurrentPage"];
                return (o == null) ? 0 : (int)o;
            }
            set { ViewState["CurrentPage"] = value; }
        }

        private DataTable GetCarsData()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "SELECT car_id, car_name, description, image_path FROM cars where is_available=1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void LoadCars()
        {
            DataTable dt = GetCarsData();

            int start = CurrentPage * PageSize;
            DataTable pagedTable = dt.Clone();

            for (int i = start; i < start + PageSize && i < dt.Rows.Count; i++)
            {
                pagedTable.ImportRow(dt.Rows[i]);
            }

            rptCars.DataSource = pagedTable;
            rptCars.DataBind();

            // Enable/Disable navigation buttons
            btnPrev.Enabled = CurrentPage > 0;
            btnNext.Enabled = (CurrentPage + 1) * PageSize < dt.Rows.Count;
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                LoadCars();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            LoadCars();
        }

        private string GenerateBookingNumber()
        {
            string yymm = DateTime.Now.ToString("yyMM");
            int count = 1;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM bookings WHERE bookingno LIKE @prefix";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@prefix", yymm + "%");
                count += Convert.ToInt32(cmd.ExecuteScalar());
            }

            return yymm + count.ToString("D5"); // e.g. 250400001
        }

        protected void btnReserve_Click(object sender, EventArgs e)
        {
            string selectedCarId = Session["selected_carid"].ToString();


            if (string.IsNullOrEmpty(selectedCarId))
            {
                AlertNotify.ShowMessage(this, "Please select a car before proceeding.", "Warning", AlertNotify.MessageType.Warning);
                // You could display an error message if no car selected
                return;
            }


            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string checkUserQuery = "SELECT COUNT(*) FROM bookings WHERE bookingno = @bookingno;";
                using (MySqlCommand checkCmd = new MySqlCommand(checkUserQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@bookingno", txtBookingNo.Text);
                    int bookingnoExist = Convert.ToInt32(checkCmd.ExecuteScalar());
                    conn.Close();
                    if (bookingnoExist > 0)
                    {
                        txtBookingNo.Text = GenerateBookingNumber();
                    }

                    conn.Open();
                    string query = @"INSERT INTO bookings 
                             (bookingno, car_id, booking_date, return_date, status, client_remarks, client_name, client_address, client_contactno, client_email, created_at) 
                                 VALUES 
                             (@bookingno, @car_id, @booking_date, @return_date, 'pending', @client_remarks, @client_name, @client_address, @client_contactno, @client_email, NOW())";

                   
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingno", txtBookingNo.Text);
                        cmd.Parameters.AddWithValue("@car_id", selectedCarId);
                        cmd.Parameters.AddWithValue("@booking_date", txtBookingDate.Text);
                        cmd.Parameters.AddWithValue("@return_date", txtReturnDate.Text);
                        cmd.Parameters.AddWithValue("@client_remarks", txtClientRemarks.Text);
                        cmd.Parameters.AddWithValue("@client_name", txtClientName.Text);
                        cmd.Parameters.AddWithValue("@client_address", txtClientAddress.Text);
                        cmd.Parameters.AddWithValue("@client_contactno", txtClientContact.Text);
                        cmd.Parameters.AddWithValue("@client_email", txtClientEmail.Text);

                        cmd.ExecuteNonQuery();
                    }
                 
                
                    using (MySqlCommand getIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID();", conn))
                    {
                        int ids = Convert.ToInt32(getIdCmd.ExecuteScalar());
                        linkprint.NavigateUrl = "~/PrintReservation?id=" + ids.ToString();
                        linkprint.Visible = true;
                    }
                    AlertNotify.ShowMessage(this, "Successfully Submitted!", "Success", AlertNotify.MessageType.Success);
                }

            }
        }
        public void reset()
        {
            CurrentPage = 0;
            LoadCars();
            txtBookingNo.Text = GenerateBookingNumber();
            txtBookingDate.Text = "";
            txtReturnDate.Text = "";
            txtClientName.Text = "";
            txtClientEmail.Text = "";
            txtClientContact.Text = "";
            txtClientRemarks.Text = "";

        }
      
    }
      
}