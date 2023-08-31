namespace TFA.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
}

internal class User : IIdentity
{
    public User(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}

internal static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}
