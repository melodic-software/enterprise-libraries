namespace Example.Api.ApplicationServices.Queries.Results;

public class QueryResult
{
    public string Value { get; }

    public QueryResult(string value)
    {
        Value = value;
    }
}
