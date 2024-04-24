namespace Enterprise.Monitoring.Health.Model;

public class UriGroup
{
    public string Uri { get; }
    public HttpMethod HttpMethod { get; }
    public string? Name { get; }

    /// <summary>
    /// Create a new URI group.
    /// </summary>
    /// <param name="uri">The URI to use in the health check.</param>
    /// <param name="httpMethod">Defaults to GET.</param>
    /// <param name="name">This will be used as the health check name.</param>
    public UriGroup(string uri, HttpMethod? httpMethod = null, string? name = null)
    {
        Uri = uri;
        HttpMethod = httpMethod ?? HttpMethod.Get;
        Name = name;
    }
}