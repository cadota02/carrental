using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace CarRentalSystem
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            //    bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
            //                    "~/Scripts/WebForms/WebForms.js",
            //                    "~/Scripts/WebForms/WebUIValidation.js",
            //                    "~/Scripts/WebForms/MenuStandards.js",
            //                    "~/Scripts/WebForms/Focus.js",
            //                    "~/Scripts/WebForms/GridView.js",
            //                    "~/Scripts/WebForms/DetailsView.js",
            //                    "~/Scripts/WebForms/TreeView.js",
            //                    "~/Scripts/WebForms/WebParts.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
             "~/Content/AdminLTE/plugins/sweetalert2/sweetalert2.min.js",
            "~/Content/AdminLTE/plugins/toastr/toastr.min.js",
            "~/Scripts/custom/message.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
            "~/Content/AdminLTE/plugins/datatables/jquery.dataTables.js",
           "~/Content/AdminLTE/plugins/datatables-bs4/js/dataTables.bootstrap4.js"
          ));

            // Order is very important for these files to work, they have explicit dependencies
            //bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
            //        "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
            //        "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
            //        "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
            //        "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            //ADMINLTE
            bundles.Add(new StyleBundle("~/Content/adminlte-css").Include(
          "~/Content/AdminLTE/plugins/fontawesome-free/css/all.min.css",
          "~/Content/AdminLTE/plugins/bootstrap/css/bootstrap.min.css",
          "~/Content/AdminLTE/dist/css/adminlte.min.css",
          "~/Content/AdminLTE/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css",
          "~/Content/AdminLTE/plugins/toastr/toastr.min.css",
          "~/Content/AdminLTE/dist/css/custom.css",
          "~/Content/AdminLTE/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
           "~/Content/AdminLTE/plugins/datatables-bs4/css/dataTables.bootstrap4.css"));

            bundles.Add(new ScriptBundle("~/bundles/AdminLte").Include(
          "~/Content/AdminLTE/plugins/jquery/jquery.min.js",
          "~/Content/AdminLTE/plugins/bootstrap/js/bootstrap.bundle.min.js",
          "~/Content/AdminLTE/dist/js/adminlte.min.js",
             "~/Content/AdminLTE/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
              "~/Content/AdminLTE/dist/js/demo.js" ));
        }
    }
}