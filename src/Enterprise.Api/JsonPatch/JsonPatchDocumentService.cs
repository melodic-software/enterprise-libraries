using Enterprise.Http.AspNetCore.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.JsonPatch;

public static class JsonPatchDocumentService
{
    public static async Task<JsonPatchDocumentResult<T>> GetPatchDocumentAsync<T>(HttpContext httpContext, CancellationToken cancellationToken) where T : class
    {
        if (!httpContext.Request.HasJsonPatchContentType())
            return new(Results.StatusCode(StatusCodes.Status415UnsupportedMediaType), null);

        string json = await new StreamReader(httpContext.Request.Body).ReadToEndAsync(cancellationToken);
        JsonPatchDocument<T>? patchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<T>>(json);

        if (patchDocument == null)
            return new(Results.BadRequest("Invalid JSON Patch document."), null);

        return new(null, patchDocument);
    }
}