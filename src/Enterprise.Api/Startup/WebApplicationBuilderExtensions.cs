using Enterprise.Api.Caching;
using Enterprise.Api.Controllers;
using Enterprise.Api.ErrorHandling;
using Enterprise.Api.Minimal;
using Enterprise.Api.Options;
using Enterprise.Api.Security;
using Enterprise.Api.Serialization;
using Enterprise.Api.Swagger.Config;
using Enterprise.Api.Versioning;
using Enterprise.Applications.DI.Registration.Services;
using Enterprise.AutoMapper;
using Enterprise.Cors.Config;
using Enterprise.Hosting.AspNetCore.Configuration;
using Enterprise.Logging.Config;
using Enterprise.MediatR.Config;
using Enterprise.Monitoring.Health.Config;
using Enterprise.Quartz.Config;
using Enterprise.Traceability.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.Startup;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Configure and register services with the DI container.
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // First thing we need to do is register configuration / options.
        builder.Services.ConfigureOptions(builder);

        builder.ConfigureLogging();
        builder.ConfigureOpenTelemetry();

        builder.Services.ConfigureCors(builder.Environment, builder.Configuration);
        builder.Services.ConfigureIISIntegration(builder.Configuration);

        builder.Services.ConfigureErrorHandling(builder.Environment, builder.Configuration);
        builder.Services.ConfigureSecurity(builder);

        builder.Services.ConfigureSerialization();

        builder.Services.RegisterMinimalApiEndpointSelectorPolicy(builder.Configuration);

        builder.Services.AddRouting(routingOptions =>
        {
            routingOptions.AppendTrailingSlash = false;
            routingOptions.LowercaseQueryStrings = true;
            routingOptions.LowercaseUrls = true;
        });

        builder.Services.ConfigureControllers(builder.Configuration);

        builder.Services.ConfigureCaching(builder.Environment, builder.Configuration);
        builder.Services.ConfigureResponseCaching();
        builder.Services.ConfigureOutputCaching();

        // Customize problem detail results.
        //builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();

        builder.Services.ConfigureSwagger(builder.Configuration);

        // Determines the content type of files.
        // This is a built-in ASP.NET Core content type provider.
        builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

        // Required to get HttpContext reference in object instances.
        // Inject IHttpContextAccessor into constructor of class.
        builder.Services.AddHttpContextAccessor();

        // Monitoring - health check services.
        builder.ConfigureHealthChecks(builder.Configuration);

        // https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0#afwma
        // Add anti forgery token support for minimal APIs that process form data.
        //builder.Services.AddAntiforgery();

        // Allow for the rendering of server side razor components.
        // Makes rendered components available to be returned in API responses.
        //builder.Services.AddRazorComponents();

        // Registers internal services needed to manage request timeouts.
        builder.Services.AddRequestTimeouts();

        // This lets us inject clients and use them more safely without newing them up each time.
        // NOTE: You can add named clients and instance specific configuration in this constructor parameter (duplicate as needed).
        builder.Services.AddHttpClient();

        // Typed clients can be configured and injected into specific abstractions / implementations.
        // https://app.pluralsight.com/course-player?clipId=7c5b839b-c11d-43ee-94fa-0f07892d53a3
        //builder.Services.AddHttpClient<IUserGateway, UserGateway>();

        builder.Services.ConfigureApiVersioning(builder.Configuration);

        // Third party library registrations.
        builder.Services.ConfigureAutoMapper(builder.Configuration);
        // TODO: Does FluentValidation need to go here instead of in ApplicationDependencyRegistrar?
        builder.Services.ConfigureMediatR(builder.Configuration);
        builder.Services.ConfigureQuartz(builder.Configuration);

        // This uses reflection to auto wire up dependencies.
        builder.Services.RegisterServices(builder.Configuration);
    }
}
