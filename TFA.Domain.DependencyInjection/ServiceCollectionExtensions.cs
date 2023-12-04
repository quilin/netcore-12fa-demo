using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;
using TFA.Domain.UseCases.SignIn;
using TFA.Domain.UseCases.SignOn;
using TFA.Domain.UseCases.SignOut;

namespace TFA.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForumDomain(this IServiceCollection services)
    {
        services
            .AddScoped<ICreateForumUseCase, CreateForumUseCase>()
            .AddScoped<IIntentionResolver, ForumIntentionResolver>()
            .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
            .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
            .AddScoped<IGetTopicsUseCase, GetTopicsUseCase>()
            .AddScoped<ISignOnUseCase, SignOnUseCase>()
            .AddScoped<ISignInUseCase, SignInUseCase>()
            .AddScoped<ISignOutUseCase, SignOutUseCase>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>();

        services
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIdentityProvider, IdentityProvider>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>();

        services.AddValidatorsFromAssemblyContaining<Forum>(includeInternalTypes: true);

        services.AddSingleton<DomainMetrics>();

        return services;
    }
}