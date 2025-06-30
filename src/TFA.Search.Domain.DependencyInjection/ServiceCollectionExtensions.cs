using Microsoft.Extensions.DependencyInjection;
using TFA.Search.Domain.Models;

namespace TFA.Search.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSearchDomain(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SearchEntity>());

        return services;
    }
}