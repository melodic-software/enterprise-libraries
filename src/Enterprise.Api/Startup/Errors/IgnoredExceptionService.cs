using Microsoft.Extensions.Hosting;

namespace Enterprise.Api.Startup.Errors;

internal static class IgnoredExceptionService
{
    public static bool ExceptionShouldNotBeIgnored(Exception ex)
    {
        // When generating EF Core migrations, the host is aborted.
        // This is a non error, and we can safely ignore these.
        // https://github.com/dotnet/efcore/issues/29923
        bool isNotEfCoreMigrationAbort = ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore.Design";

        // Add any other "exceptions" here if needed.

        return isNotEfCoreMigrationAbort;
    }
}
