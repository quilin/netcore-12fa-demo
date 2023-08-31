using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Storage.Storages;

namespace TFA.Storage.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForumStorage(this IServiceCollection services, string dbConnectionString) =>
        services
            .AddScoped<IGetForumsStorage, GetForumsStorage>()
            .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
            .AddScoped<IGuidFactory, GuidFactory>()
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddDbContextPool<ForumDbContext>(options => options
                .UseNpgsql(dbConnectionString));
}