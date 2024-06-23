using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Example.Api.ApplicationServices.Queries.ChainOfResponsibility;

// Our regular unbound query representation (result is specified by the handler).
// This is an alternative generic - where the result type is bound to the type specified.

public class Query : IQuery;
