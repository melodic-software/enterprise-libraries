using Enterprise.Events.Callbacks.Registration.Abstract;

namespace Enterprise.ApplicationServices.Core.Queries.Dispatching;

public interface IQueryDispatchFacade : IDispatchQueries, IRegisterEventCallbacks, IClearCallbacks;
