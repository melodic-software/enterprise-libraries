using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.DI.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Options.Delegates;

public delegate void PostConfigure<TQuery, TResult>(IServiceCollection services, RegistrationContext<IHandleQuery<TQuery, TResult>> postConfigure)
    where TQuery : class, IQuery;
