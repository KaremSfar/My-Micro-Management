using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroManagement.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeSessionsController : ControllerBase
    {
        private readonly ITimeSessionsService _timeSessionsService;

        public TimeSessionsController(ITimeSessionsService timeSessionsService)
        {
            _timeSessionsService = timeSessionsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_timeSessionsService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Post(TimeSessionDTO timeSession)
        {
            var addedTimeSession = _timeSessionsService
                .AddTimeSession(timeSession);

            return Ok(addedTimeSession);
        }
    }
}
