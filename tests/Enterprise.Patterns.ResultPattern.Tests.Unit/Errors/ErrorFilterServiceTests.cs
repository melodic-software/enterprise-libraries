using Enterprise.Patterns.ResultPattern.Errors.Filtering;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors;

public class ErrorFilterServiceTests
{
    [Fact]
    public void FilterInvalid_RemovesErrorsWithoutCodeMessageAndDescriptors()
    {
        // Arrange
        List<IError> errors =
        [
            new Error("", null!, new List<ErrorDescriptor>()), // No valid code or message, no descriptors
            new Error(null!, "Valid Message", new List<ErrorDescriptor>()), // Valid message
            new Error("Valid Code", "", new List<ErrorDescriptor>()), // Valid code
            new Error("", "", new List<ErrorDescriptor> { ErrorDescriptor.Validation })
        ];

        // Act
        var filteredErrors = ErrorFilterService.FilterInvalid(errors).ToList();

        // Assert
        filteredErrors.Should().HaveCount(3);
        filteredErrors.Should().NotContain(e => string.IsNullOrWhiteSpace(e.Code) && string.IsNullOrWhiteSpace(e.Message) && !e.Descriptors.Any());
        filteredErrors.Should().ContainSingle(e => e.Message == "Valid Message");
        filteredErrors.Should().ContainSingle(e => e.Code == "Valid Code");
        filteredErrors.Should().ContainSingle(e => e.Descriptors.Contains(ErrorDescriptor.Validation));
    }

    [Fact]
    public void FilterInvalid_KeepsAllValidErrors()
    {
        // Arrange
        List<IError> errors =
        [
            new Error("Code1", "Message1", new List<ErrorDescriptor> { ErrorDescriptor.NotFound }),
            new Error("Code2", "Message2", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule })
        ];

        // Act
        var filteredErrors = ErrorFilterService.FilterInvalid(errors).ToList();

        // Assert
        filteredErrors.Should().HaveCount(2);
        filteredErrors.Should().OnlyContain(e => !string.IsNullOrWhiteSpace(e.Code) && !string.IsNullOrWhiteSpace(e.Message) && e.Descriptors.Any());
    }

    [Fact]
    public void FilterInvalid_HandlesEmptyCollection_ReturnsEmptyCollection()
    {
        // Arrange
        List<IError> errors = [];

        // Act
        IEnumerable<IError> filteredErrors = ErrorFilterService.FilterInvalid(errors);

        // Assert
        filteredErrors.Should().BeEmpty();
    }

    [Fact]
    public void FilterInvalid_RemovesAllInvalidErrors()
    {
        // Arrange
        List<IError> errors =
        [
            new Error(null!, null!, new List<ErrorDescriptor>()), // Completely invalid
            new Error("", "", new List<ErrorDescriptor>()) // Completely invalid
        ];

        // Act
        IEnumerable<IError> filteredErrors = ErrorFilterService.FilterInvalid(errors);

        // Assert
        filteredErrors.Should().BeEmpty();
    }
}
