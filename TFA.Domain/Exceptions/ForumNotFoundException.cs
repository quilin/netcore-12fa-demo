namespace TFA.Domain.Exceptions;

public class ForumNotFoundException : DomainException
{
    public ForumNotFoundException(Guid forumId) : base(DomainErrorCode.Gone, $"Forum with id {forumId} was not found")
    {
    }
}

public abstract class DomainException : Exception
{
    public DomainErrorCode DomainErrorCode { get; }

    public DomainException(DomainErrorCode domainErrorCode, string message) : base(message)
    {
        DomainErrorCode = domainErrorCode;
    }
}
