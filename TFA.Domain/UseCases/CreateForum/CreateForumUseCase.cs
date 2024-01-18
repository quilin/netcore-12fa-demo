using MediatR;
using TFA.Domain.Authorization;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateForum;

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