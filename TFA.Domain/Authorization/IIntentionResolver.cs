using TFA.Domain.Authentication;

namespace TFA.Domain.Authorization;

public interface IIntentionResolver
{
}

public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention);
}
