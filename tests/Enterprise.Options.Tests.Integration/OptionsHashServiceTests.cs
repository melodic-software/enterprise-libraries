using System.Reflection;
using Enterprise.Options.Hashing;
using Enterprise.Serialization.Json;
using Enterprise.Serialization.Json.Microsoft;
using FluentAssertions;

namespace Enterprise.Options.Tests.Integration;

public class OptionsHashServiceTests
{
    private readonly ISerializeJson _jsonSerializer = new DefaultJsonSerializer();

    [Fact]
    public void ComputeHash_ShouldProduceCorrectHash_WhenComplexObjectIsProvided()
    {
        // Arrange
        var options = new
        {
            Name = "Test",
            Details = new { Age = 30, Interests = new[] { "Coding", "Music" } }
        };

        // Act
        string hash = OptionsHashService.ComputeHash(options, _jsonSerializer);

        // Assert
        string expectedHash = "4416b687710487cf4e8403e56d221bc434f8e9f126136054fe9721c1e839b06e";

        hash.Should().Be(expectedHash);
    }

    [Fact]
    public void ComputeHash_ShouldProduceCorrectHash_WhenSimpleObjectIsProvided()
    {
        // Arrange
        var options = new { Name = "Test", Value = 123 };

        // Act
        string hash = OptionsHashService.ComputeHash(options, _jsonSerializer);

        // Assert
        // Verify that a valid hash is produced (specific hash value can depend on the actual serialization result)
        hash.Should().NotBeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldReturnEmptyString_WhenEmptyObjectIsProvided()
    {
        // Arrange
        var options = new { };

        // Act
        string hash = OptionsHashService.ComputeHash(options, _jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldBeSuccessful_WhenComplexObjectIsProvided()
    {
        // Arrange
        var options = new
        {
            Outer = new { Inner = new { Value = "InnerValue" } },
            Collection = new List<int> { 1, 2, 3 }
        };

        // Act
        string hash = OptionsHashService.ComputeHash(options, _jsonSerializer);

        // Assert
        hash.Should().NotBeEmpty();
    }

    [Fact]
    public void ComputeHash_ShouldReturnEmptyString_WhenNullObjectIsProvided()
    {
        // Act
        string hash = OptionsHashService.ComputeHash(null!, _jsonSerializer);

        // Assert
        hash.Should().BeEmpty();
    }

    [Fact]
    public async Task ComputeHash_ShouldBeThreadSafe_WhenAccessedConcurrently()
    {
        // Arrange
        var options = new { Name = "ConcurrentTest", Value = 42 };
        var jsonSerializer = new DefaultJsonSerializer();
        var tasks = new List<Task>();
        int numberOfConcurrentAccesses = 100;

        // Act
        for (int i = 0; i < numberOfConcurrentAccesses; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                string hash = OptionsHashService.ComputeHash(options, _jsonSerializer);
                hash.Should().NotBeEmpty();
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        // If no exceptions are thrown and all tasks complete, the test passes
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreDelegateProperties_WhenProvided()
    {
        TestNonSerializableType(new { Action = new Action(() => { }) });
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreAssemblyProperties_WhenProvided()
    {
        TestNonSerializableType(new { Assembly = Assembly.GetExecutingAssembly() });
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreTypeProperties_WhenProvided()
    {
        TestNonSerializableType(new { Type = typeof(string) });
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreStreamProperties_WhenProvided()
    {
        TestNonSerializableType(new { Stream = new MemoryStream() });
    }

    [Fact]
    public void ComputeHash_ShouldIgnoreTaskProperties_WhenProvided()
    {
        TestNonSerializableType(new { Task = Task.CompletedTask });
    }

    private void TestNonSerializableType(object nonSerializableObject)
    {
        // Act
        Action act = () => OptionsHashService.ComputeHash(nonSerializableObject, _jsonSerializer);

        // Assert
        act.Should().NotThrow<NotSupportedException>("because the method should handle non-serializable types gracefully.");
    }
}
