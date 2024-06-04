using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureOptions<TQuery, TResponse>(RegistrationOptions<TQuery, TResponse> options)
    where TQuery : IBaseQuery;
