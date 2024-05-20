namespace Enterprise.Serilog.AspNetCore.RequestCorrelation;

public static class RequestCorrelationConstants
{
    public const string CorrelationIdHeaderName = "X-Correlation-Id";
    public const string SerilogPropertyName = "CorrelationId";
}