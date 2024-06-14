using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model.Generic;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model.Generic;

public class ResultTests
{
    [Fact]
    public void Constructor_ShouldInitializeValueAndEmptyErrors_WhenCalledWithValue()
    {
        // Act
        var result = new Result<string>("value");

        // Assert
        result.Value.Should().Be("value");
        result.Errors.Should().BeEmpty();
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
        var result = new Result<string>(default, errors);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Code.Should().Be("Code");
    }

    [Fact]
    public void Success_ShouldReturnSuccessfulResult_WhenCalledWithValue()
    {
        // Act
        var result = Result<string>.Success("value");

        // Assert
        result.Value.Should().Be("value");
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
        var result = Result<string>.Failure(error);

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
        var result = Result<string>.Failure(errors);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Value_ShouldThrowInvalidOperationException_WhenCalledOnFailureResult()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var result = Result<string>.Failure(error);

        // Act
        Action act = () => { string value = result.Value; };

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot access the value of a failed result.*");
    }

    [Fact]
    public void Create_ShouldReturnSuccessResult_WhenCalledWithNonNullValue()
    {
        // Act
        var result = Result<string>.Create("value");

        // Assert
        result.Value.Should().Be("value");
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Create_ShouldReturnFailureResult_WhenCalledWithNullValue()
    {
        // Act
        var result = Result<string>.Create(null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Code.Should().Be("NullValue");
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnSuccessResult_WhenConvertedFromValue()
    {
        // Act
        Result<string> result = "value";

        // Assert
        result.Value.Should().Be("value");
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnFailureResult_WhenConvertedFromError()
    {
        // Arrange
        var error = new Error("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation });

        // Act
        Result<string> result = error;

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
        Result<string> result = errors;

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
        Result<string> result = errors;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(2);
    }
}
