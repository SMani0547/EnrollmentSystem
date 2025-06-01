using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using USPSystem.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using USPFinance.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Linq;
using USPSystem.Models;

namespace USPSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class HoldManagementController : BaseController
    {
        private readonly ILogger<HoldManagementController> _logger;

        public HoldManagementController(
            StudentHoldService studentHoldService,
            PageHoldService pageHoldService,
            UserManager<ApplicationUser> userManager,
            ILogger<HoldManagementController> logger)
            : base(studentHoldService, pageHoldService, userManager)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult HoldMessage()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var students = await _studentHoldService.GetAllStudents();
                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving students for hold management");
                TempData["ErrorMessage"] = "Failed to retrieve student list. Please try again.";
                return View(new List<USPFinance.Models.StudentFinance>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceHold(string studentId, string reason)
        {
            _logger.LogInformation("PlaceHold action called");
            _logger.LogInformation($"StudentId: {studentId}");
            _logger.LogInformation($"Reason: {reason}");

            if (string.IsNullOrEmpty(studentId))
            {
                _logger.LogWarning("Student ID is missing");
                TempData["ErrorMessage"] = "Student ID is required.";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(reason))
            {
                _logger.LogWarning("Reason is missing");
                TempData["ErrorMessage"] = "Hold reason is required.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var result = await _studentHoldService.PlaceHold(studentId, reason);
                if (result)
                {
                    _logger.LogInformation($"Successfully placed hold for student {studentId}");
                    TempData["SuccessMessage"] = "Hold placed successfully.";
                }
                else
                {
                    _logger.LogWarning($"Failed to place hold for student {studentId}");
                    TempData["ErrorMessage"] = "Failed to place hold.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error placing hold for student {studentId}");
                TempData["ErrorMessage"] = "An error occurred while placing the hold.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveHold(string studentId)
        {
            _logger.LogInformation("RemoveHold action called");
            _logger.LogInformation($"StudentId: {studentId}");

            if (string.IsNullOrEmpty(studentId))
            {
                _logger.LogWarning("Student ID is missing");
                TempData["ErrorMessage"] = "Student ID is required.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var result = await _studentHoldService.RemoveHold(studentId);
                if (result)
                {
                    _logger.LogInformation($"Successfully removed hold for student {studentId}");
                    TempData["SuccessMessage"] = "Hold removed successfully.";
                }
                else
                {
                    _logger.LogWarning($"Failed to remove hold for student {studentId}");
                    TempData["ErrorMessage"] = "Failed to remove hold.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing hold for student {studentId}");
                TempData["ErrorMessage"] = "An error occurred while removing the hold.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 