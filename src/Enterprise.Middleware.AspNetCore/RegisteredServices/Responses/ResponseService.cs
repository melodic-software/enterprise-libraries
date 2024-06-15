using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text.Json;
using Enterprise.Middleware.AspNetCore.RegisteredServices.Dtos;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices.Responses;

public static class ResponseService
{
    public static async Task CreateResponse(HttpContext context, List<ServiceDescriptionDto> dtos, JsonSerializerOptions serializerOptions)
    {
        string json = JsonSerializer.Serialize(dtos, serializerOptions);
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(json);
    }
}
