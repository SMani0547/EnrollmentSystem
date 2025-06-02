using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;
using USPSystem.Services;

namespace USPSystem.Controllers
{
    [Authorize]
    public class SpecialConsiderationController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SpecialConsiderationController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IWebHostEnvironment hostEnvironment,
            StudentHoldService studentHoldService,
            PageHoldService pageHoldService)
            : base(studentHoldService, pageHoldService, userManager)
        {
            _context = context;
            _emailService = emailService;
            _hostEnvironment = hostEnvironment;
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return NotFound();
            }

            var applications = await _context.SpecialConsiderationApplications
                .Where(a => a.StudentId == user.StudentId)
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();

            return View(applications);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Apply()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return NotFound();
            }

            // Pre-fill student information from the database
            var model = new SpecialConsiderationApplication
            {
                StudentId = user.StudentId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Telephone = user.PhoneNumber ?? "",
                Campus = "",
                Address = "",
                SemesterYear = $"Semester {(DateTime.Now.Month <= 6 ? 1 : 2)}, {DateTime.Now.Year}",
                Reason = ""
            };

            // Get student's courses
            ViewBag.StudentCourses = await _context.StudentEnrollments
                .Where(e => e.StudentId == user.Id)
                .Include(e => e.Course)
                .Select(e => e.Course)
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Apply(SpecialConsiderationApplication model, IFormFile supportingDocument)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Handle file upload if there's a supporting document
                if (supportingDocument != null && supportingDocument.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "special_consideration");
                    
                    // Create directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate unique filename
                    var uniqueFileName = $"{user.StudentId}_{DateTime.Now:yyyyMMddHHmmss}_{supportingDocument.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await supportingDocument.CopyToAsync(stream);
                    }

                    // Store file path in the model
                    model.SupportingDocuments = uniqueFileName;
                }

                // Set application date and status
                model.ApplicationDate = DateTime.Now;
                model.ApplicationStatus = "Pending";

                // Add application to database
                _context.SpecialConsiderationApplications.Add(model);
                await _context.SaveChangesAsync();

                // Send confirmation email
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Special Consideration Application Received",
                    $"Dear {user.FirstName},<br><br>" +
                    $"Your application for Special Consideration has been received and is being processed. " +
                    $"Your application reference number is: SC-{model.Id}.<br><br>" +
                    $"You will be notified of the outcome via email.<br><br>" +
                    $"Regards,<br>USP Student Services"
                );

                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            // Get student's courses again for the dropdown
            ViewBag.StudentCourses = await _context.StudentEnrollments
                .Where(e => e.StudentId == user.Id)
                .Include(e => e.Course)
                .Select(e => e.Course)
                .ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return NotFound();
            }

            var application = await _context.SpecialConsiderationApplications
                .FirstOrDefaultAsync(a => a.Id == id && a.StudentId == user.StudentId);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Manage()
        {
            var applications = await _context.SpecialConsiderationApplications
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();

            return View(applications);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ReviewApplication(int id)
        {
            var application = await _context.SpecialConsiderationApplications
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateStatus(int id, string status, string comments)
        {
            var application = await _context.SpecialConsiderationApplications
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            // Update application status
            application.ApplicationStatus = status;

            // Save changes
            await _context.SaveChangesAsync();

            // Find the student by StudentId
            var student = await _userManager.Users
                .FirstOrDefaultAsync(u => u.StudentId == application.StudentId);
                
            if (student != null)
            {
                string emailSubject = "Update on Your Special Consideration Application";
                string emailBody = $"Dear {student.FirstName},<br><br>" +
                    $"The status of your Special Consideration Application (Reference: SC-{application.Id}) " +
                    $"has been updated to: <strong>{status}</strong>.<br><br>";

                if (!string.IsNullOrEmpty(comments))
                {
                    emailBody += $"Additional comments: {comments}<br><br>";
                }

                emailBody += "If you have any questions, please contact the Examinations Office.<br><br>" +
                    "Regards,<br>USP Student Services";

                await _emailService.SendEmailAsync(application.Email, emailSubject, emailBody);
            }

            return RedirectToAction(nameof(Manage));
        }
    }
} 