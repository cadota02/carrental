using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
namespace CarRentalSystem.shared
{
    public partial class CalendarSchedule : System.Web.UI.Page
    {
        public string con = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Dynamically set master page
            get_userrole(Page.User.Identity.Name.ToString());

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
           if(!IsPostBack)
            {
                LoadCarsDropdown();
            }
        }
        private void LoadCarsDropdown()
        {
         

            using (MySqlConnection conn = new MySqlConnection(con))
            {
                conn.Open();
                string query = "SELECT car_id, car_name FROM cars where is_available=1";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    ddlCars.DataSource = reader;
                    ddlCars.DataValueField = "car_id";
                    ddlCars.DataTextField = "car_name";
                    ddlCars.DataBind();
                }
            }

            ddlCars.Items.Insert(0, new ListItem("-- All Cars --", "0"));
        }
        public void get_userrole(string username)
        {
            //try
            //{
            using (MySqlConnection conn = new MySqlConnection(con))
            {
                conn.Open();
                String cb = "select Role from users where Username='" + username + "' ";
                MySqlCommand cmd = new MySqlCommand(cb);
                cmd.Connection = conn;
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {

                    string role = rdr["Role"].ToString();

                    if (role == "Client")
                        MasterPageFile = "~/SiteCustomer.Master";
                    else
                        MasterPageFile = "~/SiteAdmin.master";

                }
                rdr.Close();
                conn.Close();
            }
            //}
            //catch (Exception ex)
            //{
            //    ShowMessage(ex.Message.ToString(), "");

            //}

        }
    }
}