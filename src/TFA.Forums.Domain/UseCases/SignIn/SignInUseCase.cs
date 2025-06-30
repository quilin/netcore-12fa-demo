using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Exceptions;

namespace TFA.Forums.Domain.UseCases.SignIn;

internal class SignInUseCase(
    ISignInStorage storage,
    IPasswordManager passwordManager,
    ISymmetricEncryptor encryptor,
    IOptions<AuthenticationConfiguration> options)
    : IRequestHandler<SignInCommand, (IIdentity identity, string token)>
{
    private readonly AuthenticationConfiguration configuration = options.Value;

    public async Task<(IIdentity identity, string token)> Handle(
        SignInCommand command, CancellationToken cancellationToken)
    {
        var recognisedUser = await storage.FindUser(command.Login, cancellationToken);
        if (recognisedUser is null)
        {
            throw new ValidationException(new ValidationFailure[]
            {
                new()
                {
                    PropertyName = nameof(command.Login),
                    ErrorCode = ValidationErrorCode.Invalid,
                    AttemptedValue = command.Login
                }
            });
        }

        var passwordMatches = passwordManager.ComparePasswords(
            command.Password, recognisedUser.Salt, recognisedUser.PasswordHash);
        if (!passwordMatches)
        {
            throw new ValidationException(new ValidationFailure[]
            {
                new()
                {
                    PropertyName = nameof(command.Password),
                    ErrorCode = ValidationErrorCode.Invalid,
                    AttemptedValue = command.Password
                }
            });
        }

        // TODO: Expiration moment generation is ugly
        var sessionId = await storage.CreateSession(
            recognisedUser.UserId, DateTimeOffset.UtcNow + TimeSpan.FromHours(1), cancellationToken);
        var token = await encryptor.Encrypt(sessionId.ToString(), configuration.Key, cancellationToken);
        return (new User(recognisedUser.UserId, sessionId), token);
    }
}