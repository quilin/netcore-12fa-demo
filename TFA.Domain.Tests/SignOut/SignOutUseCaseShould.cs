using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.UseCases.SignOut;

namespace TFA.Domain.Tests.SignOut;

public class SignOutUseCaseShould
{
    private readonly SignOutUseCase sut;
    private readonly Mock<ISignOutStorage> storage;
    private readonly ISetup<ISignOutStorage, Task> removeSessionSetup;
    private readonly ISetup<IIdentityProvider, IIdentity> currentIdentitySetup;
    private readonly ISetup<IIntentionManager, bool> signOutIsAllowedSetup;

    public SignOutUseCaseShould()
    {
        storage = new Mock<ISignOutStorage>();
        removeSessionSetup = storage.Setup(s => s.RemoveSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var identityProvider = new Mock<IIdentityProvider>();
        currentIdentitySetup = identityProvider.Setup(p => p.Current);

        var intentionManager = new Mock<IIntentionManager>();
        signOutIsAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<AccountIntention>()));

        sut = new SignOutUseCase(
            intentionManager.Object,
            identityProvider.Object,
            storage.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenUserIsNotAuthenticated()
    {
        signOutIsAllowedSetup.Returns(false);
        await sut.Invoking(s => s.Execute(new SignOutCommand(), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
    }

    [Fact]
    public async Task RemoveCurrentIdentitySession()
    {
        signOutIsAllowedSetup.Returns(true);
        var sessionId = Guid.Parse("DC933EC9-C7B4-4CAA-A3DE-A394FF55BBEF");
        currentIdentitySetup.Returns(new User(Guid.Empty, sessionId));
        removeSessionSetup.Returns(Task.CompletedTask);

        await sut.Execute(new SignOutCommand(), CancellationToken.None);

        storage.Verify(s => s.RemoveSession(sessionId, It.IsAny<CancellationToken>()), Times.Once);
        storage.VerifyNoOtherCalls();
    }
}