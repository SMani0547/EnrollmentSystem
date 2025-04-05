using Microsoft.AspNetCore.Mvc;

namespace USPSystem.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(new { success = true, message = "Hello from the Backend!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Internal Server Error", details = ex.Message });
            }
        }

    }
}
