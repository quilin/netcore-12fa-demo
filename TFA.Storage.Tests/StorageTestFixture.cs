using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Testcontainers.PostgreSql;

namespace TFA.Storage.Tests;

public class StorageTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer dbContainer = new PostgreSqlBuilder().Build();

    public ForumDbContext GetDbContext() => new(new DbContextOptionsBuilder<ForumDbContext>()
        .UseNpgsql(dbContainer.GetConnectionString()).Options);

    public IMapper GetMapper() => new Mapper(new MapperConfiguration(cfg =>
        cfg.AddMaps(Assembly.GetAssembly(typeof(ForumDbContext)))));

    public IMemoryCache GetMemoryCache() => new MemoryCache(new MemoryCacheOptions());

    public virtual async Task InitializeAsync()
    {
        await dbContainer.StartAsync();
        var forumDbContext = new ForumDbContext(new DbContextOptionsBuilder<ForumDbContext>()
            .UseNpgsql(dbContainer.GetConnectionString()).Options);
        await forumDbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await dbContainer.DisposeAsync();
}