using System.Data.Common;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Enterprise.EntityFramework.Interceptors.Demo;

public class QueryHintInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        ApplyQueryHint(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new())
    {
        ApplyQueryHint(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    private void ApplyQueryHint(DbCommand command)
    {
        bool commandTextDoesNotContainEntityName = !command.CommandText
            .ToUpper(CultureInfo.InvariantCulture)
            .Contains("EntityName", StringComparison.OrdinalIgnoreCase);

        if (commandTextDoesNotContainEntityName)
        {
            return;
        }

        // Example: DBA recommends a query hint for any access to the table tied to the given entity.
        command.CommandText += " OPTIONS (PLAN NAME)";
    }
}
