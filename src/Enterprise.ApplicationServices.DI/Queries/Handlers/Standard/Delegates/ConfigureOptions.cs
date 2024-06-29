using Enterprise.ApplicationServices.Core.Queries.Model.Base;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Delegates;

public delegate void ConfigureOptions<TQuery, TResult>(RegistrationOptions<TQuery, TResult> registrationOptions) 
    where TQuery : class, IBaseQuery;
