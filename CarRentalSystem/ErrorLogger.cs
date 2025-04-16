using System;
using System.IO;
using System.Web;


namespace CarRentalSystem
{
    public class ErrorLogger
    {
        public static void WriteErrorLog(Exception ex)
        {
            try
            {
                // Get the webpage name safely
                string webPageName = HttpContext.Current?.Request?.Path != null
                    ? Path.GetFileName(HttpContext.Current.Request.Path)
                    : "UnknownPage";

                // Create a unique log filename per day
                string errorLogFilename = $"ErrorLog_{webPageName}_{DateTime.Now:dd-MM-yyyy}.txt";

                // Define the log directory and ensure it exists
                string logDirectory = HttpContext.Current.Server.MapPath("~/log/");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Full path for the log file
                string logFilePath = Path.Combine(logDirectory, errorLogFilename);

                // Append error details to the log file
                using (StreamWriter stwriter = new StreamWriter(logFilePath, true))
                {
                    stwriter.WriteLine("-------------------------------------------------");
                    stwriter.WriteLine($"Error Log - {DateTime.Now:hh:mm tt}");
                    stwriter.WriteLine($"WebPage Name: {webPageName}");
                    stwriter.WriteLine($"Message: {ex.Message}");
                    stwriter.WriteLine($"Stack Trace: {ex.StackTrace}");
                    stwriter.WriteLine("-------------------------------------------------");
                }
            }
            catch (Exception logEx)
            {
                // Log to Event Viewer or another fallback logging mechanism
                System.Diagnostics.Debug.WriteLine("Failed to write error log: " + logEx.Message);
            }
        }
    }
}