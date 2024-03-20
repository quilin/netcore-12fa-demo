using MediatR;
using TFA.Forums.Domain.Authorization;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
    IIntentionManager intentionManager,
    ICreateForumStorage storage)
    : IRequestHandler<CreateForumCommand, Forum>
{
    public async Task<Forum> Handle(CreateForumCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(ForumIntention.Create);
        return await storage.Create(command.Title, cancellationToken);
    }
}