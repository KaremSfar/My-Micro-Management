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

        /// <summary>
        /// Starts a new time session for the current user and specified project.
        /// </summary>
        /// <param name="projectId">The project ID to start a session for.</param>
        /// <returns>The started TimeSessionDTO.</returns>
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] Guid projectId)
        {
            var userId = GetUserId();
            var session = await _timeSessionsService.StartTimeSession(userId, projectId);
            return Ok(session);
        }

        /// <summary>
        /// Stops the current time session for the logged-in user.
        /// </summary>
        /// <returns>No content if successful.</returns>
        [HttpPost("stop")]
        public async Task<IActionResult> Stop()
        {
            var userId = GetUserId();
            await _timeSessionsService.StopTimeSession(userId);
            return NoContent();
        }

        private Guid GetUserId()
            => Guid.Parse(User.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
