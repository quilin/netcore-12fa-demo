using TFA.Forums.Domain.Authentication;

namespace TFA.Forums.Domain.Authorization;

public interface IIntentionResolver
{
}

public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention);
}
