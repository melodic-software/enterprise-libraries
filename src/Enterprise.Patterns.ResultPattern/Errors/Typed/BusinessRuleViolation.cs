namespace Enterprise.Patterns.ResultPattern.Errors.Typed;

public class BusinessRuleViolation : Error
{
    public const string GenericCode = "BusinessRuleViolation";
    public const string GenericMessage = "A business rule has been violated.";
    public const ErrorDescriptor Descriptor = ErrorDescriptor.BusinessRule;

    public BusinessRuleViolation(
        string code = GenericCode,
        string message = GenericMessage,
        Dictionary<string, object>? metadata = null)
        : base(code, message, [Descriptor], metadata ?? [])
    {
    }
}