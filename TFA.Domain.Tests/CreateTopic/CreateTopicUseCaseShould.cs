using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.Models;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.Tests.CreateTopic;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase sut;
    private readonly Mock<ICreateTopicStorage> storage;
    private readonly ISetup<ICreateTopicStorage, Task<Topic>> createTopicSetup;
    private readonly ISetup<IIdentity, Guid> getCurrentUserIdSetup;
    private readonly ISetup<IIntentionManager, bool> intentionIsAllowedSetup;
    private readonly Mock<IIntentionManager> intentionManager;
    private readonly Mock<IGetForumsStorage> getForumsStorage;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> getForumsSetup;

    public CreateTopicUseCaseShould()
    {
        storage = new Mock<ICreateTopicStorage>();
        createTopicSetup = storage.Setup(s =>
            s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        getForumsStorage = new Mock<IGetForumsStorage>();
        getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        getCurrentUserIdSetup = identity.Setup(s => s.UserId);

        intentionManager = new Mock<IIntentionManager>();
        intentionIsAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntention>()));

        var validator = new Mock<IValidator<CreateTopicCommand>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        sut = new(validator.Object, intentionManager.Object, identityProvider.Object, getForumsStorage.Object,
            storage.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
    {
        var forumId = Guid.Parse("3BB52FCF-FA8F-4DA3-9954-25A67F75B71A");

        intentionIsAllowedSetup.Returns(false);

        await sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Whatever"), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
        intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        var forumId = Guid.Parse("5E1DCF96-E8F3-41C9-BD59-6479140933B3");

        intentionIsAllowedSetup.Returns(true);
        getForumsSetup.ReturnsAsync(Array.Empty<Forum>());

        (await sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Some title"), CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>())
            .Which.DomainErrorCode.Should().Be(DomainErrorCode.Gone);
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
    {
        var forumId = Guid.Parse("E20A64A3-47E3-4076-96D0-7EF226EAF5F2");
        var userId = Guid.Parse("91B714CC-BDFF-47A1-A6DC-E71DDE8C25F7");

        intentionIsAllowedSetup.Returns(true);
        getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = forumId } });
        getCurrentUserIdSetup.Returns(userId);
        var expected = new Topic();
        createTopicSetup.ReturnsAsync(expected);

        var actual = await sut.Execute(new CreateTopicCommand(forumId, "Hello world"), CancellationToken.None);
        actual.Should().Be(expected);

        storage.Verify(s => s.CreateTopic(forumId, userId, "Hello world", It.IsAny<CancellationToken>()), Times.Once);
    }
}