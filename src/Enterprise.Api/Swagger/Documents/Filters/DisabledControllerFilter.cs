using Enterprise.Api.Controllers.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Documents.Filters;

public class DisabledControllerFilter : IDocumentFilter
{
    private readonly ILogger<DisabledControllerFilter> _logger;
    private readonly ControllerOptions _controllerOptions;

    public DisabledControllerFilter(IOptions<ControllerOptions> controllerOptions, ILogger<DisabledControllerFilter> logger)
    {
        _logger = logger;
        _controllerOptions = controllerOptions.Value;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (_controllerOptions.EnableControllers)
        {
            return;
        }

        var groupedDescriptions = context.ApiDescriptions.GroupBy(GetPath).ToList();

        if (!groupedDescriptions.Any())
        {
            return;
        }

        _logger.LogInformation(
            "Controllers have not been enabled. " +
            "Any Swagger paths associated uniquely with controllers are being removed."
        );

        foreach (IGrouping<string, ApiDescription> group in groupedDescriptions)
        {
            ProcessGroup(swaggerDoc, group);
        }
    }

    private string GetPath(ApiDescription apiDescription)
    {
        return $"/{apiDescription.RelativePath?.TrimStart('/')}";
    }

    private void ProcessGroup(OpenApiDocument swaggerDoc, IGrouping<string, ApiDescription> group)
    {
        bool hasMinimalApi = group.Any(desc => !(desc.ActionDescriptor is ControllerActionDescriptor));
        bool allAreControllers = group.All(desc => desc.ActionDescriptor is ControllerActionDescriptor);

        if (hasMinimalApi || !allAreControllers)
        {
            return;
        }

        foreach (ApiDescription apiDescription in group)
        {
            if (apiDescription.ActionDescriptor is not ControllerActionDescriptor)
            {
                continue;
            }

            string path = GetPath(apiDescription);
            _logger.LogInformation("Removing path: {Path}.", path);
            swaggerDoc.Paths.Remove(path);
        }
    }
}
