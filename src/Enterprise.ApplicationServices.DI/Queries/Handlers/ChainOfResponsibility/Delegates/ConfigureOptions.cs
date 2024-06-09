using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureOptions<TQuery, TResult>(RegistrationOptions<TQuery, TResult> options)
    where TQuery : IBaseQuery;
