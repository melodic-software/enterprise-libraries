using Enterprise.Api.ErrorHandling.Middleware.Constants;
using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.ErrorHandling.Middleware.Registration;

public static class MiddlewareRegistrationExtensions
{
    [Obsolete(ObsoleteConstants.UseIExceptionHandlerWarning)]
    public static void UseCriticalExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<CriticalExceptionMiddleware>();
    }

    [Obsolete(ObsoleteConstants.UseIExceptionHandlerWarning)]
    public static void UseGlobalErrorHandlingMiddleware(this WebApplication app)
    {
        app.UseMiddleware<GlobalErrorHandlingMiddleware>();
    }
}
