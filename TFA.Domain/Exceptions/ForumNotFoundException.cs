namespace TFA.Domain.Exceptions;

public class ForumNotFoundException : Exception
{
    public ForumNotFoundException(Guid forumId) : base($"Forum with id {forumId} was not found")
    {
    }
}