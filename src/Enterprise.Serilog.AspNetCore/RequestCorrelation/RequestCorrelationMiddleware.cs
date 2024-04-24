using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using static Enterprise.Serilog.AspNetCore.RequestCorrelation.RequestCorrelationConstants;

namespace Enterprise.Serilog.AspNetCore.RequestCorrelation;

internal sealed class RequestCorrelationMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty(SerilogPropertyName, GetCorrelationId(context)))
        {
            return next.Invoke(context);
        }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        // This can take in a correlation ID provided by another external service talking with this API.
        // This allows for tracing a single request across multiple services in a microservice environment.

        context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out StringValues correlationId);
        string result = correlationId.FirstOrDefault() ?? context.TraceIdentifier;
        return result;
    }
}