using Microsoft.Extensions.Hosting;
using Serilog;

namespace Enterprise.Serilog.AspNetCore.Lifecycle;

/// <summary>
/// Manages the lifecycle for ASP.NET Core applications, ensuring Serilog is properly flushed on shutdown.
/// </summary>
public static class AspNetCoreLifecycleManager
{
    /// <summary>
    /// Initializes lifecycle management for ASP.NET Core applications.
    /// This method registers an action to flush Serilog logs when the application is stopping.
    /// </summary>
    /// <param name="appLifetime">The application lifetime management service.</param>
    public static void Initialize(IHostApplicationLifetime appLifetime)
    {
        Log.Information("Registering Serilog CloseAndFlush for ApplicationStopping event.");
        appLifetime.ApplicationStopping.Register(Log.CloseAndFlush);
    }
}