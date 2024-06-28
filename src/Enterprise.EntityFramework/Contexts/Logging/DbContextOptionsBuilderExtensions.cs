using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Contexts.Logging;

public static class DbContextOptionsBuilderExtensions
{
    public static void ConfigureLogging(this DbContextOptionsBuilder optionsBuilder, IHostEnvironment environment)
    {
        // EF Core logging extends .NET logging APIs.

        // Log to the console.
        optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);

        // Log to the debug window.
        optionsBuilder.LogTo(log => Debug.WriteLine(log));

        // Enable sensitive data logging.
        if (environment.IsDevelopment())
        {
            // Don't do this unless data has been cleansed OR if there is no sensitive data that can be accessed by the context.
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
