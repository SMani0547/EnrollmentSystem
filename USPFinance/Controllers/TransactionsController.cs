using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPFinance.Data;
using USPFinance.Models;

namespace USPFinance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Set<Transaction>().ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Set<Transaction>().FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return transaction;
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
        {
            _context.Set<Transaction>().Add(transaction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }

        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }
            _context.Entry(transaction).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Set<Transaction>().FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            _context.Set<Transaction>().Remove(transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Set<Transaction>().Any(e => e.Id == id);
        }
    }
} 