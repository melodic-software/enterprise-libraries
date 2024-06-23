using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Example.Api.ApplicationServices.Queries.Results;

namespace Example.Api.ApplicationServices.Queries.Standard.Bound.Simple;

// Our regular unbound query representation (result is specified by the handler).
// This is an alternative generic - where the result type is bound to the type specified.

public class BoundQuery : IQuery<QueryResult>;
