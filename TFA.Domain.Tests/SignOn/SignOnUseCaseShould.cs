using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.UseCases.SignOn;

namespace TFA.Domain.Tests.SignOn;

public class SignOnUseCaseShould
{
    private readonly SignOnUseCase sut;
    private readonly ISetup<IPasswordManager,(byte[] Salt, byte[] Hash)> generatePasswordPartsSetup;
    private readonly ISetup<ISignOnStorage,Task<Guid>> createUserSetup;
    private readonly Mock<ISignOnStorage> storage;

    public SignOnUseCaseShould()
    {
        var validator = new Mock<IValidator<SignOnCommand>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<SignOnCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var passwordManager = new Mock<IPasswordManager>();
        generatePasswordPartsSetup = passwordManager.Setup(m => m.GeneratePasswordParts(It.IsAny<string>()));

        storage = new Mock<ISignOnStorage>();
        createUserSetup = storage.Setup(s =>
            s.CreateUser(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        sut = new SignOnUseCase(validator.Object, passwordManager.Object, storage.Object);
    }

    [Fact]
    public async Task CreateUser_WithGeneratedPasswordParts()
    {
        var salt = new byte[] { 1 };
        var hash = new byte[] { 2 };
        generatePasswordPartsSetup.Returns((Salt: salt, Hash: hash));

        await sut.Execute(new SignOnCommand("Test", "qwerty"), CancellationToken.None);
        
        storage.Verify(s => s.CreateUser("Test", salt, hash, It.IsAny<CancellationToken>()), Times.Once);
        storage.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ReturnIdentityOfNewlyCreatedUser()
    {
        generatePasswordPartsSetup.Returns((Salt: new byte[] { 1 }, Hash: new byte[] { 2 }));
        createUserSetup.ReturnsAsync(Guid.Parse("7483221E-FE0E-44EE-85B6-94D5279A8988"));

        var actual = await sut.Execute(new SignOnCommand("Test", "qwerty"), CancellationToken.None);
        actual.UserId.Should().Be(Guid.Parse("7483221E-FE0E-44EE-85B6-94D5279A8988"));
    }
}