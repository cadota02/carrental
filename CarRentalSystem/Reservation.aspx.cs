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

            string selectedCarId = Session["selected_carid"].ToString();
            if (string.IsNullOrEmpty(selectedCarId))
            {
                AlertNotify.ShowMessage(this, "Please select a car before proceed.", "Warning", AlertNotify.MessageType.Warning);
                return;
            }

            try
            {
                int userid = 0;
                if (IsExists(txt_username.Text, "Username"))
                {

                    var result = DBUserdefualt.GetUserIdAndRoleVerifypass(txt_username.Text);
                    int GetExistinguserId = result.userId;
                    string pass = result.pass;
                    if (BCrypt.Net.BCrypt.Verify(txt_password.Text, pass))
                    {
                        reserve_Car(GetExistinguserId.ToString(), selectedCarId);
                    }
                    else
                    {
                        AlertNotify.ShowMessage(this, "Incorrect password. If you don’t have an account yet, please register using a different username.", "Warning", AlertNotify.MessageType.Warning);

                        return;
                    }

                }
                else
                {
                    if ((userid = SaveUser()) > 0)
                    {
                        reserve_Car(userid.ToString(), selectedCarId);

                    }
                    else
                    {
                        AlertNotify.ShowMessage(this, "Failed to register account!", "Warning", AlertNotify.MessageType.Warning);
                    }

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
                             (@bookingno, @car_id, @booking_date, @return_date, 'pending', @client_remarks, @client_name, @client_address, @client_contactno, @client_email, NOW(), @userid)";



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
                        int stat = cmd.ExecuteNonQuery();
                        if (stat > 0)
                        {
                            AlertNotify.ShowMessage(this, "Booking reservation submitted successfully!","Success", AlertNotify.MessageType.Success);
                           
                        }
                      
                        conn.Close();
                     
                    }

                    using (MySqlCommand getIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID();", conn))
                    {
                        int ids = Convert.ToInt32(getIdCmd.ExecuteScalar());
                        linkprint.NavigateUrl = "~/PrintReservation?id=" + ids.ToString();
                        linkprint.Visible = true;
                    }


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
            txt_lastname.Text = "";
            txt_firstname.Text = "";
            txt_mi.Text = "";
            txtClientEmail.Text = "";
            txtClientContact.Text = "";
            txtClientRemarks.Text = "";

        }
        private int SaveUser()
        {
            int userid = 0;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO users 
                                (Firstname, Lastname, Middlename, UserPosition, Username, PasswordHash, Address, ContactNo, Email, Role, IsApproved)
                                VALUES
                                (@Firstname, @Lastname, @Middlename, @UserPosition, @Username, @PasswordHash, @Address, @ContactNo, @Email, @Role, @IsApproved);  SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Firstname", txt_firstname.Text.Trim());
                    cmd.Parameters.AddWithValue("@Lastname", txt_lastname.Text.Trim());
                    cmd.Parameters.AddWithValue("@Middlename", txt_mi.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserPosition", "Customer");
                    cmd.Parameters.AddWithValue("@Username", txt_username.Text.Trim());

                    // Hash password before saving

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txt_password.Text.Trim());
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    cmd.Parameters.AddWithValue("@Address", txtClientAddress.Text);
                    cmd.Parameters.AddWithValue("@ContactNo", txtClientContact.Text);
                    cmd.Parameters.AddWithValue("@Email", txtClientEmail.Text);
                    cmd.Parameters.AddWithValue("@Role", "Client"); // e.g., Admin or Client
                    cmd.Parameters.AddWithValue("@IsApproved", 0);

                    object result = cmd.ExecuteScalar();
                    int insertedUserId = Convert.ToInt32(result);


                    if (insertedUserId > 0)
                    {
                        userid = insertedUserId;


                    }
                    else
                    {
                        userid = 0;


                    }
                }
                catch (Exception ex)
                {

                    AlertNotify.ShowMessage(this, "Error: " + ex.Message, "Error", AlertNotify.MessageType.Error);
                }
            }
            return userid;
        }
        private bool IsExists(string name, string column)
        {

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM users WHERE " + column + " = @column";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@column", name);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

       
        protected void btnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#availabilityModal').modal('hide')", true);
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LoadCars();
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