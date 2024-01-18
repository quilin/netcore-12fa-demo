using TFA.Domain.Authentication;

namespace TFA.Domain.Authorization;

public interface IIntentionManager
{
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;
}

internal class IntentionManager(
    IEnumerable<IIntentionResolver> resolvers,
    IIdentityProvider identityProvider)
    : IIntentionManager
{
    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
    {
        var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
        return matchingResolver?.IsAllowed(identityProvider.Current, intention) ?? false;
    }
}

internal static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention)
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention))
        {
            throw new IntentionManagerException();
        }
    }
}