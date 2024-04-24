using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace Enterprise.Api.JsonPatch;

public class JsonPatchDocumentResult<T> where T : class
{
    public IResult? ErrorResult { get; }
    public JsonPatchDocument<T>? PatchDocument { get; }

    public JsonPatchDocumentResult(IResult? errorResult, JsonPatchDocument<T>? patchDocument)
    {
        ErrorResult = errorResult;
        PatchDocument = patchDocument;
    }
}