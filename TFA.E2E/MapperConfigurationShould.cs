using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace TFA.E2E;

public class MapperConfigurationShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public MapperConfigurationShould(
        WebApplicationFactory<Program> factory)
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