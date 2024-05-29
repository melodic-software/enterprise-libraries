using Enterprise.DateTimes.Model;
using Enterprise.DateTimes.Utc;
using FluentAssertions;
using Moq;
using static Enterprise.DateTimes.Formatting.DateTimeFormatStrings;

namespace Enterprise.DateTimes.Tests.Unit.Model;

public class UniversalDateTimeTests
{
    [Fact]
    public void UniversalDateTime_DefaultConstructor_SetsCurrentUtcTime()
    {
        // Arrange
        DateTimeOffset beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var universalDateTime = new UniversalDateTime();

        // Assert
        DateTimeOffset afterCreation = DateTimeOffset.UtcNow;
        Assert.InRange(universalDateTime.DateTimeOffset, beforeCreation, afterCreation);
    }

    [Fact]
    public void UniversalDateTime_ConstructorWithDateTimeOffset_SetsCorrectTime()
    {
        // Arrange
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        // Act
        var universalDateTime = new UniversalDateTime(utcNow);

        // Assert
        Assert.Equal(utcNow, universalDateTime.DateTimeOffset);
    }

    [Fact]
    public void UniversalDateTime_ConstructorWithNonUtcDateTimeOffset_ThrowsArgumentException()
    {
        // Arrange
        var localDateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        var nonUtcOffset = TimeSpan.FromHours(1); // Or any non-zero offset.
        var nonUtcDateTimeOffset = new DateTimeOffset(localDateTime, nonUtcOffset);

        // Act
        Exception? exception = Record.Exception(() => new UniversalDateTime(nonUtcDateTimeOffset));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
        exception?.Message.Should().Contain("must be in UTC");
    }

    [Fact]
    public void UniversalDateTime_ConstructorWithDateTime_CallsEnsureUtcService()
    {
        // Arrange
        DateTime dateTime = DateTime.Now;
        Mock<IEnsureUtcService> mockService = new();
        mockService.Setup(service => service.EnsureUtc(It.IsAny<DateTime>())).Returns(DateTime.UtcNow);

        // Act
        var universalDateTime = new UniversalDateTime(dateTime, mockService.Object);

        // Assert
        mockService.Verify(service => service.EnsureUtc(dateTime), Times.Once());
    }

    [Fact]
    public void UniversalDateTime_ConstructorWithNullEnsureUtcService_ThrowsArgumentNullException()
    {
        // Arrange
        DateTime dateTime = DateTime.Now;

        // Act
        Exception? exception = Record.Exception(() => new UniversalDateTime(dateTime, null!));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void UniversalDateTime_DateTimeProperty_ReturnsUtcDateTime()
    {
        // Arrange
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        var universalDateTime = new UniversalDateTime(utcNow);

        // Act
        DateTime dateTime = universalDateTime.DateTime;

        // Assert
        Assert.Equal(DateTimeKind.Utc, dateTime.Kind);
        Assert.Equal(utcNow.UtcDateTime, dateTime);
    }

    [Fact]
    public void UniversalDateTime_DateOnlyProperty_ReturnsUtcDateOnly()
    {
        // Arrange
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        var universalDateTime = new UniversalDateTime(utcNow);

        // Act
        DateOnly dateOnly = universalDateTime.DateOnly;

        // Assert
        Assert.Equal(DateOnly.FromDateTime(utcNow.Date), dateOnly);
    }

    [Fact]
    public void UniversalDateTime_ToString_ReturnsIso8601FormattedString()
    {
        // Arrange
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        var universalDateTime = new UniversalDateTime(utcNow);

        // Act
        string dateString = universalDateTime.ToString();

        // Assert
        Assert.Equal(utcNow.ToString(Iso8601), dateString);
    }

    [Fact]
    public void UniversalDateTime_LeapYear_HandledCorrectly()
    {
        // Arrange
        var leapYearDate = new DateTime(2020, 2, 29, 0, 0, 0, DateTimeKind.Utc);

        Mock<IEnsureUtcService> mockEnsureUtcService = new();
        mockEnsureUtcService.Setup(x => x.EnsureUtc(It.IsAny<DateTime>())).Returns(leapYearDate);

        var universalDateTime = new UniversalDateTime(leapYearDate, mockEnsureUtcService.Object);

        // Act & Assert
        Assert.Equal(29, universalDateTime.DateTime.Day);
        Assert.Equal(2, universalDateTime.DateTime.Month);
        Assert.Equal(2020, universalDateTime.DateTime.Year);
    }

    [Fact]
    public void UniversalDateTime_ExtremePastDate_HandledCorrectly()
    {
        // Arrange
        var ancientDate = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc); // The earliest date.

        Mock<IEnsureUtcService> mockEnsureUtcService = new();
        mockEnsureUtcService.Setup(x => x.EnsureUtc(It.IsAny<DateTime>())).Returns(ancientDate);

        // Act
        var ancientUniversalDateTime = new UniversalDateTime(ancientDate, mockEnsureUtcService.Object);

        // Assert
        Assert.Equal(ancientDate, ancientUniversalDateTime.DateTime);
    }

    [Fact]
    public void UniversalDateTime_ExtremeFutureDate_HandledCorrectly()
    {
        // Arrange
        var futureDate = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc); // The latest date.

        Mock<IEnsureUtcService> mockEnsureUtcService = new();
        mockEnsureUtcService.Setup(x => x.EnsureUtc(It.IsAny<DateTime>())).Returns(futureDate);

        // Act
        var futureUniversalDateTime = new UniversalDateTime(futureDate, mockEnsureUtcService.Object);

        // Assert
        Assert.Equal(futureDate, futureUniversalDateTime.DateTime);
    }
}
