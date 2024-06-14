using Enterprise.Patterns.ResultPattern.Extensions.Generic;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Extensions.Generic;

public partial class ResultExtensionsTests
{
    [Fact]
    public void ToResult_ShouldConvertValueToSuccessResult()
    {
        // Arrange
        string value = "test value";

        // Act
        var result = ResultExtensions.ToResult(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void ToResult_ShouldConvertStructValueToSuccessResult()
    {
        // Arrange
        int value = 123;

        // Act
        var result = ResultExtensions.ToResult(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void ToResult_ShouldConvertNullableStructValueToSuccessResult()
    {
        // Arrange
        int? value = 123;

        // Act
        var result = ResultExtensions.ToResult(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void ToResult_ShouldConvertNullValueToNonSuccessfulResult_WhenProvidedNullValue()
    {
        // Arrange
        int? value = null;

        // Act
        var result = ResultExtensions.ToResult(value);
        Action action = () => { value = result.Value; };

        // Assert
        result.IsSuccess.Should().BeFalse();
        action.Should().Throw<InvalidOperationException>();
    }
}
