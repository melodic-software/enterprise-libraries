using System.Reflection;
using Enterprise.Api.Controllers.Behavior;
using Enterprise.Api.Controllers.Formatters;
using Enterprise.Api.Controllers.Options;
using Enterprise.Api.Validation.Filters;
using Enterprise.Constants;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enterprise.Api.Controllers;

public static class ControllerConfigService
{
    public static void ConfigureControllers(this IServiceCollection services, IConfiguration configuration)
    {
        ControllerOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<ControllerOptions>(configuration, ControllerOptions.ConfigSectionKey);

        if (!options.EnableControllers)
        {
            return;
        }

        IMvcBuilder builder = AddControllers(services, options);

        builder.ConfigureFormatters(options.FormatterOptions);
        builder.AddMvcOptions(ConfigureMvcOptions(options));

        // These are action filters that are not globally enabled and must be manually added to an API controller or action method.
        // https://code-maze.com/action-filters-aspnetcore/
        builder.Services.AddScoped<ModelStateValidationFilterAttribute>();

        builder.ConfigureApiBehavior();
    }

    private static IMvcBuilder AddControllers(IServiceCollection services, ControllerOptions options)
    {
        // this registers ONLY the controllers in the service collection and not views or pages
        // because they are not required in most web API projects
        IMvcBuilder builder = services.AddControllers();

        // If controllers exist in other projects, these can be registered here.
        foreach (Type controllerAssemblyType in options.ControllerAssemblyTypes)
        {
            Assembly assembly = controllerAssemblyType.Assembly;
            builder.AddApplicationPart(assembly);
        }

        // TODO: Explore this and dynamic "part" deprecation as shown here with modular monolith:
        // https://github.com/devmentors/Inflow/blob/master/src/Shared/Inflow.Shared.Infrastructure/Extensions.cs#L106
        // NOTE: We'd probably adapt the previous method to filter based off of assembly/module state (enabled/disabled).
        //builder.ConfigureApplicationPartManager(manager =>
        //{
            
        //});

        return builder;
    }

    public static void MapControllers(this WebApplication app)
    {
        IOptions<ControllerOptions> options = app.Services.GetRequiredService<IOptions<ControllerOptions>>();
        MapControllers(app, options.Value);
    }

    public static void MapControllers(this WebApplication app, ControllerOptions options)
    {
        if (!options.EnableControllers)
        {
            return;
        }

        // This shortcut mixes request pipeline setup with route management.
        // No routes are specified, and it is assumed attributes will be added to controllers and actions.
        IEndpointConventionBuilder endpointConventionBuilder = ControllerEndpointRouteBuilderExtensions.MapControllers(app);
    }

    private static Action<MvcOptions> ConfigureMvcOptions(ControllerOptions options)
    {
        return o =>
        {
            // Content negotiation: https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting
            o.RespectBrowserAcceptHeader = true;

            // If the client tries to negotiate for the media type the server doesn't support, it will return 406 Not Acceptable.
            o.ReturnHttpNotAcceptable = true;

            // Since non-nullable properties are treated as if they had a [Required(AllowEmptyStrings = true)] attribute,
            // missing Title in the request will also result in a validation error.
            // This can be overridden by setting this to "true". The default value is "false".
            // TODO: Disable this by default, but make it configurable.
            o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = false;

            // Formats the property names used as error keys.
            // NewtonsoftJsonValidationMetadataProvider can be used as an alternative, but be sure to call .AddNewtonsoftJson() on the builder.
            o.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());

            var inputFormatters = o.InputFormatters.ToList();

            // NOTE: The default output formatter is the first one in the list.
            var outputFormatters = o.OutputFormatters.ToList();
            IOutputFormatter? defaultOutputFormatter = outputFormatters.FirstOrDefault();
            Type? defaultOutputFormatterType = defaultOutputFormatter?.GetType();

            // Instead of manually adding these attributes to all controllers / methods, they can be applied globally here via filters.
            // NOTE: Any convention attributes like [ApiConventionType(typeof(DefaultApiConventions))] will be overridden if filters are applied globally.
            o.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
            o.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
            o.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
            o.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status415UnsupportedMediaType));
            o.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status422UnprocessableEntity));
            o.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

            o.Filters.Add(new ProducesAttribute(MediaTypeConstants.Json, MediaTypeConstants.Xml, MediaTypeConstants.ProblemPlusJson, MediaTypeConstants.ProblemPlusXml));

            // Register custom filters.
            if (options.EnableGlobalAuthorizeFilter)
            {
                o.Filters.Add(new AuthorizeFilter());
            }

            // NOTE: You can pass in authorization policy names to the authorize filter and apply a global authorization policy.
            //o.Filters.Add<ExceptionFilter>();

            // Cache profiles can be configured here.
            // Controllers can refer to the key names here by setting the CacheProfileName property of the ResponseCache attribute.
            //o.CacheProfiles.Add("120SecondsCacheProfile", new() { Duration = 120 });
        };
    }
}
