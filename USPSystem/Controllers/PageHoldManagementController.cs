using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using USPSystem.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using USPFinance.Models;
using Microsoft.AspNetCore.Identity;
using USPSystem.Models;

namespace USPSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class PageHoldManagementController : BaseController
    {
        private readonly ILogger<PageHoldManagementController> _logger;

        public PageHoldManagementController(
            StudentHoldService studentHoldService,
            PageHoldService pageHoldService,
            UserManager<ApplicationUser> userManager,
            ILogger<PageHoldManagementController> logger)
            : base(studentHoldService, pageHoldService, userManager)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _pageHoldService.GetAllPageHolds();
                return View(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving page holds");
                TempData["ErrorMessage"] = "Failed to retrieve page holds. Please try again.";
                return View(new List<PageHold>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleHold(int id)
        {
            try
            {
                var success = await _pageHoldService.TogglePageHold(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Page hold status updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update page hold status.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling page hold for ID: {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the page hold.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHold(int id, string description)
        {
            try
            {
                var success = await _pageHoldService.UpdatePageHold(id, description);
                if (success)
                {
                    TempData["SuccessMessage"] = "Page hold updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update page hold.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating page hold for ID: {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the page hold.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 