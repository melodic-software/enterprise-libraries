using Enterprise.Api.Controllers.Behavior;
using Enterprise.Api.Controllers.Formatters;
using Enterprise.Api.Controllers.Options;
using Enterprise.Api.Validation.Filters;
using Enterprise.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using Enterprise.Options.Core.Services;

namespace Enterprise.Api.Controllers;

public static class ControllerConfigService
{
    public static void ConfigureControllers(this IServiceCollection services, IConfiguration configuration)
    {
        ControllerConfigOptions controllerConfigOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<ControllerConfigOptions>(configuration, ControllerConfigOptions.ConfigSectionKey);

        if (!controllerConfigOptions.EnableControllers)
        {
            return;
        }

        IMvcBuilder builder = AddControllers(services, controllerConfigOptions);

        builder.ConfigureFormatters(controllerConfigOptions.FormatterConfigOptions);
        builder.AddMvcOptions(ConfigureMvcOptions(controllerConfigOptions));

        // These are action filters that are not globally enabled and must be manually added to an API controller or action method.
        // https://code-maze.com/action-filters-aspnetcore/
        builder.Services.AddScoped<ModelStateValidationFilterAttribute>();

        builder.ConfigureApiBehavior();
    }

    private static IMvcBuilder AddControllers(IServiceCollection services, ControllerConfigOptions controllerConfigOptions)
    {
        // this registers ONLY the controllers in the service collection and not views or pages
        // because they are not required in most web API projects
        IMvcBuilder builder = services.AddControllers();

        // If controllers exist in other projects, these can be registered here.
        foreach (Type controllerAssemblyType in controllerConfigOptions.ControllerAssemblyTypes)
        {
            Assembly assembly = controllerAssemblyType.Assembly;
            builder.AddApplicationPart(assembly);
        }

        return builder;
    }

    public static void MapControllers(this WebApplication app)
    {
        IOptions<ControllerConfigOptions> options = app.Services.GetRequiredService<IOptions<ControllerConfigOptions>>();
        MapControllers(app, options.Value);
    }

    public static void MapControllers(this WebApplication app, ControllerConfigOptions controllerConfigOptions)
    {
        if (!controllerConfigOptions.EnableControllers)
        {
            return;
        }

        // This shortcut mixes request pipeline setup with route management.
        // No routes are specified, and it is assumed attributes will be added to controllers and actions.
        IEndpointConventionBuilder endpointConventionBuilder = ControllerEndpointRouteBuilderExtensions.MapControllers(app);
    }

    private static Action<MvcOptions> ConfigureMvcOptions(ControllerConfigOptions controllerConfigOptions)
    {
        return options =>
        {
            // Content negotiation: https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting
            options.RespectBrowserAcceptHeader = true;

            // If the client tries to negotiate for the media type the server doesn't support, it will return 406 Not Acceptable.
            options.ReturnHttpNotAcceptable = true;

            // Since non-nullable properties are treated as if they had a [Required(AllowEmptyStrings = true)] attribute,
            // missing Title in the request will also result in a validation error.
            // This can be overridden by setting this to "true". The default value is "false".
            // TODO: Disable this by default, but make it configurable.
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = false;

            // Formats the property names used as error keys.
            // NewtonsoftJsonValidationMetadataProvider can be used as an alternative, but be sure to call .AddNewtonsoftJson() on the builder.
            options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());

            var inputFormatters = options.InputFormatters.ToList();

            // NOTE: The default output formatter is the first one in the list.
            var outputFormatters = options.OutputFormatters.ToList();
            IOutputFormatter? defaultOutputFormatter = outputFormatters.FirstOrDefault();
            Type? defaultOutputFormatterType = defaultOutputFormatter?.GetType();

            // Instead of manually adding these attributes to all controllers / methods, they can be applied globally here via filters.
            // NOTE: Any convention attributes like [ApiConventionType(typeof(DefaultApiConventions))] will be overridden if filters are applied globally.
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status415UnsupportedMediaType));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status422UnprocessableEntity));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

            options.Filters.Add(new ProducesAttribute(MediaTypeConstants.Json, MediaTypeConstants.Xml, MediaTypeConstants.ProblemPlusJson, MediaTypeConstants.ProblemPlusXml));

            // Register custom filters.
            if (controllerConfigOptions.EnableGlobalAuthorizeFilter)
            {
                options.Filters.Add(new AuthorizeFilter());
            }

            // NOTE: You can pass in authorization policy names to the authorize filter and apply a global authorization policy.
            //options.Filters.Add<ExceptionFilter>();

            // Cache profiles can be configured here.
            // Controllers can refer to the key names here by setting the CacheProfileName property of the ResponseCache attribute.
            //options.CacheProfiles.Add("120SecondsCacheProfile", new() { Duration = 120 });
        };
    }
}
