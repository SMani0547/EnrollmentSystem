using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using iTextSharp.text;
using iTextSharp.text.pdf;
using USPEducation.Models;
using USPEducation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace USPEducation.Controllers
{
    public class FinanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult DownloadFinanceReport()
        {
            string studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var studentFinance = _context.StudentFinances
                .FirstOrDefault(s => s.StudentID == studentId);

            if (studentFinance == null)
            {
                return NotFound("Finance record not found");
            }

            var enrollments = _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .ToList();

            decimal totalFee = enrollments.Sum(e => e.Course.Fee);
            decimal amountPaid = studentFinance.AmountPaid;
            decimal outstandingBalance = totalFee - amountPaid; // Calculate the outstanding balance
            string invoiceNumber = "INV-" + new Random().Next(100000, 999999);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // 📌 Add Header Image (Full Width)
                string headerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceImage", "Invoiceheader.png");

                if (System.IO.File.Exists(headerImagePath))
                {
                    Image headerImage = Image.GetInstance(headerImagePath);
                    headerImage.ScaleToFit(500, 100);
                    headerImage.Alignment = Element.ALIGN_CENTER;
                    document.Add(headerImage);
                }

                document.Add(new Paragraph("\n")); // Space after header

                // 📌 Invoice Title
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22);
                Paragraph title = new Paragraph("Student Finance Invoice", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(title);
                document.Add(new Paragraph("\n"));

                // 📌 Invoice Number
                Font invoiceFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                document.Add(new Paragraph($"Invoice Number: {invoiceNumber}", invoiceFont));
                document.Add(new Paragraph("\n"));

                // 📌 Student Information Table
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);
                PdfPTable studentInfoTable = new PdfPTable(2) { WidthPercentage = 100, SpacingBefore = 10 };
                studentInfoTable.SetWidths(new float[] { 30f, 70f });

                studentInfoTable.AddCell(new PdfPCell(new Phrase("Student ID:", headerFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 8 });
                studentInfoTable.AddCell(new PdfPCell(new Phrase(studentFinance.StudentID, FontFactory.GetFont(FontFactory.HELVETICA, 14))) { Padding = 8 });

                studentInfoTable.AddCell(new PdfPCell(new Phrase("Last Updated:", headerFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 8 });
                studentInfoTable.AddCell(new PdfPCell(new Phrase(studentFinance.LastUpdated.ToString("dd-MM-yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 14))) { Padding = 8 });

                document.Add(studentInfoTable);
                document.Add(new Paragraph("\n"));

                // 📌 Course Table
                PdfPTable table = new PdfPTable(3) { WidthPercentage = 100, SpacingBefore = 10 };
                table.SetWidths(new float[] { 20f, 60f, 20f });

                Font tableHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);
                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                table.AddCell(new PdfPCell(new Phrase("Course Code", tableHeaderFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 10 });
                table.AddCell(new PdfPCell(new Phrase("Title", tableHeaderFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 10 });
                table.AddCell(new PdfPCell(new Phrase("Fee ($)", tableHeaderFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 10 });

                foreach (var enrollment in enrollments)
                {
                    table.AddCell(new PdfPCell(new Phrase(enrollment.Course.CourseCode, cellFont)) { Padding = 10 });
                    table.AddCell(new PdfPCell(new Phrase(enrollment.Course.Title, cellFont)) { Padding = 10 });
                    table.AddCell(new PdfPCell(new Phrase(enrollment.Course.Fee.ToString("F2"), cellFont)) { Padding = 10 });
                }

                document.Add(table);
                document.Add(new Paragraph("\n"));

                // 📌 Total Fee Section (Moved to the Bottom)
                Font totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph totalFeeText = new Paragraph($"Total Fees: ${totalFee:F2}", totalFont)
                {
                    Alignment = Element.ALIGN_RIGHT
                };
                document.Add(totalFeeText);

                // 📌 Outstanding Balance Section
                Paragraph outstandingBalanceText = new Paragraph($"Balance: ${outstandingBalance:F2}", totalFont)
                {
                    Alignment = Element.ALIGN_RIGHT
                };
                document.Add(outstandingBalanceText);

                document.Add(new Paragraph("\n"));

                // 📌 Footer Image (Larger Size)
                string footerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceImage", "Invoicefooter.png");

                if (System.IO.File.Exists(footerImagePath))
                {
                    Image footerImage = Image.GetInstance(footerImagePath);
                    footerImage.ScaleToFit(500, 150); // Increased size for better visibility
                    footerImage.Alignment = Element.ALIGN_CENTER;
                    document.Add(footerImage);
                }
                // 📌 Add a New Page and Add Financial Detail Image
                document.NewPage(); // This will start a new page

                string financialDetailImagePath = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceImage", "financialdetail.png");

                if (System.IO.File.Exists(financialDetailImagePath))
                {
                    Image financialDetailImage = Image.GetInstance(financialDetailImagePath);

                    // Scale the image to fit the A4 page width and height
                    financialDetailImage.ScaleToFit(PageSize.A4.Width - 20, PageSize.A4.Height - 20); // Add some padding around the image

                    // Center the image on the page
                    financialDetailImage.Alignment = Element.ALIGN_CENTER;

                    // Add the image to the document
                    document.Add(financialDetailImage);
                }

                document.Close();
                writer.Close();

                return File(memoryStream.ToArray(), "application/pdf", "Student_Finance_Invoice.pdf");
            }
        }


    }
}
