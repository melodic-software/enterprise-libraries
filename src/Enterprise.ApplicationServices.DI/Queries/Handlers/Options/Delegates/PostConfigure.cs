using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model.Base;
using Enterprise.DI.Registration.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Options.Delegates;

public delegate void PostConfigure<TQuery, TResult>(IServiceCollection services, RegistrationContext<IHandleQuery<TQuery, TResult>> postConfigure)
    where TQuery : class, IBaseQuery;
