using System.Security.Claims;
using iTextSharp.text;
using iTextSharp.text.pdf;
using USPSystem.Models;
using USPSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using USPSystem.Services;

namespace USPSystem.APIController;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class InvoiceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IStudentFinanceService _financeService;

    public InvoiceController(ApplicationDbContext context, IStudentFinanceService financeService)
    {
        _context = context;
        _financeService = financeService;
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadFinanceReport()
    {
        string studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == studentId);

        if (currentUser == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var studentFinance = await _financeService.GetStudentFinanceWithDetailsAsync(studentId);

        if (studentFinance == null)
        {
            return NotFound(new { message = "Finance record not found" });
        }

        var enrollments = _context.StudentEnrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Course)
            .ToList();

        decimal totalFee = enrollments.Sum(e => e.Course.Fees ?? 0);
        decimal amountPaid = studentFinance.AmountPaid;
        decimal outstandingBalance = totalFee - amountPaid;
        string invoiceNumber = "INV-" + new Random().Next(100000, 999999);

        using (MemoryStream memoryStream = new MemoryStream())
        {
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            string headerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Invoiceheader.png");
            if (System.IO.File.Exists(headerImagePath))
            {
                Image headerImage = Image.GetInstance(headerImagePath);
                headerImage.ScaleToFit(500, 100);
                headerImage.Alignment = Element.ALIGN_CENTER;
                document.Add(headerImage);
            }

            document.Add(new Paragraph("\n"));
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22);
            document.Add(new Paragraph("Student Finance Invoice", titleFont) { Alignment = Element.ALIGN_CENTER });
            document.Add(new Paragraph($"\nInvoice Number: {invoiceNumber}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)));

            PdfPTable studentInfoTable = new PdfPTable(2) { WidthPercentage = 100, SpacingBefore = 10 };
            studentInfoTable.SetWidths(new float[] { 30f, 70f });

            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);
            studentInfoTable.AddCell(new PdfPCell(new Phrase("Student ID:", headerFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 8 });
            studentInfoTable.AddCell(new PdfPCell(new Phrase(currentUser.StudentId, FontFactory.GetFont(FontFactory.HELVETICA, 14))) { Padding = 8 });

            studentInfoTable.AddCell(new PdfPCell(new Phrase("Last Updated:", headerFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 8 });
            studentInfoTable.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd-MM-yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 14))) { Padding = 8 });

            document.Add(studentInfoTable);
            document.Add(new Paragraph("\n"));

            PdfPTable table = new PdfPTable(3) { WidthPercentage = 100, SpacingBefore = 10 };
            table.SetWidths(new float[] { 20f, 60f, 20f });

            var tableHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            table.AddCell(new PdfPCell(new Phrase("Course Code", tableHeaderFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 10 });
            table.AddCell(new PdfPCell(new Phrase("Title", tableHeaderFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 10 });
            table.AddCell(new PdfPCell(new Phrase("Fee ($)", tableHeaderFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 10 });

            foreach (var enrollment in enrollments)
            {
                table.AddCell(new PdfPCell(new Phrase(enrollment.Course.Code, cellFont)) { Padding = 10 });
                table.AddCell(new PdfPCell(new Phrase(enrollment.Course.Name, cellFont)) { Padding = 10 });
                table.AddCell(new PdfPCell(new Phrase((enrollment.Course.Fees ?? 0).ToString("F2"), cellFont)) { Padding = 10 });
            }

            document.Add(table);
            document.Add(new Paragraph("\n"));

            var totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            document.Add(new Paragraph($"Total Fees: ${totalFee:F2}", totalFont) { Alignment = Element.ALIGN_RIGHT });
            document.Add(new Paragraph($"Balance: ${outstandingBalance:F2}", totalFont) { Alignment = Element.ALIGN_RIGHT });
            document.Add(new Paragraph("\n"));

            string footerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Invoicefooter.png");
            if (System.IO.File.Exists(footerImagePath))
            {
                Image footerImage = Image.GetInstance(footerImagePath);
                footerImage.ScaleToFit(500, 150);
                footerImage.Alignment = Element.ALIGN_CENTER;
                document.Add(footerImage);
            }

            document.NewPage();
            string financialDetailImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "financialdetail.png");
            if (System.IO.File.Exists(financialDetailImagePath))
            {
                Image financialDetailImage = Image.GetInstance(financialDetailImagePath);
                financialDetailImage.ScaleToFit(PageSize.A4.Width - 20, PageSize.A4.Height - 20);
                financialDetailImage.Alignment = Element.ALIGN_CENTER;
                document.Add(financialDetailImage);
            }

            document.Close();
            writer.Close();

            return File(memoryStream.ToArray(), "application/pdf", "Student_Finance_Invoice.pdf");
        }
    }
}
