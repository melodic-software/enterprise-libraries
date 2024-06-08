﻿using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Options.Delegates;

public delegate void PostConfigure<TQuery, TResponse>(IServiceCollection services, RegistrationContext<IHandleQuery<TQuery, TResponse>> postConfigure)
    where TQuery : IBaseQuery;