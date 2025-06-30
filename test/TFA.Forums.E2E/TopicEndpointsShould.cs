using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace TFA.Forums.E2E;

public class TopicEndpointsShould(ForumApiApplicationFactory factory)
    : IClassFixture<ForumApiApplicationFactory>, IAsyncLifetime
{
    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;

    // [Fact]
    public async Task ReturnForbidden_WhenNotAuthenticated()
    {
        using var httpClient = factory.CreateClient();

        using var forumCreatedResponse = await httpClient.PostAsync("forums",
            JsonContent.Create(new { title = "Test forum" }));
        forumCreatedResponse.EnsureSuccessStatusCode();

        var createdForum = await forumCreatedResponse.Content.ReadFromJsonAsync<API.Models.Forum>();
        createdForum.Should().NotBeNull();

        var responseMessage = await httpClient.PostAsync($"forums/{createdForum!.Id}/topics",
            JsonContent.Create(new { title = "Hello world" }));
        responseMessage.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}