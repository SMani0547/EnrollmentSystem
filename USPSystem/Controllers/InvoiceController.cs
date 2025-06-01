using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using iTextSharp.text;
using iTextSharp.text.pdf;
using USPSystem.Models;
using USPSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using USPSystem.Services;
using Microsoft.AspNetCore.Identity;
using USPFinance.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace USPSystem.Controllers;

[Authorize(Roles = "Student")]
public class InvoiceController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly IStudentFinanceService _financeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(
        ApplicationDbContext context, 
        IStudentFinanceService financeService, 
        StudentHoldService studentHoldService,
        PageHoldService pageHoldService,
        UserManager<ApplicationUser> userManager,
        ILogger<InvoiceController> logger)
        : base(studentHoldService, pageHoldService, userManager)
    {
        _context = context;
        _financeService = financeService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ActionResult> DownloadFinanceReport()
    {
        string studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation("Starting invoice generation for student {StudentId}", studentId);

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == studentId);

        if (currentUser == null)
        {
            _logger.LogWarning("User not found for ID {StudentId}", studentId);
            return NotFound("User not found");
        }

        _logger.LogInformation("Found user with StudentId: {StudentId}", currentUser.StudentId);

        var studentFinanceDetails = await _financeService.GetStudentFinanceWithDetailsAsync(currentUser.StudentId);

        if (studentFinanceDetails == null)
        {
            _logger.LogWarning("Finance record not found for student {StudentId}", currentUser.StudentId);
            return NotFound("Finance record not found");
        }

        _logger.LogInformation("Retrieved finance details for student {StudentId}. TotalFees: {TotalFees}, AmountPaid: {AmountPaid}", 
            currentUser.StudentId, studentFinanceDetails.TotalFees, studentFinanceDetails.AmountPaid);

        var enrollments = _context.StudentEnrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Course)
            .ToList();

        _logger.LogInformation("Found {Count} enrollments for student {StudentId}", enrollments.Count, studentId);

        decimal totalFee = enrollments.Sum(e => e.Course.Fees ?? 0);
        decimal amountPaid = studentFinanceDetails.AmountPaid;
        decimal outstandingBalance = totalFee - amountPaid;
        string invoiceNumber = "INV-" + new Random().Next(100000, 999999);

        _logger.LogInformation("Calculated fees - Total: {TotalFee}, Paid: {AmountPaid}, Outstanding: {OutstandingBalance}", 
            totalFee, amountPaid, outstandingBalance);

        try
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add Header Image (Full Width)
                string headerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceImage", "Invoiceheader.png");
                _logger.LogInformation("Looking for header image at: {Path}", headerImagePath);

                if (System.IO.File.Exists(headerImagePath))
                {
                    Image headerImage = Image.GetInstance(headerImagePath);
                    headerImage.ScaleToFit(500, 100);
                    headerImage.Alignment = Element.ALIGN_CENTER;
                    document.Add(headerImage);
                    _logger.LogInformation("Added header image to PDF");
                }
                else
                {
                    _logger.LogWarning("Header image not found at path: {Path}", headerImagePath);
                }

                document.Add(new Paragraph("\n")); // Space after header

                // Invoice Title
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22);
                Paragraph title = new Paragraph("Student Finance Invoice", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(title);
                document.Add(new Paragraph("\n"));

                // Invoice Number
                Font invoiceFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                document.Add(new Paragraph($"Invoice Number: {invoiceNumber}", invoiceFont));
                document.Add(new Paragraph("\n"));

                // Student Information Table
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);
                PdfPTable studentInfoTable = new PdfPTable(2) { WidthPercentage = 100, SpacingBefore = 10 };
                studentInfoTable.SetWidths(new float[] { 30f, 70f });

                studentInfoTable.AddCell(new PdfPCell(new Phrase("Student ID:", headerFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 8 });
                studentInfoTable.AddCell(new PdfPCell(new Phrase(currentUser.StudentId, FontFactory.GetFont(FontFactory.HELVETICA, 14))) { Padding = 8 });

                studentInfoTable.AddCell(new PdfPCell(new Phrase("Last Updated:", headerFont)) { BackgroundColor = new BaseColor(0, 51, 102), Padding = 8 });
                studentInfoTable.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd-MM-yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 14))) { Padding = 8 });

                document.Add(studentInfoTable);
                document.Add(new Paragraph("\n"));

                // Course Table
                PdfPTable table = new PdfPTable(3) { WidthPercentage = 100, SpacingBefore = 10 };
                table.SetWidths(new float[] { 20f, 60f, 20f });

                Font tableHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE);
                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

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

                // Total Fee Section
                Font totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph totalFeeText = new Paragraph($"Total Fees: ${totalFee:F2}", totalFont)
                {
                    Alignment = Element.ALIGN_RIGHT
                };
                document.Add(totalFeeText);

                // Outstanding Balance Section
                Paragraph outstandingBalanceText = new Paragraph($"Balance: ${outstandingBalance:F2}", totalFont)
                {
                    Alignment = Element.ALIGN_RIGHT
                };
                document.Add(outstandingBalanceText);

                document.Add(new Paragraph("\n"));

                // Footer Image
                string footerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceImage", "Invoicefooter.png");
                _logger.LogInformation("Looking for footer image at: {Path}", footerImagePath);

                if (System.IO.File.Exists(footerImagePath))
                {
                    Image footerImage = Image.GetInstance(footerImagePath);
                    footerImage.ScaleToFit(500, 150); // Increased size for better visibility
                    footerImage.Alignment = Element.ALIGN_CENTER;
                    document.Add(footerImage);
                    _logger.LogInformation("Added footer image to PDF");
                }
                else
                {
                    _logger.LogWarning("Footer image not found at path: {Path}", footerImagePath);
                }

                // Add a New Page and Add Financial Detail Image
                document.NewPage();

                string financialDetailImagePath = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceImage", "financialdetail.png");
                _logger.LogInformation("Looking for financial detail image at: {Path}", financialDetailImagePath);

                if (System.IO.File.Exists(financialDetailImagePath))
                {
                    Image financialDetailImage = Image.GetInstance(financialDetailImagePath);
                    financialDetailImage.ScaleToFit(PageSize.A4.Width - 20, PageSize.A4.Height - 20);
                    financialDetailImage.Alignment = Element.ALIGN_CENTER;
                    document.Add(financialDetailImage);
                    _logger.LogInformation("Added financial detail image to PDF");
                }
                else
                {
                    _logger.LogWarning("Financial detail image not found at path: {Path}", financialDetailImagePath);
                }

                document.Close();
                writer.Close();

                _logger.LogInformation("Successfully generated PDF invoice for student {StudentId}", studentId);
                return File(memoryStream.ToArray(), "application/pdf", "Student_Finance_Invoice.pdf");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF invoice for student {StudentId}", studentId);
            return StatusCode(500, "Error generating invoice");
        }
    }
} 