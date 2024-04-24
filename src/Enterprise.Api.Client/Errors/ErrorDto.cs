using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Api.Client.Errors;

public class ErrorDto
{
    public string? Code { get; set; }
    public required string Message { get; set; }

    [SetsRequiredMembers]
    public ErrorDto(string message) : this(null, message)
    {
    }

    [SetsRequiredMembers]
    public ErrorDto(string? code, string message)
    {
        Code = code;
        Message = message;
    }

    public ErrorDto()
    {

    }
}