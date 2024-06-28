using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultTests
{
    [Fact]
    public void Constructor_ShouldInitializeEmptyErrors_WhenCalledWithoutArguments()
    {
        // Act
        var result = new Result();

        // Assert
        result.Errors.Should().BeEmpty();
        result.HasErrors.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Constructor_ShouldFilterInvalidErrors_WhenCalledWithErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("", "", new List<ErrorDescriptor>()), // Invalid
            new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }) // Valid
        };

        // Act
        var result = new Result(errors);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Code.Should().Be("Code");
    }

    [Fact]
    public void Success_ShouldReturnSuccessfulResult_WhenCalled()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Failure_ShouldReturnFailureResult_WhenCalledWithError()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void Failure_ShouldReturnFailureResult_WhenCalledWithMultipleErrors()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Code1", "Message1", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code2", "Message2", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule })
        };

        // Act
        var result = Result.Failure(errors);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void FirstError_ShouldReturnFirstTrueError_WhenCalled()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("Code1", "", new List<ErrorDescriptor>()), // Invalid
            new Error("Code2", "Message2", new List<ErrorDescriptor> { ErrorDescriptor.Validation })
        };

        // Act
        var result = new Result(errors);
        IError firstError = result.FirstError;

        // Assert
        firstError.Code.Should().Be("Code2");
    }

    [Fact]
    public void FirstError_ShouldReturnNoneError_WhenNoTrueErrorsExist()
    {
        // Arrange
        var errors = new List<IError>
        {
            new Error("", "", new List<ErrorDescriptor>()) // Invalid
        };

        // Act
        var result = new Result(errors);
        IError firstError = result.FirstError;

        // Assert
        firstError.Code.Should().Be("NoError");
    }

    [Fact]
    public void ToString_ShouldFormatCorrectly_WhenCalled()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result.Failure(error);

        // Act
        string resultString = result.ToString();

        // Assert
        resultString.Should().Be("IsSuccess: False Error(s): 1");
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnFailureResult_WhenConvertedFromError()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });

        // Act
        Result result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnFailureResult_WhenConvertedFromErrorArray()
    {
        // Arrange
        Error[] errors = new[]
        {
            new Error("Code1", "Message1", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code2", "Message2", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule })
        };

        // Act
        Result result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnFailureResult_WhenConvertedFromErrorList()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error("Code1", "Message1", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code2", "Message2", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule })
        };

        // Act
        Result result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(2);
    }
}
