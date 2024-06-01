using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Commands;

public sealed class CommandHandlerRegistrationOptions
{
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Transient;
    public bool UseDefaultDecorators { get; set; }
    public bool UseChainOfResponsibility { get; set; } = true;
}
