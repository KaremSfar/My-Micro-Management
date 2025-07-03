using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroManagement.Activity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSessionsActivityController : ControllerBase
    {
        [HttpPost]
        public IActionResult StartTimeSession([FromBody] Guid projectId)
        {
            // Here you would typically call a service to handle the creation of the time session
            // For now, we will just return a success response
            return Ok(new { Message = "Time session created successfully" });
        }
    }
}
