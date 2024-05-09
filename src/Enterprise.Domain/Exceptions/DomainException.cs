namespace Enterprise.Domain.Exceptions;

/// <summary>
/// This base implementation can be used in higher layers to handle and map to responses.
/// In the case of a Web API, these can be caught globally and translated to a 422 status code (Unprocessable Entity) response.
/// Exceptions are expensive, so consider using the result pattern over throwing exceptions for business logic / domain validation errors.
/// </summary>
public abstract class DomainException : Exception;
