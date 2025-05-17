using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace CarRentalSystem
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute("adminr1", "AdminHome", "~/Admin/AdminHome.aspx");
            routes.MapPageRoute("adminr2", "Account", "~/Admin/Account.aspx");
            routes.MapPageRoute("adminr3", "Cars", "~/Admin/Cars.aspx");
            routes.MapPageRoute("adminr4", "Services", "~/Admin/Services.aspx");
            routes.MapPageRoute("adminr5", "Contacts", "~/Admin/Contacts.aspx");
             routes.MapPageRoute("adminr6", "Billing", "~/Admin/Billing.aspx");
            routes.MapPageRoute("adminr7", "BillingRecords", "~/Admin/BillingRecord.aspx");

            routes.MapPageRoute("customer1", "MyReservation", "~/Customer/CustomerHome.aspx");
            routes.MapPageRoute("customer2", "AddReservation", "~/Customer/AddReservation.aspx");

            routes.MapPageRoute("shared1", "ChangePass", "~/shared/ChangePass.aspx");

            routes.MapPageRoute("shared2", "CarSchedule", "~/shared/CalendarSchedule.aspx");

            routes.MapPageRoute("shared3", "BillPrint", "~/shared/BillPrint.aspx");
        }
    }
}
