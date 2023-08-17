using TFA.Domain.Authentication;

namespace TFA.Domain.Authorization;

public interface IIntentionManager
{
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;

    bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct;
}

public class IntentionManager : IIntentionManager
{
    private readonly IEnumerable<IIntentionResolver> resolvers;
    private readonly IIdentityProvider identityProvider;

    public IntentionManager(
        IEnumerable<IIntentionResolver> resolvers,
        IIdentityProvider identityProvider)
    {
        this.resolvers = resolvers;
        this.identityProvider = identityProvider;
    }

    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
    {
        var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
        return matchingResolver?.IsAllowed(identityProvider.Current, intention) ?? false;
    }

    public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct
    {
        throw new NotImplementedException();
    }
}

public static class IntentionManagerExtensions
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