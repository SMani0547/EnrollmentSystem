using Microsoft.AspNetCore.Mvc;
using USPFinance.Services;
using System.Threading.Tasks;

namespace USPFinance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFinanceUpdateController : ControllerBase
    {
        private readonly StudentFinanceUpdateService _updateService;

        public StudentFinanceUpdateController(StudentFinanceUpdateService updateService)
        {
            _updateService = updateService;
        }

        [HttpPost("update/{studentId}")]
        public async Task<IActionResult> UpdateStudentFinance(string studentId)
        {
            try
            {
                await _updateService.UpdateStudentFinanceAsync(studentId);
                return Ok(new { message = "Student finance updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 