namespace TFA.Forums.Domain.Exceptions;

public class ForumNotFoundException(Guid forumId)
    : DomainException(DomainErrorCode.Gone, $"Forum with id {forumId} was not found");