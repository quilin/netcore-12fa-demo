using FluentValidation;
using MediatR;
using TFA.Domain.Authentication;

namespace TFA.Domain.UseCases.SignOn;

internal class SignOnUseCase : IRequestHandler<SignOnCommand, IIdentity>
{
    private readonly IValidator<SignOnCommand> validator;
    private readonly IPasswordManager passwordManager;
    private readonly ISignOnStorage storage;

    public SignOnUseCase(
        IValidator<SignOnCommand> validator,
        IPasswordManager passwordManager,
        ISignOnStorage storage)
    {
        this.validator = validator;
        this.passwordManager = passwordManager;
        this.storage = storage;
    }

    public async Task<IIdentity> Handle(SignOnCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
        var userId = await storage.CreateUser(command.Login, salt, hash, cancellationToken);

        return new User(userId, Guid.Empty);
    }
}