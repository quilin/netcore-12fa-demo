using TFA.Forums.Domain.Authentication;

namespace TFA.Forums.Domain.Authorization.AccessManagement;

public interface IIntentionManager
{
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;
    bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct;
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

    public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct
    {
        var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention, TObject>>().FirstOrDefault();
        return matchingResolver?.IsAllowed(identityProvider.Current, intention, target) ?? false;
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

    public static void ThrowIfForbidden<TIntention, TObject>(this IIntentionManager intentionManager,
        TIntention intention,
        TObject target)
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention, target))
        {
            throw new IntentionManagerException();
        }
    }
}