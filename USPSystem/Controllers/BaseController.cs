using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using USPSystem.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using USPSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace USPSystem.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly StudentHoldService _studentHoldService;
        protected readonly PageHoldService _pageHoldService;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<BaseController>? _logger;

        protected BaseController(
            StudentHoldService studentHoldService,
            PageHoldService pageHoldService,
            UserManager<ApplicationUser> userManager,
            ILogger<BaseController>? logger = null)
        {
            _studentHoldService = studentHoldService;
            _pageHoldService = pageHoldService;
            _userManager = userManager;
            _logger = logger;
        }

        protected async Task<bool> CheckHolds()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger?.LogInformation("No user found, skipping hold check");
                return false;
            }

            var currentPath = Request.Path.Value;
            _logger?.LogInformation($"Checking holds for path: {currentPath}");

            // First check if the current page is on hold (applies to all users)
            var (isPageOnHold, pageName, pageDescription) = await _pageHoldService.CheckPageHold(currentPath ?? string.Empty);
            if (isPageOnHold)
            {
                _logger?.LogInformation($"Page is on hold: {pageName}");
                ViewData["HoldReason"] = $"This page ({pageName}) is currently on hold. {pageDescription}";
                ViewData["HoldPlacedBy"] = "System";
                ViewData["HoldStartDate"] = DateTime.Now.ToString("g");
                return true;
            }

            // Then check if the student has a hold
            var holdStatus = await _studentHoldService.CheckHoldStatus(user.StudentId);
            if (holdStatus.IsOnHold)
            {
                _logger?.LogInformation($"Student {user.StudentId} has a hold");
                ViewData["HoldReason"] = holdStatus.HoldReason;
                ViewData["HoldPlacedBy"] = holdStatus.HoldPlacedBy;
                ViewData["HoldStartDate"] = holdStatus.HoldStartDate?.ToString("g");
                return true;
            }

            _logger?.LogInformation("No holds found");
            return false;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Skip hold check for HoldManagement and PageHoldManagement controllers
            if (context.Controller.GetType().Name != "HoldManagementController" && 
                context.Controller.GetType().Name != "PageHoldManagementController")
            {
                _logger?.LogInformation($"Checking holds for controller: {context.Controller.GetType().Name}");
                var hasHold = await CheckHolds();
                if (hasHold)
                {
                    _logger?.LogInformation("Hold found, showing hold message");
                    // Continue with the action execution to show the hold message
                    await next();
                    return;
                }
            }

            await next();
        }
    }
} 