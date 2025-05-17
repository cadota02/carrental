using System;
using System.Data;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Configuration;

namespace CarRentalSystem
{
    public class DBUserdefualt
    {

        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        public static void CreateDefaultAdmin()
        {
            string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connString))
            {
                try
                {
                    con.Open();

                    // Check if an admin user already exists
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = 'admin'";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, con))
                    {
                        int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userCount == 0) // No admin user exists, create one
                        {
                            string defaultUsername = "admin";
                            string defaultPassword = "Admin@123"; // Change this after first login
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(defaultPassword);

                            string insertQuery = "INSERT INTO users (Firstname,Lastname,Middlename,Userposition, username, PasswordHash,Address,ContactNo,Email,Role, IsApproved) " +
                                                 "VALUES ('AdminFirst','AdminLast','AdminMI','Administrator', @username, @password,'','09123213','admin@example.com', 'Admin',1)";

                            using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, con))
                            {
                                insertCmd.Parameters.AddWithValue("@username", defaultUsername);
                                insertCmd.Parameters.AddWithValue("@password", hashedPassword);

                                insertCmd.ExecuteNonQuery();
                                Console.WriteLine("Default admin account created successfully.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Admin account already exists.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public static (int userId, string role) GetUserIdAndRole(string username)
        {
            int userId = 0;
            string role = string.Empty;

            string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "SELECT ID, Role FROM users WHERE Username = @Username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);


                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = Convert.ToInt32(reader["ID"]);
                            role = reader["Role"].ToString();
                        }
                    }
                }
            }

            return (userId, role);
        }
        public static (int userId, string role, string pass) GetUserIdAndRoleVerifypass(string username)
        {
            int userId = 0;
            string role = string.Empty;
            string pass = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "SELECT ID, Role, PasswordHash FROM users WHERE Username = @Username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", username);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = Convert.ToInt32(reader["ID"]);
                            role = reader["Role"].ToString();
                            pass = reader["PasswordHash"].ToString();

                        }
                    }
                }
            }

            return (userId, role, pass);
        }

    }
}