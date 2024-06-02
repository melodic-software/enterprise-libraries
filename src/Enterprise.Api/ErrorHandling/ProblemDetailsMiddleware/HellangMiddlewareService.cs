using Enterprise.Api.Constants;
using Enterprise.Api.ErrorHandling.Constants;
using Enterprise.Api.ErrorHandling.Options;
using Enterprise.Exceptions;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Validation.Exceptions;
using Enterprise.Validation.Extensions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Api.ErrorHandling.ProblemDetailsMiddleware;

internal static class HellangMiddlewareService
{
    internal static void AddProblemDetails(IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        ErrorHandlingConfigOptions errorHandlingConfigOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<ErrorHandlingConfigOptions>(configuration, ErrorHandlingConfigOptions.ConfigSectionKey);

        // Only show exception details in non production environments.
        bool includeExceptionDetails = !environment.IsProduction();

        // This uses a middleware package that transforms exceptions to consistent problem responses based on RFC7807.
        // It uses a machine-readable format for specifying errors in an HTTP API.
        // https://datatracker.ietf.org/doc/html/rfc7807
        // https://github.com/khellang/Middleware/tree/25eac131b2595fa72e2072c87c24763e42bd8e31
        // https://github.com/khellang/Middleware/issues/149
        // https://andrewlock.net/handling-web-api-exceptions-with-problemdetails-middleware/
        services.AddProblemDetails(options =>
        {
            options.IsProblem = HellangMiddlewareDelegates.IsProblem;
            options.ValidationProblemStatusCode = StatusCodes.Status422UnprocessableEntity;

            Func<HttpContext, string?> getTraceId = options.GetTraceId;
            string traceIdPropertyName = options.TraceIdPropertyName;
            
            options.IncludeExceptionDetails = (httpContext, exception) =>
            {
                if (exception is NotFoundException or ValidationException)
                {
                    return false;
                }

                return includeExceptionDetails;
            };

            options.OnBeforeWriteDetails = (httpContext, problemDetails) =>
            {
                // We want to obfuscate exception details to clients of the API.
                if (problemDetails.Status == StatusCodes.Status500InternalServerError && !includeExceptionDetails)
                {
                    problemDetails.Detail = errorHandlingConfigOptions.InternalServerErrorResponseDetailMessage;
                }
            };

            // These are ignored by this middleware.
            // This is convenient if you want to pass through up to other exception middleware components
            //options.Ignore<T>();

            // NOTE: Not sure if this works with the current setup.
            
            options.Rethrow<SqliteException>();
            options.Rethrow<SqlException>();
            //options.Rethrow<ValidationException>();
            //options.Rethrow<Exception>();
            //options.RethrowAll();

            // These are available for use, but are entirely optional.
            // These can be handled manually in controller / framework code OR exceptions can be raised and caught here.
            options.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
            options.MapToStatusCode<ConcurrencyException>(StatusCodes.Status409Conflict);

            options.Map<ValidationException>(exception =>
            {
                var errorDictionary = exception.ValidationErrors.ToDictionary();

                var problemDetails = new ValidationProblemDetails(errorDictionary)
                {
                    // Validation in general can be separate from business rule violations.
                    // Simple data type and format validation are preconditions to use case execution.

                    Detail = ValidationProblemDetailsConstants.Detail,
                    Errors = errorDictionary,
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = ValidationProblemDetailsConstants.Title,
                    Type = StatusCodeLinkConstants.UnprocessableEntityLink,
                };

                // TODO: Make this conditional?
                //problemDetails.Instance = httpContext.Request.Path;

                return problemDetails;
            });

            // This is an application "fault", which is semantically different from an "error".
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });
    }

    internal static void UseProblemDetails(WebApplication app)
    {
        ProblemDetailsExtensions.UseProblemDetails(app);
    }
}
