namespace Enterprise.Validation.Model;

public record ValidationError
{
    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }

    public string PropertyName { get; init; }
    public string ErrorMessage { get; init; }
}