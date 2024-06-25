using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Middleware.AspNetCore.Registration.Abstract;

namespace Enterprise.ModularMonoliths.Model;

/// <summary>
/// Represents a module in a modular monolith.
/// </summary>
public interface IModule : IRegisterServices, IRegisterMiddleware
{
    string Name { get; }
    public IEnumerable<string> Policies { get; set; }
}
