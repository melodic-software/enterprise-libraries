using Enterprise.ApplicationServices.Core.Queries.Dispatching;
using Enterprise.Events.Callbacks.Registration.Abstract;

namespace Enterprise.ApplicationServices.Core.Queries.Facade;

public interface IQueryDispatchFacade : IDispatchQueries, IRegisterEventCallbacks, IClearCallbacks;
