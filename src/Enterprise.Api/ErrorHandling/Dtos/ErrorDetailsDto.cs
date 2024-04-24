namespace Enterprise.Api.ErrorHandling.Dtos;

public class ErrorDetailsDto
{
    public ErrorDetailsDto(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public int StatusCode { get; }
    public string Message { get; }
}