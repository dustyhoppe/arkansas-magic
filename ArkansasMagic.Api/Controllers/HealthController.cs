using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArkansasMagic.Contracts;
using ArkansasMagic.Contracts.Health;

namespace ArkansasMagic.Api.Controllers
{
    /// <summary>
    /// Health Check controller
    /// </summary>
    [Route(ApiRoutes.v1.Health)]
    public class HealthController : BaseApiController
    {
        /// <summary>
        /// Health Check
        /// </summary>
        /// <remarks>
        /// Acts as a service level health check returning 200 in the event the service is healthy.
        /// </remarks>
        [HttpGet("check")]
        [ProducesResponseType(typeof(CheckResponse), StatusCodes.Status200OK)]
        public IActionResult Check()
        {
            return Ok(new CheckResponse { ThumbsUp = true });
        }
    }
}
