using System.Net.Http.Json;
using FluentAssertions;
using TFA.Domain.Authentication;

namespace TFA.E2E;

public class AccountEndpointsShould(ForumApiApplicationFactory factory) : IClassFixture<ForumApiApplicationFactory>
{
    [Fact]
    public async Task SignInAfterSignOn()
    {
        using var httpClient = factory.CreateClient();

        // Given I create a new account
        // | Login | Password |
        // | Test  | qwerty   |
        using var signOnResponse = await httpClient.PostAsync(
            "account", JsonContent.Create(new { login = "Test", password = "qwerty" }));
        signOnResponse.IsSuccessStatusCode.Should().BeTrue();
        var createdUser = await signOnResponse.Content.ReadFromJsonAsync<User>();

        using var signInResponse = await httpClient.PostAsync(
            "account/signin", JsonContent.Create(new { login = "Test", password = "qwerty" }));
        signInResponse.IsSuccessStatusCode.Should().BeTrue();

        var signedInUser = await signInResponse.Content.ReadFromJsonAsync<User>();
        signedInUser!.UserId.Should().Be(createdUser!.UserId);

        var createForumResponse = await httpClient.PostAsync(
            "forums", JsonContent.Create(new { title = "Test title" }));
        createForumResponse.IsSuccessStatusCode.Should().BeTrue();
    }
}