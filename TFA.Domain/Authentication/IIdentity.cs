namespace TFA.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
    Guid SessionId { get; }
}

internal static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}
