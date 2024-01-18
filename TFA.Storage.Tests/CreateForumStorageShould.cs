using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TFA.Storage.Storages;

namespace TFA.Storage.Tests;

public class CreateForumStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly CreateForumStorage sut = new(
        fixture.GetMemoryCache(), new GuidFactory(), fixture.GetDbContext(), fixture.GetMapper());

    [Fact]
    public async Task InsertNewForumInDatabase()
    {
        var forum = await sut.Create("Test title", CancellationToken.None);
        forum.Id.Should().NotBeEmpty();

        await using var dbContext = fixture.GetDbContext();
        var forumTitles = await dbContext.Forums
            .Where(f => f.ForumId == forum.Id)
            .Select(f => f.Title).ToArrayAsync();

        forumTitles.Should().HaveCount(1).And.Contain("Test title");
    }
}