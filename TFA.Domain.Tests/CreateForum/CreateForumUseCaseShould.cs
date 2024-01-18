using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authorization;
using TFA.Domain.Models;
using TFA.Domain.UseCases.CreateForum;

namespace TFA.Domain.Tests.CreateForum;

public class CreateForumUseCaseShould
{
    private readonly CreateForumUseCase sut;
    private readonly Mock<ICreateForumStorage> storage;
    private readonly ISetup<ICreateForumStorage, Task<Forum>> createForumSetup;

    public CreateForumUseCaseShould()
    {
        var intentionManager = new Mock<IIntentionManager>();
        intentionManager
            .Setup(m => m.IsAllowed(It.IsAny<ForumIntention>()))
            .Returns(true);

        storage = new Mock<ICreateForumStorage>();
        createForumSetup = storage.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        sut = new CreateForumUseCase(intentionManager.Object, storage.Object);
    }

    [Fact]
    public async Task ReturnCreatedForum()
    {
        var forum = new Forum
        {
            Id = Guid.Parse("F7958512-2306-4E49-BE20-FC0698536125"),
            Title = "Hello"
        };
        createForumSetup.ReturnsAsync(forum);

        var actual = await sut.Handle(new CreateForumCommand("Hello"), CancellationToken.None);
        actual.Should().Be(forum);

        storage.Verify(s => s.Create("Hello", It.IsAny<CancellationToken>()), Times.Once);
        storage.VerifyNoOtherCalls();
    }
}