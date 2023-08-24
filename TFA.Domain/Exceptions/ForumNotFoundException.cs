namespace TFA.Domain.Exceptions;

public class ForumNotFoundException : DomainException
{
    public ForumNotFoundException(Guid forumId) : base(ErrorCode.Gone, $"Forum with id {forumId} was not found")
    {
    }
}

public abstract class DomainException : Exception
{
    public ErrorCode ErrorCode { get; }

    public DomainException(ErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
