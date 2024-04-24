using Microsoft.AspNetCore.Http.Metadata;
using System.Net.Mime;

namespace Enterprise.Api.Minimal.Metadata;

internal sealed class ProducesHtmlMetadata : IProducesResponseTypeMetadata
{
    public Type? Type => null;
    public int StatusCode => 200;
    public IEnumerable<string> ContentTypes { get; } = new[] { MediaTypeNames.Text.Html };
}