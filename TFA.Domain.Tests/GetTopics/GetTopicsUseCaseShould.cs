using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics;

public class GetTopicsUseCaseShould
{
    private readonly GetTopicsUseCase sut;
    private readonly Mock<IGetTopicsStorage> storage;
    private readonly ISetup<IGetTopicsStorage,Task<(IEnumerable<Topic> resources, int totalCount)>> getTopicsSetup;
    private readonly ISetup<IGetForumsStorage,Task<IEnumerable<Forum>>> getForumsSetup;

    public GetTopicsUseCaseShould()
    {
        var getForumsStorage = new Mock<IGetForumsStorage>();
        getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

        storage = new Mock<IGetTopicsStorage>();
        getTopicsSetup = storage.Setup(s =>
            s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        sut = new GetTopicsUseCase(getForumsStorage.Object, storage.Object);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoForum()
    {
        var forumId = Guid.Parse("64C3B227-8D4A-4A0E-A161-04F19C2ABBC4");

        getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = Guid.Parse("01B1C554-184B-4B32-913E-F7031AAD3BAC") } });

        var query = new GetTopicsQuery(forumId, 0, 1);
        await sut.Invoking(s => s.Handle(query, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnTopics_ExtractedFromStorage_WhenForumExists()
    {
        var forumId = Guid.Parse("845D0972-0E11-4BD2-A102-299E99590267");

        getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = Guid.Parse("845D0972-0E11-4BD2-A102-299E99590267") } });
        var expectedResources = new Topic[] { new() };
        var expectedTotalCount = 6;
        getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));

        var (actualResources, actualTotalCount) = await sut.Handle(
            new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedResources);
        actualTotalCount.Should().Be(expectedTotalCount);
        storage.Verify(s => s.GetTopics(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
    }
}