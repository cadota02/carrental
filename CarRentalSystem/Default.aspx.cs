using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace CarRentalSystem
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            testconnection();
        }
        public void testconnection()
        {
            string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open(); // Open the connection

                    if (conn.State == ConnectionState.Open)
                    {
                        Response.Write("Connection is Open!");
                    }
                    else
                    {
                        Response.Write("Connection is Closed!");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            } // Connection automatically closes when using 'using' statement
        }
    }
}