namespace Enterprise.Queries.Searching;

public class SearchOptions
{
    /// <summary>
    /// The value to search for.
    /// The backend service is responsible for selecting which fields to use in the search.
    /// Often this is done with full text search.
    /// </summary>
    public string? SearchQuery { get; }

    public SearchOptions(string? searchQuery)
    {
        SearchQuery = searchQuery?.Trim();
    }
}