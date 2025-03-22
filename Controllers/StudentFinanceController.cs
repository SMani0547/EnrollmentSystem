using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using USPEducation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace USPEducation.Controllers
{
    public class StudentFinanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentFinanceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> FinanceMenu()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var studentFinance = await _context.StudentFinances
                .FirstOrDefaultAsync(sf => sf.StudentID == user.Id);

            if (studentFinance == null)
            {
                // Pass a flag to the view to indicate no financial record
                ViewBag.NoFinanceRecord = true;
                return View("~/Views/Student/FinanceMenu.cshtml");
            }

            // Pass the finance details to the view
            ViewBag.NoFinanceRecord = false;
            return View("~/Views/Student/FinanceMenu.cshtml", studentFinance);
        }


    }
}