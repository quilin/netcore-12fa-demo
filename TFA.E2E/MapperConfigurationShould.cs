using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace TFA.E2E;

public class MapperConfigurationShould : IClassFixture<ForumApiApplicationFactory>
{
    private readonly ForumApiApplicationFactory factory;

    public MapperConfigurationShould(
        ForumApiApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public void BeValid()
    {
        var configurationProvider = factory.Services.GetRequiredService<IMapper>().ConfigurationProvider;
        configurationProvider.Invoking(p => p.AssertConfigurationIsValid()).Should().NotThrow();
    }
}