using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CarRentalSystem
{

    public class Dashboard
    {
        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        public async Task<DataTable> GetAppointmentsAsync()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "SELECT * FROM bookings";  // SQL query to fetch all appointments

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                await Task.Run(() => adapter.Fill(dt)); // Asynchronously fill DataTable
            }

            return dt;
        }
        public async Task<DashboardCounts> GetCountsAsync()
        {
            // Get appointments data from DB asynchronously
            DataTable dt = await GetAppointmentsAsync();

            // Perform the counting asynchronously (e.g., large datasets can be processed in parallel)
            var result = await Task.Run(() =>
            {
                int pendingCount = dt.AsEnumerable().Count(row => row.Field<string>("status") == "pending");
                int approvedCount = dt.AsEnumerable().Count(row => row.Field<string>("status") == "approved");
                int rejectedCount = dt.AsEnumerable().Count(row => row.Field<string>("status") == "rejected");
                int total = pendingCount + approvedCount + rejectedCount;
                return new DashboardCounts
                {
                    Pending = pendingCount,
                    Approved = approvedCount,
                    Rejected = rejectedCount,
                    Total = total
                };
            });

            return result;
        }

        public class DashboardCounts
        {
            public int Pending { get; set; }
            public int Approved { get; set; }
            public int Rejected { get; set; }
            public int Total { get; set; }
        }
    }


}