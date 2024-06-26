﻿using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Middleware.AspNetCore;

public class ListStartupServicesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceCollection _services;
    private readonly ILogger<ListStartupServicesMiddleware> _logger;
    private const string Path = "/debug/all-registered-services";

    public ListStartupServicesMiddleware(RequestDelegate next, IServiceCollection services, ILogger<ListStartupServicesMiddleware> logger)
    {
        _next = next;
        _services = services;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Path == Path)
        {
            // TODO: Add filtering so DI errors, keyed services, service lifetimes, or type names can be queried specifically.

            // TODO: Add query string parameters to filter by namespace.
            // This can be prebuilt like "show all custom" (namespaces other than System.*, Microsoft.*, etc.) or vice versa

            List<ServiceDescriptionDto?> result = _services.Select(x => CreateDto(x, httpContext))
                .Where(x => x != null)
                .OrderBy(x => x?.ServiceTypeFullName)
                .ToList();

            // TODO: Do we want to group by source type namespace?

            JsonSerializerDefaults serializationDefaults = JsonSerializerDefaults.Web;
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(serializationDefaults);
            string json = JsonSerializer.Serialize(result, jsonSerializerOptions);

            httpContext.Response.ContentType = MediaTypeNames.Application.Json;

            await httpContext.Response.WriteAsync(json);
        }
        else
        {
            await _next(httpContext);
        }
    }

    private ServiceDescriptionDto? CreateDto(ServiceDescriptor serviceDescriptor, HttpContext httpContext)
    {
        try
        {
            string? instanceTypeName = null;
            string? serviceKey = null;

            if (serviceDescriptor.IsKeyedService)
            {
                if (serviceDescriptor.KeyedImplementationInstance != null)
                {
                    instanceTypeName = serviceDescriptor.KeyedImplementationInstance.GetType().FullName;
                }
                else if (serviceDescriptor.KeyedImplementationFactory != null)
                {
                    object instance = serviceDescriptor.KeyedImplementationFactory.Invoke(httpContext.RequestServices, serviceDescriptor.ServiceKey);
                    instanceTypeName = instance.GetType().FullName;
                }

                if (serviceDescriptor.ServiceKey?.GetType() == typeof(string))
                {
                    serviceKey = serviceDescriptor.ServiceKey.ToString();
                }
            }
            else
            {
                if (serviceDescriptor.ImplementationInstance != null)
                {
                    instanceTypeName = serviceDescriptor.ImplementationInstance.GetType().FullName;
                }
                else if (serviceDescriptor.ImplementationFactory != null)
                {
                    object instance = serviceDescriptor.ImplementationFactory.Invoke(httpContext.RequestServices);
                    instanceTypeName = instance.GetType().FullName;
                }
            }

            return new ServiceDescriptionDto
            {
                ServiceTypeFullName = serviceDescriptor.ServiceType.FullName,
                ServiceLifetime = serviceDescriptor.Lifetime.ToString(),
                InstanceTypeName = instanceTypeName,
                ServiceKey = serviceKey
            };
        }
        catch (Exception ex)
        {
            // TODO: Add these to an error list and return them in the results.
            _logger.LogError(ex, "Service does not have a registered implementation.");
        }

        return null;
    }
}