using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using Enterprise.Api.Minimal.Metadata;
using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Minimal.CustomResults;

public class HtmlResult : IResult, IEndpointMetadataProvider
{
    private readonly string _html;

    public HtmlResult(string html)
    {
        _html = html;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }

    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ProducesHtmlMetadata());

        // This is an alternative.
        //builder.Metadata.Add(new ProducesAttribute(MediaTypeNames.Text.Html));
    }
}