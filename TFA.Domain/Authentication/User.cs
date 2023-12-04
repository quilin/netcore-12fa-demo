namespace TFA.Domain.Authentication;

public class User : IIdentity
{
    public User(Guid userId, Guid sessionId)
    {
        UserId = userId;
        SessionId = sessionId;
    }

    public Guid UserId { get; }
    public Guid SessionId { get; }

    public static User Guest => new(Guid.Empty, Guid.Empty);
}