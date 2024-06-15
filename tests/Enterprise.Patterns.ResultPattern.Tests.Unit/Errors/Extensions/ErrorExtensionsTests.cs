using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors.Extensions;

public class ErrorExtensionsTests
{
    [Fact]
    public void IsTrueError_ShouldReturnTrue_WhenErrorHasTrueDescriptors()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.Validation });

        // Act
        bool result = error.IsTrueError();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsTrueError_ShouldReturnFalse_WhenErrorHasNoTrueDescriptors()
    {
        // Arrange
        IError? error = Substitute.For<IError>();
        error.Descriptors.Returns(new List<ErrorDescriptor> { ErrorDescriptor.NoError });

        // Act
        bool result = error.IsTrueError();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ToResult_ShouldReturnFailureResultWithError()
    {
        // Arrange
        IError? error = Error.NullValue();

        // Act
        var result = error.ToResult<string>();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
