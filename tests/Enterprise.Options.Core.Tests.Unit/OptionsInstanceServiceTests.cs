using System.Collections.Concurrent;
using System.Reflection;
using Enterprise.Options.Core.Model;
using Enterprise.Options.Core.Services.Singleton;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Enterprise.Options.Core.Tests.Unit;

public class OptionsInstanceServiceTests
{
    [Fact]
    public void ConfigureDefaultInstance_ShouldConfigureInstance_WhenValidInstanceProvided()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var testOptions = new TestOptions { Property1 = "Test" };

        // Act
        service.ConfigureDefaultInstance(testOptions);

        // Assert
        service.ConfigureDefaultInstance(() => testOptions);
        TestOptions configuredInstance = service.GetOptionsInstance<TestOptions>(Substitute.For<IConfiguration>(), null);
        configuredInstance.Property1.Should().Be("Test");
    }

    [Fact]
    public void ConfigureDefaultInstance_ShouldReplaceExistingInstance_WhenCalledMultipleTimes()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var initialOptions = new TestOptions { Property1 = "Initial" };
        var newOptions = new TestOptions { Property1 = "New" };

        // Act
        service.ConfigureDefaultInstance(initialOptions);
        service.ConfigureDefaultInstance(newOptions);

        // Assert
        TestOptions configuredInstance = service.GetOptionsInstance<TestOptions>(Substitute.For<IConfiguration>(), null);
        configuredInstance.Property1.Should().Be("New");
    }

    [Fact]
    public void ConfigureDefaultInstance_ShouldNotReplaceExistingInstance_WhenCalledOnce()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var initialOptions = new TestOptions { Property1 = "Initial" };

        // Act
        service.ConfigureDefaultInstance(initialOptions);

        // Assert
        TestOptions configuredInstance = service.GetOptionsInstance<TestOptions>(Substitute.For<IConfiguration>(), null);
        configuredInstance.Property1.Should().Be("Initial");
    }

    [Fact]
    public void ConfigureDefaultInstance_ShouldThrowException_WhenNullInstanceProvided()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();

        // Act
        Action act = () => service.ConfigureDefaultInstance((TestOptions)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Configure_ShouldAddInitialAndAdditionalActions_WhenCalledMultipleTimes()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var testOptions = new TestOptions();
        void Action1(TestOptions options) => options.Property1 = "Action1";
        void Action2(TestOptions options) => options.Property2 = 123;

        // Act
        service.Configure<TestOptions>(Action1);
        service.Configure<TestOptions>(Action2);

        // Assert
        TestOptions configuredInstance = service.GetOptionsInstance<TestOptions>(Substitute.For<IConfiguration>(), null);
        configuredInstance.Property1.Should().Be("Action1");
        configuredInstance.Property2.Should().Be(123);
    }

    [Fact]
    public void Configure_ShouldApplyActionsInOrder()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        void Action1(TestOptions options) => options.Property1 = "First";
        void Action2(TestOptions options) => options.Property1 += " Second";

        // Act
        service.Configure<TestOptions>(Action1);
        service.Configure<TestOptions>(Action2);

        // Assert
        TestOptions configuredInstance = service.GetOptionsInstance<TestOptions>(Substitute.For<IConfiguration>(), null);
        configuredInstance.Property1.Should().Be("First Second");
    }

    [Fact]
    public void Configure_ShouldNotAffectOtherOptionTypes()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var testOptions = new TestOptions { Property1 = "TestOptions" };
        var anotherTestOptions = new AnotherTestOptions { Property1 = "AnotherTestOptions" };
        service.ConfigureDefaultInstance(testOptions);
        service.ConfigureDefaultInstance(anotherTestOptions);

        // Act
        service.Configure<TestOptions>(opts => opts.Property1 = "Modified");

        // Assert
        TestOptions configuredTestOptions = service.GetOptionsInstance<TestOptions>(Substitute.For<IConfiguration>(), null);
        AnotherTestOptions configuredAnotherTestOptions = service.GetOptionsInstance<AnotherTestOptions>(Substitute.For<IConfiguration>(), null);

        configuredTestOptions.Property1.Should().Be("Modified");
        configuredAnotherTestOptions.Property1.Should().Be("AnotherTestOptions");
    }

    [Fact]
    public void Configure_ShouldNotLockInstance_WhenConfigureIsCalled()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var testOptions = new TestOptions { Property1 = "Initial" };

        // Act
        service.Configure<TestOptions>(opts => opts.Property1 = "Configured");
        TestOptions optionsInstance = service.GetOptionsInstance<TestOptions>(Substitute.For<IConfiguration>(), null);

        // Assert
        optionsInstance.Property1.Should().Be("Configured");

        PropertyInfo? instanceDictionaryField = typeof(OptionsInstanceService)
            .GetProperty("InstanceDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
        var instanceDictionary = instanceDictionaryField!.GetValue(service) as ConcurrentDictionary<Type, OptionsInstanceDictionaryItem>;

        bool success = instanceDictionary!.TryGetValue(typeof(TestOptions), out OptionsInstanceDictionaryItem? item);

        success.Should().BeTrue();
        item.Should().NotBeNull();
        item!.IsLocked.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldThrowException_WhenNullActionProvided()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();

        // Act
        Action act = () => service.Configure<TestOptions>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Configure_ShouldThrowException_WhenReconfiguringLockedOptions()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var testOptions = new TestOptions { Property1 = "Initial" };
        service.UseInstance(testOptions);

        // Act
        Action act = () => service.Configure<TestOptions>(opts => opts.Property1 = "Modified");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("A locked options instance has already been registered and can no longer be reconfigured.");
    }

    [Fact]
    public void GetOptionsInstance_ShouldAutoCreateInstance_WhenNotConfigured()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        IConfiguration? configuration = Substitute.For<IConfiguration>();

        // Act
        TestOptions optionsInstance = service.GetOptionsInstance<TestOptions>(configuration, null);

        // Assert
        optionsInstance.Should().NotBeNull();
    }

    [Fact]
    public void GetOptionsInstance_ShouldReturnConfiguredInstance_WhenInstanceExists()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        IConfiguration? configuration = Substitute.For<IConfiguration>();
        var testOptions = new TestOptions { Property1 = "Configured" };
        service.ConfigureDefaultInstance(testOptions);

        // Act
        TestOptions optionsInstance = service.GetOptionsInstance<TestOptions>(configuration, null);

        // Assert
        optionsInstance.Should().BeEquivalentTo(testOptions);
    }

    [Fact]
    public void GetOptionsInstance_ShouldReturnNewInstance_WhenTypeNotConfigured()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        IConfiguration configuration = Substitute.For<IConfiguration>();

        // Act
        AnotherTestOptions optionsInstance = service.GetOptionsInstance<AnotherTestOptions>(configuration, null);

        // Assert
        optionsInstance.Should().NotBeNull();
        optionsInstance.Property1.Should().BeNull();
    }

    [Fact]
    public void GetOptionsInstance_ShouldReturnSameInstance_WhenCalledTwice()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        IConfiguration configuration = Substitute.For<IConfiguration>();
        service.ConfigureDefaultInstance(new TestOptions { Property1 = "Initial" });

        // Act
        TestOptions firstCallInstance = service.GetOptionsInstance<TestOptions>(configuration, null);
        TestOptions secondCallInstance = service.GetOptionsInstance<TestOptions>(configuration, null);

        // Assert
        firstCallInstance.Should().BeSameAs(secondCallInstance);
    }

    [Fact]
    public void GetOptionsInstance_ShouldThrowException_WhenConfigurationIsNull()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();

        // Act
        Action act = () => service.GetOptionsInstance<TestOptions>(null!, null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetOptionsInstance_ShouldThrowException_WhenInstanceDictionaryTypeMismatch()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        IConfiguration configuration = Substitute.For<IConfiguration>();

        PropertyInfo? instanceDictionaryField = typeof(OptionsInstanceService)
            .GetProperty("InstanceDictionary", BindingFlags.NonPublic | BindingFlags.Instance);

        var instanceDictionary = instanceDictionaryField!.GetValue(service) as ConcurrentDictionary<Type, OptionsInstanceDictionaryItem>;

        instanceDictionary!.TryAdd(typeof(TestOptions), OptionsInstanceDictionaryItem.New(new AnotherTestOptions { Property1 = "Mismatch" }));

        // Act
        Action act = () => service.GetOptionsInstance<TestOptions>(configuration, null);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("Instance dictionary contains a type mismatch for the given key: *");
    }

    [Fact]
    public void GetOptionsInstance_ShouldUseDefaultFactoryMethod_WhenConfigured()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        IConfiguration? configuration = Substitute.For<IConfiguration>();
        service.ConfigureDefaultInstance(() => new TestOptions { Property1 = "DefaultFactory" });

        // Act
        TestOptions optionsInstance = service.GetOptionsInstance<TestOptions>(configuration, null);

        // Assert
        optionsInstance.Property1.Should().Be("DefaultFactory");
    }

    [Fact]
    public void UseInstance_ShouldLockInstance_WhenCalled()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var testOptions = new TestOptions { Property1 = "LockedInstance" };

        // Act
        service.UseInstance(testOptions);

        // Assert
        PropertyInfo? instanceProperty = typeof(OptionsInstanceService)
            .GetProperty("InstanceDictionary", BindingFlags.NonPublic | BindingFlags.Instance);

        var instanceDictionary = instanceProperty!.GetValue(service) as ConcurrentDictionary<Type, OptionsInstanceDictionaryItem>;

        bool success = instanceDictionary!.TryGetValue(typeof(TestOptions), out OptionsInstanceDictionaryItem? item);

        success.Should().BeTrue();
        item.Should().NotBeNull();
        item!.Options.Should().BeEquivalentTo(testOptions);
        item.IsLocked.Should().BeTrue();
    }

    [Fact]
    public void UseInstance_ShouldPreventFurtherConfiguration_WhenInstanceUsed()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var testOptions = new TestOptions { Property1 = "Initial" };
        service.UseInstance(testOptions);

        // Act
        Action act = () => service.Configure<TestOptions>(opts => opts.Property1 = "Configured");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("A locked options instance has already been registered and can no longer be reconfigured.");
    }

    [Fact]
    public void UseInstance_ShouldReplaceExistingLockedInstance_WhenCalled()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var initialOptions = new TestOptions { Property1 = "Initial" };
        var newOptions = new TestOptions { Property1 = "New" };

        // Act
        service.UseInstance(initialOptions);
        service.UseInstance(newOptions);

        // Assert
        PropertyInfo? instanceDictionaryField = typeof(OptionsInstanceService)
            .GetProperty("InstanceDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
        var instanceDictionary = instanceDictionaryField!.GetValue(service) as ConcurrentDictionary<Type, OptionsInstanceDictionaryItem>;

        bool success = instanceDictionary!.TryGetValue(typeof(TestOptions), out OptionsInstanceDictionaryItem? item);

        success.Should().BeTrue();
        item.Should().NotBeNull();
        item!.Options.Should().BeEquivalentTo(newOptions);
        item.IsLocked.Should().BeTrue();
    }

    [Fact]
    public void UseInstance_ShouldReplaceInstance_WhenCalledMultipleTimes()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();
        var initialOptions = new TestOptions { Property1 = "Initial" };
        var newOptions = new TestOptions { Property1 = "New" };

        // Act
        service.UseInstance(initialOptions);
        service.UseInstance(newOptions);

        // Assert
        TestOptions configuredInstance = service.GetOptionsInstance<TestOptions>(Substitute.For<IConfiguration>(), null);
        configuredInstance.Property1.Should().Be("New");
    }

    [Fact]
    public void UseInstance_ShouldThrowException_WhenNullInstanceProvided()
    {
        // Arrange
        var service = OptionsInstanceService.CreateTestInstance();

        // Act
        Action act = () => service.UseInstance<TestOptions>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    private sealed class TestOptions
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
    }

    private sealed class AnotherTestOptions
    {
        public string Property1 { get; set; }
    }
}
