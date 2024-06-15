using Enterprise.Patterns.ResultPattern.Errors.Deduplication;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors.Deduplication;

public class ErrorDedupeServiceTests
{
    [Fact]
    public void ErrorDedupeService_ShouldRemoveDuplicates_WhenCalledWithDuplicateErrors()
    {
        // Arrange
        List<IError> errors =
        [
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }), // Duplicate
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule, ErrorDescriptor.Validation }), // Different count
            new Error("Code2", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code1", "Different message", new List<ErrorDescriptor> { ErrorDescriptor.Validation })
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
    public void ErrorDedupeService_ShouldReturnEmptyCollection_WhenCalledWithEmptyCollection()
    {
        // Arrange
        List<IError> errors = [];

        // Act
        IEnumerable<IError> dedupedErrors = ErrorDedupeService.DedupeErrors(errors);

        // Assert
        dedupedErrors.Should().BeEmpty();
    }

    [Fact]
    public void ErrorDedupeService_ShouldReturnSingleError_WhenCalledWithSingleError()
    {
        // Arrange
        List<IError> errors = [new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation })];

        // Act
        IEnumerable<IError> dedupedErrors = ErrorDedupeService.DedupeErrors(errors).ToList();

        // Assert
        dedupedErrors.Should().HaveCount(1);
        dedupedErrors.First().Code.Should().Be("Code1");
        dedupedErrors.First().Message.Should().Be("Error message");
    }

    [Fact]
    public void ErrorDedupeService_ShouldDistinguishErrors_WhenCalledWithDifferentDescriptorCounts()
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

    [Fact]
    public void ErrorDedupeService_ShouldReturnOriginalCollection_WhenCalledWithNoDuplicates()
    {
        // Arrange
        List<IError> errors =
        [
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code2", "Another message", new List<ErrorDescriptor> { ErrorDescriptor.NotFound })
        ];

        // Act
        IEnumerable<IError> dedupedErrors = ErrorDedupeService.DedupeErrors(errors).ToList();

        // Assert
        dedupedErrors.Should().HaveCount(2);
        dedupedErrors.Should().Contain(e => e.Code == "Code1" && e.Message == "Error message");
        dedupedErrors.Should().Contain(e => e.Code == "Code2" && e.Message == "Another message");
    }

    [Fact]
    public void ErrorDedupeService_ShouldRetainUniqueErrors_WhenCalledWithMixedDuplicateAndUniqueErrors()
    {
        // Arrange
        List<IError> errors =
        [
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code1", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }), // Duplicate
            new Error("Code2", "Error message", new List<ErrorDescriptor> { ErrorDescriptor.Validation }),
            new Error("Code2", "Another message", new List<ErrorDescriptor> { ErrorDescriptor.NotFound })
        ];

        // Act
        IEnumerable<IError> dedupedErrors = ErrorDedupeService.DedupeErrors(errors).ToList();

        // Assert
        dedupedErrors.Should().HaveCount(3);
        dedupedErrors.Should().ContainSingle(e => e.Code == "Code1" && e.Message == "Error message");
        dedupedErrors.Should().ContainSingle(e => e.Code == "Code2" && e.Message == "Error message");
        dedupedErrors.Should().ContainSingle(e => e.Code == "Code2" && e.Message == "Another message");
    }
}
