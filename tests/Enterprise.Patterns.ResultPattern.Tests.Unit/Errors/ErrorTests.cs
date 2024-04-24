using Enterprise.Patterns.ResultPattern.Errors;
using Enterprise.Patterns.ResultPattern.Errors.Typed;
using Enterprise.Patterns.ResultPattern.Model;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors;

public class ErrorTests
{
    [Fact]
    public void Error_Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        List<ErrorDescriptor> descriptors = [ErrorDescriptor.Validation];
        Dictionary<string, object> metadata = new Dictionary<string, object> { { "Detail", "Sample detail" } };

        // Act
        Error error = new Error("Error.Code", "Error message", descriptors, metadata);

        // Assert
        error.Code.Should().Be("Error.Code");
        error.Message.Should().Be("Error message");
        error.Descriptors.Should().ContainSingle().Which.Should().Be(ErrorDescriptor.Validation);
        error.Metadata.Should().ContainKey("Detail").WhoseValue.Should().Be("Sample detail");
    }

    [Fact]
    public void Error_Constructor_HandlesNullInputs()
    {
        // Act
        Error error = new Error(null!, null!, null!);

        // Assert
        error.Code.Should().Be("Unknown");
        error.Message.Should().BeEmpty();
        error.Descriptors.Should().BeEmpty();
    }

    [Fact]
    public void Error_FactoryMethods_CreateSpecificErrorTypes()
    {
        // Act
        ValidationError validationError = Error.Validation("Validation failed");
        NotFoundError notFoundError = Error.NotFound("NotFound.Code", "Resource not found");
        Error customError = Error.Custom("Custom.Code", "Custom message", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule });

        // Assert
        validationError.Code.Should().Be(ValidationError.GenericCode);
        notFoundError.Message.Should().Be("Resource not found");
        customError.Code.Should().Be("Custom.Code");
        customError.Descriptors.Should().ContainSingle().Which.Should().Be(ErrorDescriptor.BusinessRule);
    }

    [Fact]
    public void Error_ToResult_ReturnsFailureWithThisError()
    {
        // Arrange
        Error error = new Error("Error.Code", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule });

        // Act
        Result result = error.ToResult;

        // Assert
        result.Should().BeOfType<Result>().Which.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void Error_ToString_FormatsCorrectly()
    {
        // Arrange
        Error error = new Error("Error.Code", "Error occurred", new List<ErrorDescriptor>());

        // Act
        string resultString = error.ToString();

        // Assert
        resultString.Should().Be("Error.Code - Error occurred");
    }

    [Fact]
    public void Error_None_ReturnsNoErrorInstance()
    {
        // Act
        NoError error = Error.None();

        // Assert
        error.Should().BeOfType<NoError>();
        error.Code.Should().Be("NoError");
    }

    [Fact]
    public void Error_CustomHandlesEmptyDescriptorsAndMetadata()
    {
        // Act
        Error error = Error.Custom("Custom.Code", "Message", new List<ErrorDescriptor>(), new Dictionary<string, object>());

        // Assert
        error.Descriptors.Should().BeEmpty();
        error.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void Error_MetadataIsReadOnly()
    {
        // Arrange
        Dictionary<string, object> metadata = new Dictionary<string, object> { { "key", "initial" } };
        Error error = Error.Custom("Code", "Message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }, metadata);

        // Act
        string newKey = "newKey";
        metadata.Add(newKey, "newValue");

        // Assert
        error.Metadata.Should().NotContainKey(newKey);
    }

    [Fact]
    public void Error_ImmutabilityOfDescriptors()
    {
        // Arrange
        List<ErrorDescriptor> descriptors = [ErrorDescriptor.Validation];
        Error error = new Error("Code", "Message", descriptors);

        // Act
        descriptors.Add(ErrorDescriptor.NotFound);

        // Assert
        error.Descriptors.Should().HaveCount(1);
        error.Descriptors.Should().NotContain(ErrorDescriptor.NotFound);
    }
}
