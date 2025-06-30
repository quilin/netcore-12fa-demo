using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Authorization.AccessManagement;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.Authorization;

internal class TopicIntentionResolver :
    IIntentionResolver<TopicIntention>,
    IIntentionResolver<TopicIntention, Topic>
{
    public bool IsAllowed(IIdentity subject, TopicIntention intention) => intention switch
    {
        TopicIntention.Create => subject.IsAuthenticated(),
        _ => false
    };

    public bool IsAllowed(IIdentity subject, TopicIntention intention, Topic target) => intention switch
    {
        TopicIntention.CreateComment => subject.IsAuthenticated(),
        _ => false
    };
}