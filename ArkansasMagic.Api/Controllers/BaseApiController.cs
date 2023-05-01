using Microsoft.AspNetCore.Mvc;
using ArkansasMagic.Contracts;
using ArkansasMagic.Domain.Handlers;

namespace ArkansasMagic.Api.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ErrorResponse(CommandResult commandResult)
        {
            var errorResponse = new ErrorResponse { Errors = commandResult.Errors };
            return ErrorResponse(errorResponse, commandResult.FailureReason);
        }

        protected IActionResult ErrorResponse<T>(QueryResult<T> queryResult)
        {
            var errorResponse = new ErrorResponse { Errors = queryResult.Errors };
            return ErrorResponse(errorResponse, queryResult.FailureReason);
        }

        private IActionResult ErrorResponse(ErrorResponse response, FailureReasonType failureReason)
        {
            switch (failureReason)
            {
                case FailureReasonType.NotAuthenticated:
                    return Unauthorized(response);
                case FailureReasonType.MissingRequiredPolicy:
                    return StatusCode(403, response);
                case FailureReasonType.EntityNotFound:
                    return NotFound(response);
                case FailureReasonType.ConcurrencyException:
                    return Conflict(response);
                case FailureReasonType.ValidationErrors:
                default:
                    return UnprocessableEntity(response);
            }
        }
    }
}