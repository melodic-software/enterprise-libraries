using System.Globalization;
using Enterprise.Api.Controllers.Options;
using Enterprise.Api.Swagger.Documents.Filters;
using Enterprise.Api.Swagger.Extensions;
using Enterprise.Api.Swagger.Operations.Filters;
using Enterprise.Api.Swagger.Options;
using Enterprise.Api.Swagger.SchemaFilters;
using Enterprise.Api.Swagger.Services;
using Enterprise.Api.Versioning.Constants;
using Enterprise.Api.Versioning.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.Api.Swagger.Documents.DocumentInclusionService;
using static Enterprise.Api.Swagger.Documents.SwaggerDocumentService;

namespace Enterprise.Api.Swagger;

public class SwaggerGenOptionsConfigurer : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly SwaggerOptions _swaggerOptions;
    private readonly ControllerOptions _controllerOptions;
    private readonly VersioningOptions _versioningOptions;
    private readonly IConfiguration _config;
    private readonly ILogger<SwaggerGenOptionsConfigurer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceCollection _serviceCollection;

    public SwaggerGenOptionsConfigurer(SwaggerOptions swaggerOptions,
        IOptions<ControllerOptions> controllerOptions,
        IOptions<VersioningOptions> versioningOptions,
        IConfiguration config,
        ILogger<SwaggerGenOptionsConfigurer> logger,
        IServiceProvider serviceProvider,
        IServiceCollection serviceCollection)
    {
        _swaggerOptions = swaggerOptions;
        _controllerOptions = controllerOptions.Value;
        _versioningOptions = versioningOptions.Value;
        _config = config;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _serviceCollection = serviceCollection;
    }

    public void Configure(SwaggerGenOptions options)
    {
        try
        {
            if (_swaggerOptions.CustomConfigure != null)
            {
                // This is a full customization of the swagger spec generation.
                _swaggerOptions.CustomConfigure(options);
                return;
            }

            // The document name is used in the path:
            // swagger/{documentName}/swagger.json
            // ex: https://localhost:5000/swagger/v1/swagger.json

            // Separate documents can be created centered around specific resources
            // and controllers can be decorated with the [ApiExplorerSettings] attribute
            // specifying a group name that matches the swagger document name.
            // https://app.pluralsight.com/course-player?clipId=e04fd8bb-f0a8-4048-a55c-dd9eae937613

            // The other more popular approach is to create documents for specific versions and to let the resources all co-exist per version
            // This is what we do by default...

            ConfigureSwaggerDocuments(options, _swaggerOptions, _serviceProvider, _config);

            options.DocInclusionPredicate(CanIncludeDocument);

            ResolveConflictingActions(options);

            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();

            // Every class in the swagger JSON must have a unique schemaId.
            // Swashbuckle tries to just use the class name as a simple schemaId,
            // however if you have two classes in different namespaces with the same name this will not work.
            // This outputs full assembly qualified names for models under the "schemas" area at the bottom of the Swagger UI.
            options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
            options.DescribeAllParametersInCamelCase();

            options.AddSecurity(_swaggerOptions);
            options.AddXmlComments(_swaggerOptions);

            AddDocumentFilters(options);
            AddSchemaFilters(options);
            AddOperationFilters(options, _versioningOptions);

            OrderActions(options);

            // allow for adding application specific configuration
            _swaggerOptions.PostConfigure?.Invoke(options, _serviceCollection);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error loading discovery document for Swagger UI.");
        }
    }

    private void ResolveConflictingActions(SwaggerGenOptions options)
    {
        //options.ResolveConflictingActions(apiDescriptions => ResolveSimple(apiDescriptions.ToList(), _logger));

        options.ResolveConflictingActions(apiDescriptions =>
            ConflictingActionResolver.ResolveConflictingActions(apiDescriptions.ToList(), _logger, _controllerOptions.EnableControllers));
    }

    private static void AddDocumentFilters(SwaggerGenOptions options)
    {
        options.DocumentFilter<DisabledControllerFilter>();
        options.DocumentFilter<EnvironmentFilter>();
        options.DocumentFilter<PathLowercaseDocumentFilter>();
        options.DocumentFilter<AlphabeticSortingFilter>();
        options.DocumentFilter<CustomDocumentFilter>();
    }

    private static void AddSchemaFilters(SwaggerGenOptions options)
    {
        options.SchemaFilter<CamelCaseSchemaFilter>();
    }

    private static void AddOperationFilters(SwaggerGenOptions options, VersioningOptions versionOptions)
    {
        bool mediaTypeVersioningEnabled = versionOptions.EnableMediaTypeVersioning;

        // TODO: Inject this in as configuration, particularly if this can be customized by each API instance.
        // For now, we're just going to rely on the preconfigured constant values.
        List<string> allVersionNames =
        [
            VersioningConstants.VersionQueryStringParameterName,
            VersioningConstants.CustomVersionRequestHeader,
            // Add other version parameter names as needed
        ];
        
        options.OperationFilter<NonApplicableParamFilter>();
        options.OperationFilter<RemoveVersionParamsFilter>(mediaTypeVersioningEnabled, allVersionNames);
        options.OperationFilter<SetDefaultVersionParamValueFilter>(allVersionNames);
        options.OperationFilter<CamelCaseQueryParamFilter>();
    }

    private static void OrderActions(SwaggerGenOptions options)
    {
        // https://terencegolla.com/.net/swashbucklecustom-ordering-of-controllers/
        string[] methodsOrder = ["get", "put", "patch", "post", "delete", "head", "options", "trace"];

        //options.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

        options.OrderActionsBy(apiDesc =>
        {
            string? controller = apiDesc.ActionDescriptor.RouteValues["controller"];
            string? httpMethod = apiDesc.HttpMethod?.ToLower(CultureInfo.InvariantCulture);
            int indexOf = Array.IndexOf(methodsOrder, httpMethod);
            string sortKey = $"{controller}_{indexOf}";
            return sortKey;
        });
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}
