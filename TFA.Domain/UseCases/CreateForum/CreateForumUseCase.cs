using FluentValidation;
using MediatR;
using TFA.Domain.Authorization;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumUseCase : IRequestHandler<CreateForumCommand, Forum>
{
    private readonly IValidator<CreateForumCommand> validator;
    private readonly IIntentionManager intentionManager;
    private readonly ICreateForumStorage storage;

    public CreateForumUseCase(
        IValidator<CreateForumCommand> validator,
        IIntentionManager intentionManager,
        ICreateForumStorage storage)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.storage = storage;
    }

    public async Task<Forum> Handle(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        return await storage.Create(command.Title, cancellationToken);
    }
}