using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Authorization.AccessManagement;

namespace TFA.Forums.Domain.Authorization;

internal class ForumIntentionResolver : IIntentionResolver<ForumIntention>
{
    public bool IsAllowed(IIdentity subject, ForumIntention intention) => intention switch
    {
        ForumIntention.Create => subject.IsAuthenticated(),
        _ => false
    };
}