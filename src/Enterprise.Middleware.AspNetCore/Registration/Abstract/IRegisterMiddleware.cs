using Microsoft.AspNetCore.Builder;

namespace Enterprise.Middleware.AspNetCore.Registration.Abstract;

public interface IRegisterMiddleware
{
    /// <summary>
    /// Registers middleware components in the request pipeline.
    /// </summary>
    /// <param name="app">The web application used for registration.</param>
    public static abstract void RegisterMiddleware(WebApplication app);
}
