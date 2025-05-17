using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web;
using System.Configuration;
namespace CarRentalSystem
{
    public class UserHelper
    {
        public static string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        public static int GetCurrentUserId()
        {
            // Check session cache first
            if (HttpContext.Current.Session["UserId"] != null)
            {
                return Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            }

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return -1;

            string username = HttpContext.Current.User.Identity.Name;
            int userId = GetUserIdFromDatabase(username);

            // Cache in session
            if (userId != -1)
            {
                HttpContext.Current.Session["UserId"] = userId;
            }

            return userId;
        }

        public static int GetUserIdFromDatabase(string username)
        {
            int userId = -1;


            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "SELECT ID FROM users WHERE Username = @username LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
            }

            return userId;

        }
       
    }
}