using System.Net.Mime;

namespace Enterprise.Constants;

public static class MediaTypeConstants
{
    public const string Csv = MediaTypeNames.Text.Csv; // "text/csv"
    public const string Javascript = "application/javascript";
    public const string Json = MediaTypeNames.Application.Json; // "application/json"
    public const string JsonPatch = "application/json-patch+json";
    public const string ProblemPlusJson = "application/problem+json";
    public const string ProblemPlusXml = "application/problem+xml";
    public const string Xml = MediaTypeNames.Application.Xml; // "application/xml"
}