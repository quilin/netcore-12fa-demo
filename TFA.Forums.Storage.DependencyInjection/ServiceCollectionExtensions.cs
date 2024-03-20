using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forums.Domain;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.UseCases;
using TFA.Forums.Domain.UseCases.CreateForum;
using TFA.Forums.Domain.UseCases.CreateTopic;
using TFA.Forums.Domain.UseCases.GetForums;
using TFA.Forums.Domain.UseCases.GetTopics;
using TFA.Forums.Domain.UseCases.SignIn;
using TFA.Forums.Domain.UseCases.SignOn;
using TFA.Forums.Domain.UseCases.SignOut;
using TFA.Forums.Storage.Storages;

namespace TFA.Forums.Storage.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForumStorage(this IServiceCollection services, string dbConnectionString)
    {
        services
            .AddScoped<IDomainEventStorage, DomainEventStorage>()
            .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
            .AddScoped<ICreateForumStorage, CreateForumStorage>()
            .AddScoped<IGetForumsStorage, GetForumsStorage>()
            .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
            .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
            .AddScoped<ISignOnStorage, SignOnStorage>()
            .AddScoped<ISignInStorage, SignInStorage>()
            .AddScoped<ISignOutStorage, SignOutStorage>()
            .AddScoped<IGuidFactory, GuidFactory>()
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddDbContextPool<ForumDbContext>(options => options
                .UseNpgsql(dbConnectionString));

        services.AddSingleton<IUnitOfWork, UnitOfWork>();

        services.AddMemoryCache();

        services.AddAutoMapper(config => config
            .AddMaps(Assembly.GetAssembly(typeof(ForumDbContext))));

        return services;
    }
}