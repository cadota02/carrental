using System;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Globalization;

namespace CarRentalSystem.shared
{
    /// <summary>
    /// Summary description for AvailiabityCarsHandler
    /// </summary>
    public class CarSchedule : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder json = new StringBuilder();
            json.Append("[");
            string carId = context.Request.QueryString["carid"];
           
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT booking_date,return_date, car_name, client_name, car_id,booking_id, status, formatted_date, duration_days " +
                    "FROM vw_booking WHERE status !='rejected'";
                if(carId !="0")
                {
                    query += " and car_id=@carid";
                }
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (carId != "0")
                    {
                        cmd.Parameters.AddWithValue("@carid", carId);
                    }
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        bool first = true;
                        while (rdr.Read())
                        {
                            if (!first) json.Append(",");
                            //json.Append("{");
                            //json.AppendFormat("\"title\":\"{0}\",", rdr["Status"].ToString() + " " + rdr["Fullname"].ToString());
                            //json.AppendFormat("\"start\":\"{0}\"", Convert.ToDateTime(rdr["AppointmentDateTime"]).ToString("yyyy-MM-ddTHH:mm:ss"));
                            //json.Append("}");
                            string status = rdr["status"].ToString();
                            string color = (status == "approved") ? "green" : "orange";

                            string tooltip = $"Car: {rdr["car_name"]}, Customer: {rdr["client_name"]}, Date booked: {rdr["formatted_date"]}, duration: {rdr["duration_days"]}, Status: {status}";
                            DateTime bookingDate = Convert.ToDateTime(rdr["booking_date"]);
                            DateTime returnDate = Convert.ToDateTime(rdr["return_date"]).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                            // Escape double quotes in tooltip
                            tooltip = tooltip.Replace("\"", "\\\"");

                            json.Append("{");
                            json.AppendFormat("\"title\":\"{0}\",", rdr["car_name"].ToString() + ", customer " + rdr["client_name"].ToString() + " (" + rdr["duration_days"].ToString() + " day/s) ");
                            //json.AppendFormat("\"start\":\"{0}\",", Convert.ToDateTime(rdr["booking_date"]).ToString("yyyy-MM-ddT"));
                            //json.AppendFormat("\"end\":\"{0}\",", Convert.ToDateTime(rdr["return_date"]).ToString("yyyy-MM-ddT"));
                            json.AppendFormat("\"start\":\"{0:yyyy-MM-ddTHH:mm:ss}\",", bookingDate);
                            json.AppendFormat("\"end\":\"{0:yyyy-MM-ddTHH:mm:ss}\",", returnDate);
                            json.AppendFormat("\"color\":\"{0}\",", color);
                            json.AppendFormat("\"extendedProps\":{{\"tooltip\":\"{0}\"}}", tooltip);
                            json.Append("}");

                            first = false;
                        }
                    }
                }
            }

            json.Append("]");
            context.Response.Write(json.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}