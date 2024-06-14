using System.Collections.Generic;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using FluentAssertions;
using Xunit;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors.Model;

public class ErrorTests
{
    [Fact]
    public void Error_Constructor_ShouldInitializePropertiesCorrectly_WhenCalledWithValidArguments()
    {
        // Arrange
        var descriptors = new List<ErrorDescriptor> { ErrorDescriptor.Validation };
        var metadata = new Dictionary<string, object> { { "Detail", "Sample detail" } };

        // Act
        var error = new Error("Error.Code", "Error message", descriptors, metadata);

        // Assert
        error.Code.Should().Be("Error.Code");
        error.Message.Should().Be("Error message");
        error.Descriptors.Should().ContainSingle().Which.Should().Be(ErrorDescriptor.Validation);
        error.Metadata.Should().ContainKey("Detail").WhoseValue.Should().Be("Sample detail");
    }

    [Fact]
    public void Error_Constructor_ShouldHandleNullInputs_WhenCalledWithNullArguments()
    {
        // Act
        var error = new Error(null, null, null);

        // Assert
        error.Code.Should().Be("Unknown");
        error.Message.Should().BeEmpty();
        error.Descriptors.Should().BeEmpty();
    }

    [Fact]
    public void Error_Validation_ShouldCreateValidationError_WhenCalledWithMessage()
    {
        // Act
        ValidationError validationError = Error.Validation("Validation failed");

        // Assert
        validationError.Code.Should().Be(ValidationError.GenericCode);
        validationError.Message.Should().Be("Validation failed");
    }

    [Fact]
    public void Error_NotFound_ShouldCreateNotFoundError_WhenCalledWithCodeAndMessage()
    {
        // Act
        NotFoundError notFoundError = Error.NotFound("NotFound.Code", "Resource not found");

        // Assert
        notFoundError.Code.Should().Be("NotFound.Code");
        notFoundError.Message.Should().Be("Resource not found");
    }

    [Fact]
    public void Error_Custom_ShouldCreateCustomError_WhenCalledWithDescriptorsAndMetadata()
    {
        // Act
        var customError = Error.Custom("Custom.Code", "Custom message", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule });

        // Assert
        customError.Code.Should().Be("Custom.Code");
        customError.Message.Should().Be("Custom message");
        customError.Descriptors.Should().ContainSingle().Which.Should().Be(ErrorDescriptor.BusinessRule);
    }

    [Fact]
    public void Error_ToResult_ShouldReturnFailureWithThisError_WhenCalled()
    {
        // Arrange
        var error = new Error("Error.Code", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule });

        // Act
        Result result = error.ToResult;

        // Assert
        result.Should().BeOfType<Result>().Which.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void Error_ToString_ShouldFormatCorrectly_WhenCalled()
    {
        // Arrange
        var error = new Error("Error.Code", "Error occurred", new List<ErrorDescriptor>());

        // Act
        string resultString = error.ToString();

        // Assert
        resultString.Should().Be("Error.Code - Error occurred");
    }

    [Fact]
    public void Error_None_ShouldReturnNoErrorInstance_WhenCalled()
    {
        // Act
        NoError error = Error.None();

        // Assert
        error.Should().BeOfType<NoError>();
        error.Code.Should().Be("NoError");
    }

    [Fact]
    public void Error_Custom_ShouldHandleEmptyDescriptorsAndMetadata_WhenCalledWithEmptyArguments()
    {
        // Act
        var error = Error.Custom("Custom.Code", "Message", new List<ErrorDescriptor>(), new Dictionary<string, object>());

        // Assert
        error.Descriptors.Should().BeEmpty();
        error.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Error_Metadata_ShouldBeReadOnly_WhenCreated()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "key", "initial" } };
        var error = Error.Custom("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }, metadata);

        // Act
        string newKey = "newKey";
        metadata.Add(newKey, "newValue");

        // Assert
        error.Metadata.Should().NotContainKey(newKey);
    }

    [Fact]
    public void Error_Descriptors_ShouldBeImmutable_WhenCreated()
    {
        // Arrange
        var descriptors = new List<ErrorDescriptor> { ErrorDescriptor.Validation };
        var error = new Error("Code", "Message", descriptors);

        // Act
        descriptors.Add(ErrorDescriptor.NotFound);

        // Assert
        error.Descriptors.Should().HaveCount(1);
        error.Descriptors.Should().NotContain(ErrorDescriptor.NotFound);
    }

    [Fact]
    public void Error_NullValue_ShouldCreateNullValueError_WhenCalledWithoutArguments()
    {
        // Act
        NullValueError nullValueError = Error.NullValue();

        // Assert
        nullValueError.Should().BeOfType<NullValueError>();
    }

    [Fact]
    public void Error_NullValue_ShouldCreateNullValueError_WhenCalledWithCodeAndMessage()
    {
        // Act
        NullValueError nullValueError = Error.NullValue("Null.Code", "Null message");

        // Assert
        nullValueError.Code.Should().Be("Null.Code");
        nullValueError.Message.Should().Be("Null message");
    }

    [Fact]
    public void Error_BusinessRuleViolation_ShouldCreateBusinessRuleViolationError_WhenCalledWithCodeAndMessage()
    {
        // Act
        BusinessRuleViolation businessRuleViolation = Error.BusinessRuleViolation("Business.Code", "Business rule violated");

        // Assert
        businessRuleViolation.Code.Should().Be("Business.Code");
        businessRuleViolation.Message.Should().Be("Business rule violated");
    }

    [Fact]
    public void Error_Conflict_ShouldCreateConflictError_WhenCalledWithCodeAndMessage()
    {
        // Act
        ConflictError conflictError = Error.Conflict("Conflict.Code", "Conflict occurred");

        // Assert
        conflictError.Code.Should().Be("Conflict.Code");
        conflictError.Message.Should().Be("Conflict occurred");
    }

    [Fact]
    public void Error_Permission_ShouldCreatePermissionError_WhenCalledWithoutArguments()
    {
        // Act
        PermissionError permissionError = Error.Permission();

        // Assert
        permissionError.Should().BeOfType<PermissionError>();
    }

    [Fact]
    public void Error_Permission_ShouldCreatePermissionError_WhenCalledWithCodeAndMessage()
    {
        // Act
        PermissionError permissionError = Error.Permission("Permission.Code", "Permission denied");

        // Assert
        permissionError.Code.Should().Be("Permission.Code");
        permissionError.Message.Should().Be("Permission denied");
    }
}
