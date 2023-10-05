using System.Net.Http.Json;
using FluentAssertions;
using TFA.Domain.Authentication;
using Xunit.Abstractions;

namespace TFA.E2E;

public class AccountEndpointsShould : IClassFixture<ForumApiApplicationFactory>
{
    private readonly ForumApiApplicationFactory factory;
    private readonly ITestOutputHelper testOutputHelper;

    public AccountEndpointsShould(
        ForumApiApplicationFactory factory,
        ITestOutputHelper testOutputHelper)
    {
        this.factory = factory;
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task SignInAfterSignOn()
    {
        using var httpClient = factory.CreateClient();

        using var signOnResponse = await httpClient.PostAsync(
            "account", JsonContent.Create(new { login = "Test", password = "qwerty" }));
        signOnResponse.IsSuccessStatusCode.Should().BeTrue();
        var createdUser = await signOnResponse.Content.ReadFromJsonAsync<User>();

        using var signInResponse = await httpClient.PostAsync(
            "account/signin", JsonContent.Create(new { login = "Test", password = "qwerty" }));
        signInResponse.IsSuccessStatusCode.Should().BeTrue();
        signInResponse.Headers.Should().ContainKey("TFA-Auth-Token");
        testOutputHelper.WriteLine(string.Join(Environment.NewLine,
            signInResponse.Headers.Select(h => $"{h.Key} = {string.Join(", ", h.Value)}")));
        var signedInUser = await signInResponse.Content.ReadFromJsonAsync<User>();

        signedInUser.Should()
            .NotBeNull().And
            .BeEquivalentTo(createdUser);
    }
}