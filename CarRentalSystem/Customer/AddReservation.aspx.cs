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

namespace CarRentalSystem.Customer
{
    public partial class AddReservation : System.Web.UI.Page
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
                        AlertNotify.ShowMessage(this, "You must login first!", "Error", AlertNotify.MessageType.Error);
                        FormsAuthentication.RedirectToLoginPage();
                        return;
                    }
                    else
                    {
                        get_userprofile();
                    }
                   
                }
                linkprint.Visible = false;
               
                txtBookingNo.Text = GenerateBookingNumber();
                if (Request.QueryString["id"] != null)
                {
                    string bookid = Request.QueryString["id"];
                    GetBookingForEdit(bookid);
                }
                CurrentPage = 0;
                LoadCars();
            }
          
        }
        private void GetBookingForEdit(string bookid)
        {


            using (MySqlConnection con = new MySqlConnection(connString))
            {
                string query = "SELECT * FROM vw_booking WHERE booking_id = @bookid";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@bookid", bookid);

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    hd_bookid.Value = bookid;
                    txtBookingNo.Text = reader["bookingno"].ToString();
                    // Assuming you have a dropdown or hidden field for car_id
                    hd_carid.Value = reader["car_id"].ToString();
                    txtBookingDate.Text = Convert.ToDateTime(reader["booking_date"]).ToString("yyyy-MM-dd");
                    txtReturnDate.Text = Convert.ToDateTime(reader["return_date"]).ToString("yyyy-MM-dd");
                    txtClientRemarks.Text = reader["client_remarks"].ToString();
                    txtClientAddress.Text = reader["client_address"].ToString();
                    txtClientContact.Text = reader["client_contactno"].ToString();
                    txtClientEmail.Text = reader["client_email"].ToString();
                    txtsearch.Text = reader["car_name"].ToString();
                    txtCar.Text = reader["car_name"].ToString();
                    // If you store client name separately:
                    string clientName = reader["client_name"].ToString();

                    linkprint.NavigateUrl = "~/PrintReservation?id=" + bookid.ToString();
                    linkprint.Visible = true;

                    string firstName = "", middleInitial = "", lastName = "";

                    // Split the name into parts
                    var nameParts = clientName.Split(' ');

                    if (nameParts.Length == 2)
                    {
                        // e.g. "Juan Cruz"
                        firstName = nameParts[0];
                        lastName = nameParts[1];
                    }
                    else if (nameParts.Length == 3)
                    {
                        // e.g. "Juan P. Cruz" or "Juan Pedro Cruz"
                        firstName = nameParts[0];
                        middleInitial = nameParts[1].Substring(0, 1).ToUpper(); // First letter of middle name
                        lastName = nameParts[2];
                    }
                    else if (nameParts.Length >= 4)
                    {
                        // e.g. "Juan Pedro Dela Cruz"
                        firstName = nameParts[0];
                        middleInitial = nameParts[1].Substring(0, 1).ToUpper();
                        // Combine the rest as last name
                        lastName = string.Join(" ", nameParts.Skip(2));
                    }
                    txt_firstname.Text = firstName;
                    txt_mi.Text = middleInitial;
                    txt_lastname.Text = lastName;
                }
                else
                {
                    AlertNotify.ShowMessage(this, "No Record found!", "Warning", AlertNotify.MessageType.Warning);
                }

                reader.Close();
            }
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LoadCars();
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
            hd_carid.Value = carId;
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
                string query = "SELECT car_id, car_name, description, image_path, RatePerDay FROM cars where is_available=1";
                if (txtsearch.Text.Trim() != "")
                {
                    query += @" and
                       (car_name LIKE @search 
                       OR description LIKE @search) ";
                }

                query += "  order by car_name asc ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + txtsearch.Text + "%");
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
         
            try
            {
                int carId = Convert.ToInt32(hd_carid.Value);
                DateTime bookingDate = DateTime.Parse(txtBookingDate.Text);
                DateTime returnDate = DateTime.Parse(txtReturnDate.Text);
                if (bookingDate < DateTime.Today)
                {
                    AlertNotify.ShowMessage(this, "Booking date must be in the future.", "Invalid Date", AlertNotify.MessageType.Error);
                    return;
                }
                if (returnDate < bookingDate)
                {
                    AlertNotify.ShowMessage(this, "Return date must be later than the booking date.", "Invalid Date", AlertNotify.MessageType.Error);
                    return;
                }
                if (!IsCarAvailable(carId, bookingDate, returnDate))
                {
                    AlertNotify.ShowMessage(this, "This car is already booked for the selected date range.", "Warning", AlertNotify.MessageType.Warning);
                    return;
                }
                if (hd_bookid.Value == "0") //add
                {
                    string selectedCarId = Session["selected_carid"].ToString();
                    if (string.IsNullOrEmpty(selectedCarId))
                    {
                        AlertNotify.ShowMessage(this, "Please select a car before proceed.", "Warning", AlertNotify.MessageType.Warning);
                        return;
                    }
                    reserve_Car(userIDS.ToString(), selectedCarId);
                }
                else //update
                {
                   
                    Edit_reservCar();
                }
            }
            catch (Exception ex)
            {
                AlertNotify.ShowMessage(this, ex.Message.ToString(), "Error", AlertNotify.MessageType.Error);
            }
        }
        public void get_userprofile()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    String cb = "select * from users where ID=@userid ";
                    MySqlCommand cmd = new MySqlCommand(cb);
                    cmd.Parameters.AddWithValue("@userid", userIDS);
                    cmd.Connection = conn;
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {

                       txt_firstname.Text= rdr["Firstname"].ToString();
                       txt_lastname.Text = rdr["Lastname"].ToString();
                       txt_mi.Text= rdr["Middlename"].ToString();
                        txtClientAddress.Text = rdr["Address"].ToString();
                        txtClientContact.Text = rdr["ContactNo"].ToString();
                        txtClientEmail.Text = rdr["Email"].ToString();
                     

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
        public void reserve_Car(string userid, string carid)
        {



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
                             (bookingno, car_id, booking_date, return_date, status, client_remarks, client_name, client_address, client_contactno, client_email, created_at, userid) 
                                 VALUES 
                             (@bookingno, @car_id, @booking_date, @return_date, 'pending', @client_remarks, @client_name, @client_address, @client_contactno, @client_email, NOW(), @userid); SELECT LAST_INSERT_ID();";



                    string middleInitial = string.IsNullOrWhiteSpace(txt_mi.Text) || txt_mi.Text.Trim().Length < 1
                       ? ""
                       : txt_mi.Text.Trim().Substring(0, 1) + ". ";

                    string fullName = txt_firstname.Text.Trim() + " " + middleInitial + txt_lastname.Text.Trim();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@bookingno", txtBookingNo.Text);
                        cmd.Parameters.AddWithValue("@car_id", carid);
                        cmd.Parameters.AddWithValue("@booking_date", txtBookingDate.Text);
                        cmd.Parameters.AddWithValue("@return_date", txtReturnDate.Text);
                        cmd.Parameters.AddWithValue("@client_remarks", txtClientRemarks.Text);
                        cmd.Parameters.AddWithValue("@client_name", fullName);
                        cmd.Parameters.AddWithValue("@client_address", txtClientAddress.Text);
                        cmd.Parameters.AddWithValue("@client_contactno", txtClientContact.Text);
                        cmd.Parameters.AddWithValue("@client_email", txtClientEmail.Text);
                        cmd.Parameters.AddWithValue("@userid", userid);
                        object stat = cmd.ExecuteScalar();
                        if (stat !=null)
                        {
                            AlertNotify.ShowMessage(this, "Booking reservation submitted successfully!", "Success", AlertNotify.MessageType.Success);
                            reset();
                            int ids = Convert.ToInt32(stat);
                            linkprint.NavigateUrl = "~/PrintReservation?id=" + ids.ToString();
                            linkprint.Visible = true;
                        }

                        conn.Close();

                    }

                  


                }

            }
        }
        public void Edit_reservCar()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string updateQuery = @"UPDATE bookings 
                       SET car_id = @car_id,
                           booking_date = @booking_date,
                           return_date = @return_date,
                           client_remarks = @client_remarks,
                           client_name = @client_name,
                           client_address = @client_address,
                           client_contactno = @client_contactno,
                           client_email = @client_email
                       WHERE booking_id = @bookid";

            string middleInitial = string.IsNullOrWhiteSpace(txt_mi.Text) || txt_mi.Text.Trim().Length < 1
                ? ""
                : txt_mi.Text.Trim().Substring(0, 1) + ". ";

            string fullName = txt_firstname.Text.Trim() + " " + middleInitial + txt_lastname.Text.Trim();

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@bookid", hd_bookid.Value); // where clause
                    cmd.Parameters.AddWithValue("@car_id", hd_carid.Value);
                    cmd.Parameters.AddWithValue("@booking_date", txtBookingDate.Text);
                    cmd.Parameters.AddWithValue("@return_date", txtReturnDate.Text);
                    cmd.Parameters.AddWithValue("@client_remarks", txtClientRemarks.Text);
                    cmd.Parameters.AddWithValue("@client_name", fullName);
                    cmd.Parameters.AddWithValue("@client_address", txtClientAddress.Text);
                    cmd.Parameters.AddWithValue("@client_contactno", txtClientContact.Text);
                    cmd.Parameters.AddWithValue("@client_email", txtClientEmail.Text);

                    int affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        AlertNotify.ShowMessage(this, "Booking successfully updated!", "Success", AlertNotify.MessageType.Success);
                    }
                    else
                    {
                        AlertNotify.ShowMessage(this, "Booking not found or not updated.", "Warning", AlertNotify.MessageType.Warning);
                    }

                    conn.Close();
                }
            }
        }
        public void reset()
        {
            hd_carid.Value = "0";
            CurrentPage = 0;
            LoadCars();
            txtBookingNo.Text = GenerateBookingNumber();
            txtBookingDate.Text = "";
            txtReturnDate.Text = "";
            get_userprofile();
            txtCar.Text = "";
            Session["selected_carid"] = string.Empty;
        }
      

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#availabilityModal').modal('hide')", true);
        }
        protected bool IsCarAvailable(int carId, DateTime bookingDate, DateTime returnDate)
        {
            bool isAvailable = false;

            string query = @"
        SELECT COUNT(*) FROM bookings 
        WHERE car_id = @carId 
          AND (
            (booking_date <= @returnDate AND return_date >= @bookingDate)
          )";
           if(hd_bookid.Value != "0")
            {
                query += " and booking_id !=" + hd_bookid.Value + " ";
            }
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@carId", carId);
                    cmd.Parameters.AddWithValue("@bookingDate", bookingDate);
                    cmd.Parameters.AddWithValue("@returnDate", returnDate);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    isAvailable = (count == 0); // available if no overlapping bookings
                }
                conn.Close();
            }

            return isAvailable;
        }

    }

}