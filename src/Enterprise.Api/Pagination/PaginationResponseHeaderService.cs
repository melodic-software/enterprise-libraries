using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Enterprise.Api.Client.Pagination;

namespace Enterprise.Api.Pagination;

public static class PaginationResponseHeaderService
{
    public static void AddToResponseHeader(PagingMetadataDto pagingMetadataDto, ControllerBase controller)
    {
        AddToResponseHeader(pagingMetadataDto, controller.HttpContext);
    }

    public static void AddToResponseHeader(PagingMetadataDto pagingMetadataDto, IHttpContextAccessor httpContextAccessor)
    {
        HttpContext? httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            return;
        }

        AddToResponseHeader(pagingMetadataDto, httpContext.Response);
    }

    public static void AddToResponseHeader(PagingMetadataDto pagingMetadataDto, HttpContext httpContext)
    {
        AddToResponseHeader(pagingMetadataDto, httpContext.Response);
    }

    public static void AddToResponseHeader(PagingMetadataDto pagingMetadataDto, HttpResponse response)
    {
        string responseHeaderName = PaginationConstants.CustomPaginationHeader;

        // TODO: do we need to respect the "Accept" header on the request here or is this always going to be a JSON representation?
        // TODO: do we need to do anything about escaped unicode characters (for example, the "&" sign in the query string is escaped as \u0026)
        
        string paginationJson = JsonSerializer.Serialize(pagingMetadataDto, JsonSerializerOptions);

        response.Headers[responseHeaderName] = paginationJson;
    }

    private static JsonSerializerOptions JsonSerializerOptions => CreateJsonSerializerOptions();

    private static JsonSerializerOptions CreateJsonSerializerOptions() 
    {
        JsonSerializerDefaults serializationDefaults = JsonSerializerDefaults.Web;
        var jsonSerializerOptions = new JsonSerializerOptions(serializationDefaults);
        return jsonSerializerOptions;
    }
}