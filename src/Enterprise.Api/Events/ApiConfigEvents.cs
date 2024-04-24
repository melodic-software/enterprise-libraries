using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Events;

public class ApiConfigEvents
{
    // These are events that external clients can subscribe to, providing hooks into the application's lifecycle.
    // TODO: If possible, it would be nice to separate these and the handlers so the ApiConfigOptions only has access to raise events and not wire up handlers itself.

    public event Func<string[], Task>? PreConfiguring;
    public event Func<WebApplicationBuilder, Task>? BuilderCreated;
    public event Func<WebApplicationBuilder, Task>? ServicesConfigured;
    public event Func<WebApplication, Task>? WebApplicationBuilt;
    public event Func<WebApplication, Task>? RequestPipelineConfigured;
    public event Func<Exception, Task>? ConfigurationErrorOccurred;
    public event Func<Task>? ConfigurationCompleted;

    // These are internal methods to raise lifecycle events, ensuring they are invoked safely within the API.
    // Using these types of event definitions, only this class has the capability of calling them.
    internal Task RaisePreConfigurationCompleted(string[] args) => PreConfiguring?.Invoke(args) ?? Task.CompletedTask;
    internal Task RaiseBuilderCreated(WebApplicationBuilder builder) => BuilderCreated?.Invoke(builder) ?? Task.CompletedTask;
    internal Task RaiseServicesConfigured(WebApplicationBuilder builder) => ServicesConfigured?.Invoke(builder) ?? Task.CompletedTask;
    internal Task RaiseWebApplicationBuilt(WebApplication app) => WebApplicationBuilt?.Invoke(app) ?? Task.CompletedTask;
    internal Task RaiseRequestPipelineConfigured(WebApplication app) => RequestPipelineConfigured?.Invoke(app) ?? Task.CompletedTask;
    internal Task RaiseConfigurationErrorOccurred(Exception ex) => ConfigurationErrorOccurred?.Invoke(ex) ?? Task.CompletedTask;
    internal Task RaiseConfigurationCompleted() => ConfigurationCompleted?.Invoke() ?? Task.CompletedTask;

    /// <summary>
    /// Clears all handlers except for those that may be wired up to the <see cref="ConfigurationErrorOccurred"/> event.
    /// </summary>
    internal void ClearStartupHandlers()
    {
        PreConfiguring = null;
        BuilderCreated = null;
        ServicesConfigured = null;
        WebApplicationBuilt = null;
        RequestPipelineConfigured = null;
        ConfigurationCompleted = null;
    }

    /// <summary>
    /// Clears all event handlers to prevent memory leaks or unintended behavior after app shutdown.
    /// </summary>
    internal void ClearHandlers()
    {
        PreConfiguring = null;
        BuilderCreated = null;
        ServicesConfigured = null;
        WebApplicationBuilt = null;
        RequestPipelineConfigured = null;
        ConfigurationErrorOccurred = null;
        ConfigurationCompleted = null;
    }
}