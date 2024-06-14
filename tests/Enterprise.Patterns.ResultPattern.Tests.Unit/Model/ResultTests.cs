using Enterprise.Patterns.ResultPattern.Errors.Model;
using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using Enterprise.Patterns.ResultPattern.Model.Generic;
using FluentAssertions;

namespace Enterprise.Patterns.ResultPattern.Tests.Unit.Model;

public class ResultTests
{
    [Fact]
    public void Constructor_FiltersInvalidErrors()
    {
        // Arrange
        List<Error> errors =
        [
            new Error(null, null, null, null),
            new Error("Example.Code", "Example.Message", new List<ErrorDescriptor>()),
            Error.None()
        ];

        // Act
        var result = Result.Failure(errors);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void IsSuccess_True_WhenNoTrueErrors()
    {
        var result = Result.Failure(Error.None());

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void IsFailure_False_WhenNoTrueErrors()
    {
        var result = Result.Failure(Error.None());

        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Errors_Empty_WhenConstructedWithNoTrueErrors()
    {
        var result = Result.Failure(Error.None());

        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void HasErrors_False_WhenConstructedWithNoTrueErrors()
    {
        var result = Result.Failure(Error.None());

        result.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void FirstError_NoError_WhenNoErrorsProvided()
    {
        var result = Result.Success();

        result.FirstError.Should().BeEquivalentTo(Error.None());
    }

    [Fact]
    public void ToString_ContainsIsSuccess_WhenResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        string expected = $"{nameof(Result.IsSuccess)}: True"; // Ensure capitalization matches actual output

        // Act
        string resultString = result.ToString();

        // Assert
        resultString.Should().Be(expected);
    }

    [Fact]
    public void ToString_ContainsErrorCount_WhenResultIsFailure()
    {
        // Arrange
        IError error = Error.Validation("Validation Error");
        var result = Result.Failure(error);
        string expected = $"{nameof(Result.IsSuccess)}: False Error(s): 1"; // Ensure capitalization and format matches actual output

        // Act
        string resultString = result.ToString();

        // Assert
        resultString.Should().Be(expected);
    }

    [Fact]
    public void IsSuccess_True_WhenUsingFromFactoryWithNonNullValue()
    {
        var result = Result<string>.From("Value");

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Value_ReturnsProvidedValue_WhenResultIsSuccess()
    {
        string expected = "Value";
        var result = Result.Success(expected);

        result.Value.Should().Be(expected);
    }

    [Fact]
    public void Value_Throws_WhenResultIsFailure()
    {
        var result = Result.Failure<string>(Error.Validation("Validation Error"));

        Func<string> getValue = () => result.Value;

        getValue.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot access the value of a failed result.*");
    }
}
