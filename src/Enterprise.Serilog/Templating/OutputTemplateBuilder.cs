namespace Enterprise.Serilog.Templating;

public class OutputTemplateBuilder
{
    private bool _useSimpleTimeFormat;
    private string? _customEnrichmentTemplate;
    private readonly List<string> _parts = [];

    public OutputTemplateBuilder UseSimpleTimeFormat()
    {
        _useSimpleTimeFormat = true;
        return this;
    }

    /// <summary>
    /// Add the full custom enrichment data.
    /// </summary>
    /// <param name="template"></param>
    /// <returns></returns>
    public OutputTemplateBuilder SetCustomEnrichmentTemplate(string template)
    {
        _customEnrichmentTemplate = template;
        return this;
    }

    public string Build()
    {
        // Add timestamp.
        _parts.Add(_useSimpleTimeFormat
            ? "[{Timestamp:HH:mm:ss}]"
            : "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}]");

        // Add log level.
        _parts.Add("{Level:u3}");

        // Add custom enricher data if specified.
        if (!string.IsNullOrWhiteSpace(_customEnrichmentTemplate))
            _parts.Add(_customEnrichmentTemplate.Trim());

        // Add message.
        _parts.Add("{Message:lj}{NewLine}{Exception}");

        return string.Join(" ", _parts);
    }
}