using Enterprise.Api.Caching;
using Enterprise.Api.Controllers;
using Enterprise.Api.ErrorHandling;
using Enterprise.Api.Middleware.IgnoreFavicon;
using Enterprise.Api.Middleware.RootRedirect;
using Enterprise.Api.Minimal;
using Enterprise.Api.Security;
using Enterprise.Api.Swagger;
using Enterprise.Cors.Config;
using Enterprise.Logging.AspNetCore.Middleware;
using Enterprise.Middleware.AspNetCore.RegisteredServices;
using Enterprise.Middleware.AspNetCore.Registration;
using Enterprise.Monitoring.Health.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Api.Startup;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Configure the request pipeline (middleware).
    /// </summary>
    /// <param name="app"></param>
    public static void ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseLogging();

        app.UseErrorHandling();

        // This will forward proxy headers to the current request.
        // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-8.0
        // This will help during application deployment.
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        // Non production related middleware / services.
        if (!app.Environment.IsProduction())
        {
            // Adds an "endpoint" that enables querying services registered with the container (in non production environments).
            app.UseRegisteredServicesEndpointMiddleware();
            app.UseRootRedirectMiddleware();
            app.UseSwagger();
        }

        if (!app.Environment.IsDevelopment())
        {
            // This is a security feature that indicates to clients that HTTPs should be used for all future requests.
            // The "Strict-Transport-Security" header is added.
            // The default HSTS value is 30 days - you may want to change this for production scenarios
            // See RFC 6797.
            app.UseHsts();
        }

        // TODO: Add configurable option for this.
        // We may want to use HTTPs redirection locally.
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        // Enables using static files for the request.
        // If a path is not set, it will use a wwwroot folder in the project by default.
        // This also enables the addition of custom swagger UI stylesheets.
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors();

        app.UseCaching();

        app.UseSecurity();

        app.UseHealthChecks();

        // https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0#afwma
        //app.UseAntiforgery();

        // Can be added via attribute or via "WithRequestTimeout()" for minimal APIs.
        app.UseRequestTimeouts();

        // These are generic pre-built middleware for specific purposes that are not environment specific
        app.UseIgnoreFaviconMiddleware();

        // This is an extensibility hook for custom application specific middleware registrations.
        // TODO: Do we need to provide middleware hooks for specific blocks here? Further up the chain?
        app.UseMiddleware();

        // This will map controllers (if enabled).
        app.MapControllers();

        // This will register minimal API endpoints.
        app.MapEndpoints();

        // This locks down all controllers / endpoints.
        // Used the [AllowAnonymous] attribute on a controller or endpoint that needs to be publicly accessible.
        // This impacts response caching, as GET/HEAD requests cannot be cached if an auth header is present in the request.
        // NOTE: an alternative is to register the "AuthorizeFilter" globally in the controller configuration.
        //endpointConventionBuilder.RequireAuthorization();
    }
}
