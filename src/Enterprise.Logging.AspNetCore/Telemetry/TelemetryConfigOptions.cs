namespace Enterprise.Logging.AspNetCore.Telemetry;

public class TelemetryConfigOptions
{
    public const string ConfigSectionKey = "Custom:Logging:Telemetry";

    public bool EnableApplicationInsightsTelemetry { get; set; }
}