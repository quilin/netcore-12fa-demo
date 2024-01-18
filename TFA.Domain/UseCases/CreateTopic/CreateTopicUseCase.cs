using MediatR;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.UseCases.GetForums;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    IGetForumsStorage getForumsStorage,
    ICreateTopicStorage storage)
    : IRequestHandler<CreateTopicCommand, Topic>
{
    public async Task<Topic> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        var (forumId, title) = command;
        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        await getForumsStorage.ThrowIfForumNotFound(forumId, cancellationToken);

        return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
    }
}