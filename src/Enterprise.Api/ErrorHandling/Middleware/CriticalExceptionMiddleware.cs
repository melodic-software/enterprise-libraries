using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.ErrorHandling.Middleware;

[Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
public class CriticalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<CriticalExceptionMiddleware> _logger;

    public CriticalExceptionMiddleware(ILogger<CriticalExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (SqliteException sqliteException)
        {
            if (sqliteException.SqliteErrorCode == 551) // TODO: Add and reference a constant value.
            {
                _logger.LogCritical(sqliteException, "A fatal database error occurred!");
            }

            throw; // Rethrow and allow a higher middleware handle it (like the global exception handler).
        }
    }
}
