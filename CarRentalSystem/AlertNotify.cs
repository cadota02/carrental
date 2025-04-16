using System;
using System.Web.UI;

namespace CarRentalSystem
{
    public static class AlertNotify
    {
        // Enum for Message Types
        public enum MessageType { Success, Error, Warning, Information }

        // Enum for SweetAlert Types
        public enum Sweet_Alert_Type { basic = 1, success = 2, warning = 3, error = 4 }

        /// <summary>
        /// Displays a JavaScript alert message using the 'MessageShow' function.
        /// </summary>
        /// <param name="page">The current page instance</param>
        /// <param name="msg">Message text</param>
        /// <param name="title">Message title</param>
        /// <param name="type">MessageType (Success, Error, etc.)</param>
        public static void ShowMessage(Page page, string msg, string title, MessageType type)
        {
            if (page == null) return;

            string script = $"<script>MessageShow('{msg}', '{title}', '{type}');</script>";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "msg1", script, false);
        }

        /// <summary>
        /// Displays a SweetAlert popup.
        /// </summary>
        /// <param name="page">The current page instance</param>
        /// <param name="msg">Message text</param>
        /// <param name="title">Message title</param>
        /// <param name="type">Sweet_Alert_Type (basic, success, warning, error)</param>
        public static void SweetAlert(Page page, string msg, string title, Sweet_Alert_Type type)
        {
            if (page == null) return;

            string str_alert_Msg = @" swal.fire({
                                    type: '" + type + @"',
                                    title:'" + title + @"',
                                    text: '" + msg + @"',
                                    confirmButtonColor: '#DD6B55',
                                    animation: 'slide-from-top',
                                    allowEscapeKey: true,
                                    allowOutsideClick: true
                                
                               });";

            ScriptManager.RegisterStartupScript(page, page.GetType(), "PopupAlert", str_alert_Msg, true);
        }
    }

}