using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage;
using Forum = TFA.Storage.Forum;

namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly ForumDbContext forumDbContext;
    private readonly ISetup<IGuidFactory,Guid> createIdSetup;
    private readonly ISetup<IMomentProvider,DateTimeOffset> getNowSetup;
    private readonly CreateTopicUseCase sut;

    public CreateTopicUseCaseShould()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));
        forumDbContext = new ForumDbContext(dbContextOptionsBuilder.Options);

        var guidFactory = new Mock<IGuidFactory>();
        createIdSetup = guidFactory.Setup(f => f.Create());

        var momentProvider = new Mock<IMomentProvider>();
        getNowSetup = momentProvider.Setup(p => p.Now);

        sut = new CreateTopicUseCase(guidFactory.Object, momentProvider.Object, forumDbContext);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        await forumDbContext.Forums.AddAsync(new Forum
        {
            ForumId = Guid.Parse("CB9E8F9A-D7C8-47E6-9D36-E195104FAA71"),
            Title = "Basic forum"
        });
        await forumDbContext.SaveChangesAsync();

        var forumId = Guid.Parse("5E1DCF96-E8F3-41C9-BD59-6479140933B3");
        var authorId = Guid.Parse("C77B641A-86BC-4CAF-98F5-310725B8ADBE");

        await sut.Invoking(s => s.Execute(forumId, "Some title", authorId, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic()
    {
        var forumId = Guid.Parse("E20A64A3-47E3-4076-96D0-7EF226EAF5F2");
        var userId = Guid.Parse("91B714CC-BDFF-47A1-A6DC-E71DDE8C25F7");
        await forumDbContext.Forums.AddAsync(new Forum
        {
            ForumId = forumId,
            Title = "Existing forum"
        });
        await forumDbContext.Users.AddAsync(new User
        {
            UserId = userId,
            Login = "Aleks"
        });
        await forumDbContext.SaveChangesAsync();

        createIdSetup.Returns(Guid.Parse("C5AC833A-F7AF-43FA-BFC3-B7D11623A523"));
        getNowSetup.Returns(new DateTimeOffset(2023, 07, 11, 19, 17, 00, TimeSpan.FromHours(2)));

        var actual = await sut.Execute(forumId, "Hello world", userId, CancellationToken.None);

        var allTopics = await forumDbContext.Topics.ToArrayAsync();
        allTopics.Should().BeEquivalentTo(new[]
        {
            new Topic
            {
                ForumId = forumId,
                UserId = userId,
                Title = "Hello world",
            }
        }, cfg => cfg.Including(t => t.ForumId).Including(t => t.UserId).Including(t => t.Title));
        actual.Should().BeEquivalentTo(new Models.Topic
        {
            Id = Guid.Parse("C5AC833A-F7AF-43FA-BFC3-B7D11623A523"),
            Title = "Hello world",
            Author = "Aleks",
            CreatedAt = new DateTimeOffset(2023, 07, 11, 19, 17, 00, TimeSpan.FromHours(2)),
        });
    }
}