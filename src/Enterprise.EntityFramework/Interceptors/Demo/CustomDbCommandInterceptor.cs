using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Interceptors.Demo;

public class CustomDbCommandInterceptor : DbCommandInterceptor
{
    private readonly ILogger<CustomDbCommandInterceptor> _logger;

    public CustomDbCommandInterceptor(ILogger<CustomDbCommandInterceptor> logger)
    {
        _logger = logger;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        // Customizations can be made here.
        _logger.LogInformation("Reader executing.");

        return base.ReaderExecuting(command, eventData, result);
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        // Customizations can be made here.
        _logger.LogInformation("Reader executed.");

        return base.ReaderExecuted(command, eventData, result);
    }
}
