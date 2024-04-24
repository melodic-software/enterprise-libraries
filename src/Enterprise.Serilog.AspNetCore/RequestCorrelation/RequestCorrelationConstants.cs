namespace Enterprise.Serilog.AspNetCore.RequestCorrelation;

public class RequestCorrelationConstants
{
    public const string CorrelationIdHeaderName = "X-Correlation-Id";
    public const string SerilogPropertyName = "CorrelationId";
}