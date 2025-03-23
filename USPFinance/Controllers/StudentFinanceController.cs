using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPFinance.Data;
using USPFinance.Models;

namespace USPFinance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFinanceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentFinanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentFinance/{studentId}
        [HttpGet("{studentId}")]
        public async Task<ActionResult<StudentFinance>> GetStudentFinance(string studentId)
        {
            var studentFinance = await _context.StudentFinances
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (studentFinance == null)
            {
                return NotFound();
            }

            return studentFinance;
        }

        // GET: api/StudentFinance/{studentId}/details
        [HttpGet("{studentId}/details")]
        public async Task<ActionResult<StudentFinanceDetails>> GetStudentFinanceDetails(string studentId)
        {
            var studentFinance = await _context.StudentFinances
                .FirstOrDefaultAsync(s => s.StudentID == studentId);

            if (studentFinance == null)
            {
                return NotFound();
            }

            // Get related transactions for this student
            var transactions = await _context.Transactions
                .Where(t => t.Description.Contains(studentId) && t.TransactionType == "Income")
                .ToListAsync();

            // Create a detailed response
            var details = new StudentFinanceDetails
            {
                Id = studentFinance.Id,
                StudentID = studentFinance.StudentID,
                TotalFees = studentFinance.TotalFees,
                AmountPaid = studentFinance.AmountPaid,
                OutstandingBalance = studentFinance.TotalFees - studentFinance.AmountPaid,
                LastUpdated = studentFinance.LastUpdated,
                PaymentHistory = transactions.Select(t => new PaymentRecord
                {
                    Date = t.Date,
                    Amount = t.Amount,
                    Description = t.Description,
                    Notes = t.Notes
                }).ToList()
            };

            return details;
        }

        // POST: api/StudentFinance
        [HttpPost]
        public async Task<ActionResult<StudentFinance>> CreateStudentFinance(StudentFinance studentFinance)
        {
            studentFinance.LastUpdated = DateTime.Now;
            _context.StudentFinances.Add(studentFinance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentFinance), new { studentId = studentFinance.StudentID }, studentFinance);
        }

        // PUT: api/StudentFinance/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentFinance(int id, StudentFinance studentFinance)
        {
            if (id != studentFinance.Id)
            {
                return BadRequest();
            }

            studentFinance.LastUpdated = DateTime.Now;
            _context.Entry(studentFinance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentFinanceExists(id))
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

        private bool StudentFinanceExists(int id)
        {
            return _context.StudentFinances.Any(e => e.Id == id);
        }
    }
} 