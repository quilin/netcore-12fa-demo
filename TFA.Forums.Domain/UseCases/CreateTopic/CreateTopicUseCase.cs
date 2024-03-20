using MediatR;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Authorization;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.UseCases.GetForums;

namespace TFA.Forums.Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    IGetForumsStorage getForumsStorage,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTopicCommand, Topic>
{
    public async Task<Topic> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        var (forumId, title) = command;
        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        await getForumsStorage.ThrowIfForumNotFound(forumId, cancellationToken);

        await using var scope = await unitOfWork.StartScope(cancellationToken);
        var topic = await scope.GetStorage<ICreateTopicStorage>()
            .CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
        await scope.GetStorage<IDomainEventStorage>().AddEvent(topic, cancellationToken);
        await scope.Commit(cancellationToken);

        return topic;
    }
}