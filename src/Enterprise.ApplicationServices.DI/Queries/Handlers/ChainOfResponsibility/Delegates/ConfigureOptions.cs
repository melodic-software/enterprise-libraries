using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureOptions<TQuery, TResult>(RegistrationOptions<TQuery, TResult> options)
    where TQuery : class, IQuery;
