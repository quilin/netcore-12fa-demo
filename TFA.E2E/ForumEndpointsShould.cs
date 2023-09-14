using System.Net.Http.Json;
using FluentAssertions;
using TFA.API.Models;

namespace TFA.E2E;

public class ForumEndpointsShould : IClassFixture<ForumApiApplicationFactory>
{
    private readonly ForumApiApplicationFactory factory;

    public ForumEndpointsShould(ForumApiApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task CreateNewForum()
    {
        using var httpClient = factory.CreateClient();

        using var getInitialForumsResponse = await httpClient.GetAsync("forums");
        var initialForums = await getInitialForumsResponse.Content.ReadFromJsonAsync<Forum[]>();
        initialForums
            .Should().NotBeNull().And
            .Subject.As<Forum[]>().Should().BeEmpty();

        using var response = await httpClient.PostAsync("forums",
            JsonContent.Create(new { title = "Test" }));

        response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
        var forum = await response.Content.ReadFromJsonAsync<Forum>();
        forum
            .Should().NotBeNull().And
            .Subject.As<Forum>().Title.Should().Be("Test");

        using var getForumsResponse = await httpClient.GetAsync("forums");
        var forums = await getForumsResponse.Content.ReadFromJsonAsync<Forum[]>();
        forums
            .Should().NotBeNull().And
            .Subject.As<Forum[]>().Should().Contain(f => f.Title == "Test");
    }
}