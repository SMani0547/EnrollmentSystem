using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPFinance.Data;
using USPFinance.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace USPFinance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentHoldController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentHoldController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all-students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.StudentFinances
                .OrderBy(s => s.StudentID)
                .ToListAsync();

            return Ok(students);
        }

        public class PlaceHoldRequest
        {
            public string StudentId { get; set; }
            public string Reason { get; set; }
            public string PlacedBy { get; set; }
        }

        [HttpPost("place-hold")]
        public async Task<IActionResult> PlaceHold([FromBody] PlaceHoldRequest request)
        {
            if (string.IsNullOrEmpty(request.StudentId) || string.IsNullOrEmpty(request.Reason))
            {
                return BadRequest("Student ID and reason are required");
            }

            var studentFinance = await _context.StudentFinances
                .FirstOrDefaultAsync(s => s.StudentID == request.StudentId);

            if (studentFinance == null)
                return NotFound("Student not found");

            studentFinance.IsOnHold = true;
            studentFinance.HoldReason = request.Reason;
            studentFinance.HoldStartDate = DateTime.UtcNow;
            studentFinance.HoldPlacedBy = request.PlacedBy ?? "System";
            studentFinance.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(studentFinance);
        }

        [HttpPost("remove-hold")]
        public async Task<IActionResult> RemoveHold(string studentId)
        {
            var studentFinance = await _context.StudentFinances
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (studentFinance == null)
                return NotFound("Student not found");

            studentFinance.IsOnHold = false;
            studentFinance.HoldEndDate = DateTime.UtcNow;
            studentFinance.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(studentFinance);
        }

        [HttpGet("check-hold/{studentId}")]
        public async Task<IActionResult> CheckHold(string studentId)
        {
            var studentFinance = await _context.StudentFinances
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (studentFinance == null)
                return NotFound("Student not found");

            return Ok(new
            {
                IsOnHold = studentFinance.IsOnHold,
                HoldReason = studentFinance.HoldReason,
                HoldStartDate = studentFinance.HoldStartDate,
                HoldEndDate = studentFinance.HoldEndDate,
                HoldPlacedBy = studentFinance.HoldPlacedBy
            });
        }
    }
} 