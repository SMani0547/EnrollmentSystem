using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPFinance.Data;
using USPFinance.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace USPFinance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PageHoldController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PageHoldController> _logger;

        public PageHoldController(AppDbContext context, ILogger<PageHoldController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PageHold>>> GetAllPageHolds()
        {
            return await _context.PageHolds.ToListAsync();
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<PageHold>>> GetActivePageHolds()
        {
            return await _context.PageHolds
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        [HttpPost("toggle/{id}")]
        public async Task<IActionResult> TogglePageHold(int id)
        {
            var pageHold = await _context.PageHolds.FindAsync(id);
            if (pageHold == null)
                return NotFound();

            pageHold.IsActive = !pageHold.IsActive;
            pageHold.LastModifiedAt = DateTime.UtcNow;
            pageHold.LastModifiedBy = User.Identity?.Name ?? "System";

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Page hold toggled for {pageHold.PagePath} to {pageHold.IsActive}");
            return Ok(pageHold);
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdatePageHold(int id, [FromBody] PageHold updatedHold)
        {
            var pageHold = await _context.PageHolds.FindAsync(id);
            if (pageHold == null)
                return NotFound();

            pageHold.Description = updatedHold.Description;
            pageHold.IsActive = updatedHold.IsActive;
            pageHold.LastModifiedAt = DateTime.UtcNow;
            pageHold.LastModifiedBy = User.Identity?.Name ?? "System";

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Page hold updated for {pageHold.PagePath}");
            return Ok(pageHold);
        }

        [HttpGet("check/{pagePath}")]
        public async Task<IActionResult> CheckPageHold(string pagePath)
        {
            _logger.LogInformation($"Checking page hold for path: {pagePath}");

            // Get all active page holds
            var activeHolds = await _context.PageHolds
                .Where(p => p.IsActive)
                .ToListAsync();

            // Normalize the paths for comparison
            var normalizedPagePath = NormalizePath(pagePath);
            _logger.LogInformation($"Normalized path: {normalizedPagePath}");

            // Find the first hold that matches the current path exactly
            var matchingHold = activeHolds.FirstOrDefault(p => 
            {
                var normalizedHoldPath = NormalizePath(p.PagePath);
                var matches = normalizedHoldPath.Equals(normalizedPagePath, StringComparison.OrdinalIgnoreCase);
                _logger.LogInformation($"Comparing {normalizedHoldPath} with {normalizedPagePath}: {matches}");
                return matches;
            });

            if (matchingHold == null)
            {
                _logger.LogInformation("No matching hold found");
                return Ok(new { IsOnHold = false });
            }

            _logger.LogInformation($"Found matching hold: {matchingHold.PageName}");
            return Ok(new
            {
                IsOnHold = true,
                PageName = matchingHold.PageName,
                Description = matchingHold.Description
            });
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Remove leading and trailing slashes
            path = path.Trim('/');

            // Remove query string if present
            var queryIndex = path.IndexOf('?');
            if (queryIndex >= 0)
                path = path.Substring(0, queryIndex);

            // Convert to lowercase for case-insensitive comparison
            return path.ToLowerInvariant();
        }
    }
} 