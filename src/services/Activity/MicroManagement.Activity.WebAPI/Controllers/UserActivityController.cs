using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace MicroManagement.Activity.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserActivityController : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> GetThings(CancellationToken cancellationToken)
        {
            async IAsyncEnumerable<string> GetHeartRate(CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var heartRate = Random.Shared.Next(60, 100);
                    yield return heartRate.ToString();
                    await Task.Delay(2000, cancellationToken);
                }
            }

            return TypedResults.ServerSentEvents(GetHeartRate(cancellationToken));
        }
    }
}
