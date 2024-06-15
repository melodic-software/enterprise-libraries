using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.ErrorHandling.Domain;

public static class ErrorStatusCodeService
{
    public static int GetStatusCode(IReadOnlyCollection<IError> errors, HttpContext context)
    {
        if (errors.ContainsPermissionErrors())
        {
            return StatusCodes.Status403Forbidden;
        }

        if (errors.ContainsNotFound())
        {
            string requestMethod = context.Request.Method;

            // According to REST, DELETE is an idempotent operation.
            // We don't return a 404 in these cases. We always return a 204.
            int statusCode = HttpMethods.IsDelete(requestMethod)
                ? StatusCodes.Status204NoContent
                : StatusCodes.Status404NotFound;

            return statusCode;
        }

        if (errors.ContainsConflict())
        {
            return StatusCodes.Status409Conflict;
        }

        return StatusCodes.Status422UnprocessableEntity;
    }
}
