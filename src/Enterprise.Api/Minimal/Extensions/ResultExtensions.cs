using Enterprise.Api.Minimal.CustomResults;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.Minimal.Extensions;

public static class ResultExtensions
{
    public static IResult Html(this IResultExtensions extensions, string html)
    {
        ArgumentNullException.ThrowIfNull(extensions);

        return new HtmlResult(html);
    }
}