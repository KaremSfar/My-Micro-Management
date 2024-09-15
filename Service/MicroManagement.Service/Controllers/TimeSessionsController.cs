using LanguageExt;
using MicroManagement.Services.Abstraction;
using MicroManagement.Services.Abstraction.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        /// <summary>
        /// Endpoint used to return all time sessions of the current logged-in user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var timeSessions = await _timeSessionsService.GetAll(GetUserId());
            return Ok(timeSessions);
        }

        private Guid GetUserId()
            => Guid.Parse(User.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
