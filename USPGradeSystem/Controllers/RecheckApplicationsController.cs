using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using USPGradeSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace USPEducation.Controllers
{
    [Route("api/grades/recheck")]
    [ApiController]
    public class RecheckApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecheckApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/grades/recheck
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecheckApplication>>> GetAllRecheckApplications()
        {
            return await _context.RecheckApplications.ToListAsync();
        }

        // GET: api/grades/recheck/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecheckApplication>> GetRecheckApplication(int id)
        {
            var recheckApplication = await _context.RecheckApplications.FindAsync(id);

            if (recheckApplication == null)
            {
                return NotFound();
            }

            return recheckApplication;
        }

        // GET: api/grades/recheck/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<RecheckApplication>>> GetRecheckApplicationsByStudent(string studentId)
        {
            var applications = await _context.RecheckApplications
                .Where(r => r.StudentId == studentId)
                .ToListAsync();

            if (applications == null || !applications.Any())
            {
                return NotFound();
            }

            return applications;
        }

        // GET: api/grades/recheck/status
        [HttpGet("status")]
        public async Task<ActionResult<object>> GetRecheckStatus(
            [FromQuery] string studentId, 
            [FromQuery] string courseCode,
            [FromQuery] int year,
            [FromQuery] int semester)
        {
            var application = await _context.RecheckApplications
                .Where(r => r.StudentId == studentId &&
                       r.CourseCode == courseCode &&
                       r.Year == year &&
                       r.Semester == semester)
                .OrderByDescending(r => r.ApplicationDate)
                .FirstOrDefaultAsync();

            return new
            {
                HasApplied = application != null,
                RequestDate = application?.ApplicationDate,
                Status = application?.Status.ToString()
            };
        }

        // POST: api/grades/recheck
        [HttpPost]
        public async Task<ActionResult<RecheckApplication>> CreateRecheckApplication(RecheckApplication recheckApplication)
        {
            // Set default values
            recheckApplication.ApplicationDate = DateTime.UtcNow;
            recheckApplication.Status = RecheckStatus.Pending;
            
            _context.RecheckApplications.Add(recheckApplication);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecheckApplication), new { id = recheckApplication.Id }, recheckApplication);
        }

        // PUT: api/grades/recheck/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecheckApplication(int id, RecheckApplication recheckApplication)
        {
            if (id != recheckApplication.Id)
            {
                return BadRequest();
            }

            _context.Entry(recheckApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecheckApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/grades/recheck/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateRecheckStatus(int id, [FromBody] RecheckStatusUpdate statusUpdate)
        {
            var recheckApplication = await _context.RecheckApplications.FindAsync(id);
            
            if (recheckApplication == null)
            {
                return NotFound();
            }

            recheckApplication.Status = statusUpdate.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/grades/recheck/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecheckApplication(int id)
        {
            var recheckApplication = await _context.RecheckApplications.FindAsync(id);
            if (recheckApplication == null)
            {
                return NotFound();
            }

            _context.RecheckApplications.Remove(recheckApplication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecheckApplicationExists(int id)
        {
            return _context.RecheckApplications.Any(e => e.Id == id);
        }
    }

    public class RecheckStatusUpdate
    {
        public RecheckStatus Status { get; set; }
    }
} 