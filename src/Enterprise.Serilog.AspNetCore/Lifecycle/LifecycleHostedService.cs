using Microsoft.Extensions.Hosting;
using Serilog;

namespace Enterprise.Serilog.AspNetCore.Lifecycle;

/// <summary>
/// A hosted service that initializes the <see cref="AspNetCoreLifecycleManager"/> when the application starts.
/// </summary>
public class LifecycleHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _appLifetime;

    /// <summary>
    /// Creates a new instance of the <see cref="LifecycleHostedService"/>.
    /// </summary>
    /// <param name="appLifetime">The application lifetime management service.</param>
    public LifecycleHostedService(IHostApplicationLifetime appLifetime)
    {
        _appLifetime = appLifetime;
    }

    /// <summary>
    /// Called when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Log.Information("Initializing ASP.NET Core lifecycle management.");
        AspNetCoreLifecycleManager.Initialize(_appLifetime);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Cleanup tasks if needed. This is where you would put any necessary cleanup logic.
        return Task.CompletedTask;
    }
}