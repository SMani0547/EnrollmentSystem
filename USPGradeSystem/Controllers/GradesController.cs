using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using USPGradeSystem.Models;
using System.Linq;

namespace USPEducation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GradesController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 GET: api/grades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
        {
            return await _context.Grades.ToListAsync();
        }

        // 🔹 GET: api/grades/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGradesByStudentId(string studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentId == studentId)
                .ToListAsync();

            if (grades == null || !grades.Any())
            {
                return NotFound();
            }

            return grades;
        }

        // 🔹 GET: api/grades/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);

            if (grade == null)
            {
                return NotFound();
            }

            return grade;
        }

        // 🔹 GET: api/grades/recheck-status
        [HttpGet("recheck-status")]
        public async Task<ActionResult<object>> GetRecheckStatus(
            [FromQuery] string studentId, 
            [FromQuery] string courseId)
        {
            var hasApplied = await _context.RecheckApplications
                .AnyAsync(r => r.StudentId == studentId && r.CourseCode == courseId);

            return new { hasApplied = hasApplied };
        }

        // 🔹 POST: api/grades
        [HttpPost]
        public async Task<ActionResult<Grade>> PostGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGrade), new { id = grade.Id }, grade);
        }

        // 🔹 PUT: api/grades/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrade(int id, Grade grade)
        {
            if (id != grade.Id)
            {
                return BadRequest();
            }

            _context.Entry(grade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(id))
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

        // 🔹 DELETE: api/grades/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.Id == id);
        }
    }
}
