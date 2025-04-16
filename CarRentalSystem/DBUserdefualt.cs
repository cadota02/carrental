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

    }
}