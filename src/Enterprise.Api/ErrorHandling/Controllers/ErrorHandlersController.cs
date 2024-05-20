﻿using Enterprise.Api.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Api.ErrorHandling.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Route(RouteTemplates.ErrorHandlers)]
[ApiController]
[AllowAnonymous]
[Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
public class ErrorHandlersController : ControllerBase
{
    [Route(RouteTemplates.DevelopmentErrors)]
    public IActionResult HandleDevelopmentError([FromServices] IWebHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsProduction())
        {
            return NotFound();
        }

        // We can customize pre-production error responses.

        IExceptionHandlerFeature? exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (exceptionHandlerFeature == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Problem(
            detail: exceptionHandlerFeature.Error.StackTrace,
            title: exceptionHandlerFeature.Error.Message,
            instance: null,
            statusCode: null,
            type: null
        );
    }

    [Route(RouteTemplates.Errors)]
    public IActionResult HandleError() => Problem(); // return the default "problem"
}
