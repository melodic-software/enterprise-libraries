using Enterprise.Patterns.ResultPattern.Errors;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors;

public class ErrorDedupeServiceTests
{
    [Fact]
    public void DedupeErrors_RemovesDuplicates_BasedOnCodeMessageAndDescriptorCount()
    {
        // Arrange
        List<IError> errors =
        [
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }), // Duplicate
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule, ErrorDescriptor.Validation }), // Different count
            new Error("Code2", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code1", "Different message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }) // Different message
        ];

        // Act
        var dedupedErrors = ErrorDedupeService.DedupeErrors(errors).ToList();

        // Assert
        dedupedErrors.Count.Should().Be(4);
        dedupedErrors.Should().ContainSingle(e => e.Code == "Code1" && e.Message == "Error message" && e.Descriptors.Count() == 1);
        dedupedErrors.Should().ContainSingle(e => e.Code == "Code1" && e.Message == "Error message" && e.Descriptors.Count() == 2);
        dedupedErrors.Should().ContainSingle(e => e.Code == "Code2");
        dedupedErrors.Should().ContainSingle(e => e.Message == "Different message");
    }

    [Fact]
    public void DedupeErrors_HandlesEmptyCollection_ReturnsEmptyCollection()
    {
        // Arrange
        List<IError> errors = [];

        // Act
        IEnumerable<IError> dedupedErrors = ErrorDedupeService.DedupeErrors(errors);

        // Assert
        dedupedErrors.Should().BeEmpty();
    }

    [Fact]
    public void DedupeErrors_HandlesSingleError_ReturnsSingleError()
    {
        // Arrange
        List<IError> errors =
            [new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation })];

        // Act
        IEnumerable<IError> dedupedErrors = ErrorDedupeService.DedupeErrors(errors).ToList();

        // Assert
        dedupedErrors.Should().HaveCount(1);
        dedupedErrors.First().Code.Should().Be("Code1");
        dedupedErrors.First().Message.Should().Be("Error message");
    }

    [Fact]
    public void DedupeErrors_DistinguishesErrors_WithDifferentDescriptorCounts()
    {
        // Arrange
        List<IError> errors =
        [
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation, ErrorDescriptor.NotFound })
        ];

        // Act
        IEnumerable<IError> dedupedErrors = ErrorDedupeService.DedupeErrors(errors).ToList();

        // Assert
        dedupedErrors.Should().HaveCount(2);
        dedupedErrors.First().Descriptors.Should().HaveCount(1);
        dedupedErrors.ElementAt(1).Descriptors.Should().HaveCount(2);
    }
}
