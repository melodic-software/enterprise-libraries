using Enterprise.Patterns.ResultPattern.Errors.Extensions;
using Enterprise.Patterns.ResultPattern.Errors.Model;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Errors.Extensions;

public class ErrorDescriptorExtensionsTests
{
    [Fact]
    public void ContainTrueError_ShouldReturnTrue_WhenTrueErrorDescriptorsExist()
    {
        // Arrange
        var errorDescriptors = new List<ErrorDescriptor>
        {
            ErrorDescriptor.Validation,
            ErrorDescriptor.BusinessRule
        };

        // Act
        bool result = errorDescriptors.ContainTrueError();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainTrueError_ShouldReturnFalse_WhenOnlyNoErrorDescriptorsExist()
    {
        // Arrange
        var errorDescriptors = new List<ErrorDescriptor>
        {
            ErrorDescriptor.NoError
        };

        // Act
        bool result = errorDescriptors.ContainTrueError();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainTrueError_ShouldReturnFalse_WhenErrorDescriptorsAreEmpty()
    {
        // Arrange
        var errorDescriptors = new List<ErrorDescriptor>();

        // Act
        bool result = errorDescriptors.ContainTrueError();

        // Assert
        result.Should().BeFalse();
    }
}
