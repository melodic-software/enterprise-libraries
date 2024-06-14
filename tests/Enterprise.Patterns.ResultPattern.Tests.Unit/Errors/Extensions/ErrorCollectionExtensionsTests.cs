using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using FluentAssertions;
using NSubstitute;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors.Extensions;

public class ErrorCollectionExtensionsTests
{
    [Fact]
    public void ContainsNotFound_ShouldReturnTrue_WhenNotFoundErrorExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.NotFound });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsNotFound();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsNotFound_ShouldReturnFalse_WhenNoNotFoundErrorExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsNotFound();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsValidationErrors_ShouldReturnTrue_WhenValidationErrorExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsValidationErrors();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsValidationErrors_ShouldReturnFalse_WhenNoValidationErrorExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.NotFound });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsValidationErrors();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsBusinessRuleViolations_ShouldReturnTrue_WhenBusinessRuleViolationExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.BusinessRule });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsBusinessRuleViolations();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsBusinessRuleViolations_ShouldReturnFalse_WhenNoBusinessRuleViolationExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsBusinessRuleViolations();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsConflict_ShouldReturnTrue_WhenConflictErrorExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.Conflict });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsConflict();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsConflict_ShouldReturnFalse_WhenNoConflictErrorExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsConflict();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsPermissionErrors_ShouldReturnTrue_WhenPermissionErrorExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.Permission });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsPermissionErrors();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsPermissionErrors_ShouldReturnFalse_WhenNoPermissionErrorExists()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.Validation });
        var errors = new List<IError> { error };

        // Act
        bool result = errors.ContainsPermissionErrors();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetTrueErrors_ShouldReturnOnlyTrueErrors()
    {
        // Arrange
        IError trueError = Error.NullValue();
        IError falseError = Error.None();
        var errors = new List<IError> { trueError, falseError };

        // Act
        List<IError> result = errors.GetTrueErrors();

        // Assert
        result.Should().ContainSingle()
            .Which.Should().Be(trueError);
    }

    [Fact]
    public void HasTrueError_ShouldReturnTrue_WhenTrueErrorExists()
    {
        // Arrange
        IError trueError = Error.NullValue();
        IError falseError = Error.None();
        var errors = new List<IError> { trueError, falseError };

        // Act
        bool result = errors.HasTrueError();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasTrueError_ShouldReturnFalse_WhenNoTrueErrorExists()
    {
        // Arrange
        IError falseError = Error.None();
        var errors = new List<IError> { falseError };

        // Act
        bool result = errors.HasTrueError();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ToResult_ShouldReturnFailureResultWithErrors()
    {
        // Arrange
        IError error = Error.NullValue();
        var errors = new List<IError> { error };

        // Act
        var result = errors.ToResult();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void ToShouldReturnFailureResultWithErrors()
    {
        // Arrange
        IError? error = Error.NullValue();
        var errors = new List<IError> { error };

        // Act
        var result = errors.ToResult<string>();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
