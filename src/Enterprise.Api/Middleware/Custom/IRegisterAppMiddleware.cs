﻿using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Middleware.Custom;

public interface IRegisterAppMiddleware
{
    /// <summary>
    /// Registers application specific middleware components in the request pipeline.
    /// </summary>
    /// <param name="app">The web application used for registration.</param>
    public static abstract void RegisterAppMiddleware(WebApplication app);
}