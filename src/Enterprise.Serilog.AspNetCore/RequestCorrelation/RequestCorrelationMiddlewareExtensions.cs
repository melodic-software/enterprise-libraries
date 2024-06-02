using Microsoft.AspNetCore.Builder;

namespace Enterprise.Serilog.AspNetCore.RequestCorrelation;

public static class RequestCorrelationMiddlewareExtensions
{
    public static void UseRequestCorrelationMiddleware(this WebApplication app)
    {
        app.UseMiddleware<RequestCorrelationMiddleware>();
    }
}
