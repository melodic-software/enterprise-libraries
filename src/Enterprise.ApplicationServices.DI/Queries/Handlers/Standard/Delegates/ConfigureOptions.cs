using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Delegates;

public delegate void ConfigureOptions<TQuery, TResponse>(RegistrationOptions<TQuery, TResponse> registrationOptions) 
    where TQuery : IBaseQuery;
