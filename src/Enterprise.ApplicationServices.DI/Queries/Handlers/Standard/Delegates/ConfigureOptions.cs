using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Delegates;

public delegate void ConfigureOptions<TQuery, TResult>(RegistrationOptions<TQuery, TResult> registrationOptions) 
    where TQuery : IBaseQuery;
