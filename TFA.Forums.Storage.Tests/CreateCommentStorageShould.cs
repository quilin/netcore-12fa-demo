using FluentAssertions;
using TFA.Forums.Storage.Entities;
using TFA.Forums.Storage.Storages;
using Comment = TFA.Forums.Domain.Models.Comment;

namespace TFA.Forums.Storage.Tests;

public class CreateCommentStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly CreateCommentStorage sut = new(
        fixture.GetDbContext(), fixture.GetMapper(), new GuidFactory(), new MomentProvider());

    [Fact]
    public async Task ReturnNullForTopic_WhenNoMatchingTopicInDb()
    {
        var topicId = Guid.Parse("8280601A-7743-4A05-AEA9-25EE9D9C66B7");

        var actual = await sut.FindTopic(topicId, CancellationToken.None);
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ReturnFoundTopic_WhenTopicIsPresentInDb()
    {
        var topicId = Guid.Parse("0BA4146A-E22C-41CB-A4E5-03AFB748AAE3");
        var forumId = Guid.Parse("970E824F-70F5-408D-B540-49F76769BB6A");
        var userId = Guid.Parse("6FE88E07-964A-4EC1-8131-143639F7E57D");

        await using var dbContext = fixture.GetDbContext();
        await dbContext.Topics.AddAsync(new Topic
        {
            TopicId = topicId,
            Forum = new Forum
            {
                ForumId = forumId,
                Title = "Test forum"
            },
            Author = new User
            {
                UserId = userId,
                Login = "Test user",
                Salt = [],
                PasswordHash = []
            },
            Title = "Test topic",
            CreatedAt = new DateTimeOffset(2024, 04, 25, 18, 40, 00, TimeSpan.Zero)
        }, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var actual = await sut.FindTopic(topicId, CancellationToken.None);
        actual.Should().BeEquivalentTo(new Domain.Models.Topic
        {
            Id = topicId,
            UserId = userId,
            CreatedAt = new DateTimeOffset(2024, 04, 25, 18, 40, 00, TimeSpan.Zero),
            ForumId = forumId,
            Title = "Test topic"
        });
    }

    [Fact]
    public async Task ReturnNewlyCreatedComment_WhenCreating()
    {
        var topicId = Guid.Parse("859C8234-A0A3-4E9C-88C3-DA07F67E21C4");
        var forumId = Guid.Parse("FBACDD59-BD9D-477F-BD80-F5A5AAF813D0");
        var userId = Guid.Parse("E14BF9CF-5292-422E-B52F-EC5A08C8FE92");

        await using var dbContext = fixture.GetDbContext();
        await dbContext.Topics.AddAsync(new Topic
        {
            TopicId = topicId,
            Forum = new Forum
            {
                ForumId = forumId,
                Title = "Test forum"
            },
            Author = new User
            {
                UserId = userId,
                Login = "Test user",
                Salt = [],
                PasswordHash = []
            },
            Title = "Test topic",
            CreatedAt = new DateTimeOffset(2024, 04, 25, 18, 40, 00, TimeSpan.Zero)
        }, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var comment = await sut.Create(topicId, userId, "Test comment", CancellationToken.None);
        comment.Should().BeEquivalentTo(new Comment
        {
            Text = "Test comment",
            AuthorLogin = "Test user"
        }, cfg => cfg.Excluding(c => c.Id).Excluding(c => c.CreatedAt));
    }
}