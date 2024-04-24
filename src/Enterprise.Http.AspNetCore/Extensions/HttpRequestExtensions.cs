using Enterprise.Constants;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Http.AspNetCore.Extensions;

public static class HttpRequestExtensions
{
    public static bool HasJsonPatchContentType(this HttpRequest request)
    {
        return string.Equals(request.ContentType, MediaTypeConstants.JsonPatch, StringComparison.OrdinalIgnoreCase);
    }
}