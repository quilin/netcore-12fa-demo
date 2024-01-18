using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.UseCases.SignIn;

namespace TFA.Domain.Tests.SignIn;

public class SignInUseCaseShould
{
    private readonly SignInUseCase sut;
    private readonly ISetup<ISignInStorage, Task<RecognisedUser?>> findUserSetup;
    private readonly ISetup<IPasswordManager, bool> comparePasswordsSetup;
    private readonly ISetup<ISymmetricEncryptor, Task<string>> encryptSetup;
    private readonly ISetup<ISignInStorage,Task<Guid>> createSessionSetup;
    private readonly Mock<ISignInStorage> storage;
    private readonly Mock<ISymmetricEncryptor> encryptor;

    public SignInUseCaseShould()
    {
        storage = new Mock<ISignInStorage>();
        findUserSetup = storage.Setup(s => s.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        createSessionSetup = storage.Setup(s => s.CreateSession(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()));

        var passwordManager = new Mock<IPasswordManager>();
        comparePasswordsSetup = passwordManager.Setup(m => m.ComparePasswords(
            It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));

        encryptor = new Mock<ISymmetricEncryptor>();
        encryptSetup = encryptor.Setup(e =>
            e.Encrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        var configuration = new Mock<IOptions<AuthenticationConfiguration>>();
        configuration
            .Setup(c => c.Value)
            .Returns(new AuthenticationConfiguration
            {
                Base64Key = "XtDotH86WLjaEoFev6uZFN/3C0EQIApoD+5iqqmPtpg="
            });

        sut = new SignInUseCase(
            storage.Object,
            passwordManager.Object,
            encryptor.Object,
            configuration.Object);
    }

    [Fact]
    public async Task ThrowValidationException_WhenUserNotFound()
    {
        findUserSetup.ReturnsAsync(() => null);
        (await sut.Invoking(s => s.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().Contain(e => e.PropertyName == "Login");
    }

    [Fact]
    public async Task ThrowValidationException_WhenPasswordDoesntMatch()
    {
        findUserSetup.ReturnsAsync(new RecognisedUser());
        comparePasswordsSetup.Returns(false);

        (await sut.Invoking(s => s.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
            .Which.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public async Task CreateSession_WhenPasswordMatches()
    {
        var userId = Guid.Parse("EA065C67-492D-446B-9B50-1D8EABF59BD6");
        var sessionId = Guid.Parse("1D5FD923-583D-4F1B-A305-7E2E1A6CFD54");
        findUserSetup.ReturnsAsync(new RecognisedUser { UserId = userId });
        comparePasswordsSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);

        await sut.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None);
        storage.Verify(s => s.CreateSession(userId, It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnTokenAndIdentity()
    {
        var userId = Guid.Parse("154B1F4C-486F-49AA-9109-720DE5EB524D");
        var sessionId = Guid.Parse("9722BC76-FD4C-4E3F-927E-4DD1CABBA55E");
        findUserSetup.ReturnsAsync(new RecognisedUser
        {
            UserId = userId,
            PasswordHash = new byte[] { 1 },
            Salt = new byte[] { 2 }
        });
        comparePasswordsSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);
        encryptSetup.ReturnsAsync("token");

        var (identity, token) = await sut.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None);
        token.Should().NotBeEmpty();
        identity.UserId.Should().Be(userId);
        identity.SessionId.Should().Be(sessionId);
        token.Should().Be("token");
    }

    [Fact]
    public async Task EncryptSessionIdIntoToken()
    {
        var userId = Guid.Parse("EA065C67-492D-446B-9B50-1D8EABF59BD6");
        var sessionId = Guid.Parse("1d5fd923-583d-4f1b-a305-7e2e1a6cfd54");
        findUserSetup.ReturnsAsync(new RecognisedUser { UserId = userId });
        comparePasswordsSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);

        await sut.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None);

        encryptor.Verify(s => s
            .Encrypt("1d5fd923-583d-4f1b-a305-7e2e1a6cfd54", It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));
    }
}