using MediatR;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Authorization;
using TFA.Forums.Domain.Authorization.AccessManagement;
using TFA.Forums.Domain.DomainEvents;
using TFA.Forums.Domain.Exceptions;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.UseCases.CreateComment;

internal class CreateCommentUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCommentCommand, Comment>
{
    public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        await using var scope = await unitOfWork.StartScope(cancellationToken);
        var storage = scope.GetStorage<ICreateCommentStorage>();

        var topic = await storage.FindTopic(request.TopicId, cancellationToken);
        if (topic is null)
        {
            throw new TopicNotFoundException(request.TopicId);
        }

        intentionManager.ThrowIfForbidden(TopicIntention.CreateComment, topic);

        var domainEventsStorage = scope.GetStorage<IDomainEventStorage>();
        var comment = await storage.Create(
            request.TopicId, identityProvider.Current.UserId, request.Text, cancellationToken);
        await domainEventsStorage.AddEvent(ForumDomainEvent.CommentCreated(topic, comment), cancellationToken);

        await scope.Commit(cancellationToken);

        return comment;
    }
}