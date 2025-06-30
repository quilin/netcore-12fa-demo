namespace TFA.Forums.Domain.Exceptions;

public abstract class DomainException(DomainErrorCode domainErrorCode, string message) : Exception(message)
{
    public DomainErrorCode DomainErrorCode { get; } = domainErrorCode;
}