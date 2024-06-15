using System.Text.Json;
using Enterprise.Applications.DI.ServiceCollection.Abstract;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Dtos;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Filtering;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Mapping;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices;

public class RegisteredServicesEndpointMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;
    private readonly IServiceDescriptorRegistry _serviceDescriptorRegistry;
    private readonly ILogger<RegisteredServicesEndpointMiddleware> _logger;
    private const string Path = "/debug/registered-services";
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RegisteredServicesEndpointMiddleware(RequestDelegate next,
        IHostEnvironment environment,
        IServiceDescriptorRegistry serviceDescriptorRegistry,
        ILogger<RegisteredServicesEndpointMiddleware> logger)
    {
        _next = next;
        _environment = environment;
        _serviceDescriptorRegistry = serviceDescriptorRegistry;
        _logger = logger;

        JsonSerializerDefaults serializationDefaults = JsonSerializerDefaults.Web;
        _jsonSerializerOptions = new JsonSerializerOptions(serializationDefaults);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == Path && !_environment.IsProduction())
        {
            await CreateResponse(context);
        }
        else
        {
            await _next(context);
        }
    }

    private async Task CreateResponse(HttpContext context)
    {
        IQueryCollection query = context.Request.Query;

        var serviceDescriptors = _serviceDescriptorRegistry.ServiceDescriptors.ToList();
        serviceDescriptors = ServiceDescriptorFilter.Execute(query, serviceDescriptors);

        List<ServiceDescriptionDto> dtos = CreateDtos(context, serviceDescriptors);

        dtos = ServiceDescriptionDtoFilter.Execute(query, dtos);

        await ResponseService.CreateResponse(context, dtos, _jsonSerializerOptions);
    }

    private List<ServiceDescriptionDto> CreateDtos(HttpContext context, List<ServiceDescriptor> serviceDescriptors)
    {
        return serviceDescriptors
            .Select(x => DtoCreationService.CreateDto(x, context, _logger))
            .Where(x => x != null)
            .Select(x => x!)
            .OrderBy(x => x?.ServiceTypeFullName)
            .ThenBy(x => x.ImplementationTypeFullName)
            .ToList();
    }
}
