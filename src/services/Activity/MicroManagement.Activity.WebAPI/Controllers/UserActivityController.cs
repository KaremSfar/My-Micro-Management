using MicroManagement.Activity.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MicroManagement.Activity.WebAPI.Controllers
{
    [Route("/events")]
    [ApiController]
    [Authorize]
    public class UserActivityController : ControllerBase
    {
        private readonly UserActivityManager _userActivityManager;

        public UserActivityController(UserActivityManager userActivityManager)
        {
            _userActivityManager = userActivityManager;
        }

        [HttpGet]
        public IResult RegisterForEvents(CancellationToken cancellationToken)
        {
            return TypedResults.ServerSentEvents(_userActivityManager.GetEvents(GetUserId(), cancellationToken));
        }

        private Guid GetUserId()
            => Guid.Parse(User.Identities.First().Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
