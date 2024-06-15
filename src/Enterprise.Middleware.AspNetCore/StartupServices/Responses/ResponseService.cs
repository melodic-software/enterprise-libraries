using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text.Json;

namespace Enterprise.Middleware.AspNetCore.StartupServices.Responses;

public static class ResponseService
{
    public static async Task CreateResponse(HttpContext context, List<ServiceDescriptionDto> result, JsonSerializerOptions serializerOptions)
    {
        string json = JsonSerializer.Serialize(result, serializerOptions);
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(json);
    }
}
