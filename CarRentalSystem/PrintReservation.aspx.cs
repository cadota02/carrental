using System;
using System.Data;
using MySql.Data.MySqlClient;
using iTextSharp.text; //Install-Package itextsharp -Version 5.5.13.3
using iTextSharp.text.pdf;
using iTextSharp.tool.xml; //Install-Package itextsharp.xmlworker
using System.IO;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace CarRentalSystem
{
    public partial class PrintReservation : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    string prid = Request.QueryString["id"].ToString();

                    DataTable userData = GetDataFromDatabase(prid);
                    // Step 2: Generate the PDF report
                    GeneratePDFReport(userData, prid);

                }
            }
        }
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            // Step 1: Fetch data from the MySQL database
            // DataTable userData = GetDataFromDatabase("1");

            // Step 2: Generate the PDF report
            //  GeneratePDFReport(userData, "1");
        }

        private DataTable GetDataFromDatabase(string PRID)
        {

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT * FROM vw_booking where booking_id=@ID";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Add the parameter with the value
                    cmd.Parameters.AddWithValue("@ID", PRID);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }

            }
        }

        private void GeneratePDFReport(DataTable DataDB, string DBID)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                string bookingno = DataDB.Rows[0]["bookingno"].ToString();
                // Step 1: Add header (logo or title)
                AddHeader(document);

                // Step 2: Add body with tables
                AddBody(document, DataDB);

                // Step 3: Add footer
                // AddFooter(document);

                document.Close();

             
                string uniqueId = DBID; // "N" format removes hyphens
                string fileName = $"{bookingno}_{uniqueId}.pdf";

                string filePath = Server.MapPath("~/reports/booking/" + fileName);

                File.WriteAllBytes(filePath, ms.ToArray());

                string filePath2 = ResolveUrl("~/reports/booking/" + fileName);
                pdfframerpt.Attributes["src"] = filePath2;
                pdfframerpt.Style["display"] = "block";

                //// Step 4: Download PDF
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Disposition", "attachment; filename=Report.pdf");
                //Response.BinaryWrite(ms.ToArray());
                //Response.End();
            }
        }

        private void AddHeader(Document document)
        {
            // Add logo (if available) or title
            //string logoPath = Server.MapPath("~/images/logo.png"); // Replace with your logo path
            //Image logo = Image.GetInstance(logoPath);
            //logo.ScaleToFit(100f, 100f);
            //logo.Alignment = Element.ALIGN_CENTER;
            Font regularFont = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL);
            Paragraph printdate = new Paragraph(DateTime.Now.ToString("MMMM dd, yyyy"), regularFont);
            printdate.Alignment = Element.ALIGN_RIGHT;


            Paragraph header = new Paragraph("Car Rental Information System", regularFont);
            header.Alignment = Element.ALIGN_CENTER;

            Font titleFont = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD);
            Paragraph title = new Paragraph("Reservation Receipt", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;

            // document.Add(logo);
            document.Add(printdate);
            document.Add(header);
            document.Add(title);


        }

        private void AddBody(Document document, DataTable DataDB)
        {
            // Create the table for patient profiles
            PdfPTable patientProfileTable = new PdfPTable(4);
            patientProfileTable.SpacingBefore = 10f;  // Space before the table
            patientProfileTable.SpacingAfter = 10f;   // Space after the table
            patientProfileTable.WidthPercentage = 100;
            patientProfileTable.DefaultCell.Padding = 5f;


            //PdfPCell cell = new PdfPCell(new Phrase("Your Content"));
            //cell.Padding = 5f;  // Set padding for this specific cell
            //patientProfileTable.AddCell(cell);

            //sample color??
            //Font cellFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);  // Font color set to BLACK

            //// Create a custom cell with background color
            //PdfPCell cell = new PdfPCell(new Phrase("FULLNAME", cellFont));
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;  // Set background color (light gray)
            //patientProfileTable.AddCell(cell);

            // Add headers for the table
            Font cellFont = FontFactory.GetFont("Arial", 11, Font.NORMAL);
            Font cellFontNine = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            Font cellFontHeader = FontFactory.GetFont("Arial", 11, Font.BOLD);
            patientProfileTable.SetWidths(new float[] { 0.8f, 1.3f, 0.7f, 3f });  // Column 1: 1 part 30%, Column 2: 3 parts 70%
            // Loop through the data and add each row
            foreach (DataRow row in DataDB.Rows)
            {
                patientProfileTable.AddCell(new Phrase("Reservation No: ", cellFont));
                patientProfileTable.AddCell(new Phrase(row["bookingno"].ToString(), cellFontHeader));
                patientProfileTable.AddCell(new Phrase("Name", cellFont));
                patientProfileTable.AddCell(new Phrase(row["client_name"].ToString(), cellFontHeader));
                patientProfileTable.AddCell(new Phrase("Address", cellFont));
                patientProfileTable.AddCell(new Phrase(row["client_address"].ToString(), cellFont));
                patientProfileTable.AddCell(new Phrase("Contact No.", cellFontNine));
                patientProfileTable.AddCell(new Phrase(row["client_contactno"].ToString(), cellFont));
                patientProfileTable.AddCell(new Phrase("Email", cellFontNine));
                patientProfileTable.AddCell(new Phrase(row["client_email"].ToString(), cellFont));
            }

            Font titleFontU = new Font(Font.FontFamily.HELVETICA, 12, Font.UNDERLINE);
            Font titleFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
            Font fonttable = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);
            // Add the patient profile table to the document
            document.Add(new Paragraph("Client Details:", titleFont));
            document.Add(patientProfileTable);

            // Create the second table for medical records (you can modify this based on your actual data)
            PdfPTable medicalRecordTable = new PdfPTable(2);
            medicalRecordTable.WidthPercentage = 100;
            medicalRecordTable.SpacingBefore = 10f;  // Space before the table
            medicalRecordTable.SpacingAfter = 10f;   // Space after the table
            medicalRecordTable.WidthPercentage = 100;
            medicalRecordTable.DefaultCell.Padding = 5f;

            // Loop through medical records data (adjust this based on your schema)
            foreach (DataRow row in DataDB.Rows)
            {
                medicalRecordTable.AddCell("Date Book");
                medicalRecordTable.AddCell(Convert.ToDateTime(row["booking_date"]).ToString("MMMM dd, yyyy"));
                medicalRecordTable.AddCell("Return Date");
                medicalRecordTable.AddCell(Convert.ToDateTime(row["return_date"]).ToString("MMMM dd, yyyy"));
                medicalRecordTable.AddCell("Remarks");
                medicalRecordTable.AddCell(row["client_remarks"].ToString());
                medicalRecordTable.AddCell("Car details");
                string htmlContent = row["description"].ToString();
                string imagePath = Server.MapPath(row["image_path"].ToString().Substring(2)); // Remove "~/" or first 2 characters
                if (File.Exists(imagePath))
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(150f, 100f); // Adjust size
                    img.SpacingBefore = 5f;
                    img.SpacingAfter = 5f;
                    PdfPCell imageCell = new PdfPCell(img);
                   imageCell.Colspan = 2;
                    imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //   imageCell.Border = Rectangle.NO_BORDER;
                    medicalRecordTable.AddCell(imageCell);
                }
                medicalRecordTable.AddCell("");
                medicalRecordTable.AddCell(ConvertHtmlToPlainText(row["description"].ToString())); 
              
             



            }

            // Add the medical record table to the document
            document.Add(new Paragraph("\n Reservation Details:", titleFont));
            document.Add(medicalRecordTable);




            // document.Add(new Paragraph("__________________", fonttable));
            // document.Add(new Paragraph("__________________", fonttable));
        }

        private void AddFooter(Document document)
        {
            Font titleFont = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.GRAY);

            Paragraph footer = new Paragraph("Car Rental Information System", titleFont);
            footer.Alignment = Element.ALIGN_CENTER;

            document.Add(footer);
        }
        public static string ConvertHtmlToPlainText(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";

            // Decode HTML entities like &amp;, &bull;
            string decoded = HttpUtility.HtmlDecode(html);

            // Replace bullet points & line breaks
            decoded = decoded.Replace("<li>", "• ")
                             .Replace("</li>", "\n")
                             .Replace("<br />", "\n")
                             .Replace("<br>", "\n")
                             .Replace("<ul>", "")
                             .Replace("</ul>", "");

            // Optionally remove any remaining tags
            decoded = Regex.Replace(decoded, "<.*?>", ""); // removes any leftover HTML tags

            return decoded.Trim();
        }
        private PdfPCell GetHtmlAsPdfCell(string htmlContent)
        {
            using (var htmlStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlContent)))
            {
                var doc = new Document();
                var output = new MemoryStream();
                var writer = PdfWriter.GetInstance(doc, output);
                doc.Open();

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, htmlStream, null, Encoding.UTF8, new XMLWorkerFontProvider());

                doc.Close();

                string parsedText = Encoding.UTF8.GetString(output.ToArray());
                PdfPCell cell = new PdfPCell(new Phrase(parsedText));
                return cell;
            }
        }
    }
}