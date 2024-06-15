using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors.Extensions;

public class ExceptionExtensionsTests
{
    [Fact]
    public void ToError_ShouldReturnExceptionError_WhenExceptionIsGiven()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        ExceptionError result = exception.ToError();

        // Assert
        result.Should().BeOfType<ExceptionError>();
        result.Exception.Should().Be(exception);
    }

    [Fact]
    public void ToResult_ShouldReturnFailureResultWithExceptionError_WhenExceptionIsGiven()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var result = exception.ToResult<string>();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ExceptionError>()
            .Which.Exception.Should().Be(exception);
    }

    [Fact]
    public void ToResult_ShouldReturnFailureResultWithExceptionError_WhenExceptionErrorIsGiven()
    {
        // Arrange
        var exceptionError = new ExceptionError(new Exception("Test exception"));

        // Act
        var result = exceptionError.ToResult<string>();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(exceptionError);
    }
}
