using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TFA.Storage.Entities;
using TFA.Storage.Storages;

namespace TFA.Storage.Tests;

public class SignInStorageFixture : StorageTestFixture
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await using var dbContext = GetDbContext();
        await dbContext.Users.AddRangeAsync(new User
        {
            UserId = Guid.Parse("8B41C23E-123E-4F4A-93F0-BEBF9916C8B3"),
            Login = "testUser",
            Salt = new byte[] { 1 },
            PasswordHash = new byte[] { 2 }
        }, new User
        {
            UserId = Guid.Parse("85895444-65F3-47D8-857D-88F289E83D56"),
            Login = "another User",
            Salt = new byte[] { 1 },
            PasswordHash = new byte[] { 2 }
        });
        await dbContext.SaveChangesAsync();
    }
}

public class SignInStorageShould : IClassFixture<SignInStorageFixture>
{
    private readonly SignInStorageFixture fixture;
    private readonly SignInStorage sut;

    public SignInStorageShould(SignInStorageFixture fixture)
    {
        this.fixture = fixture;
        sut = new SignInStorage(
            new GuidFactory(),
            fixture.GetDbContext(),
            fixture.GetMapper());
    }

    [Fact]
    public async Task ReturnUser_WhenDatabaseContainsUserWithSameLogin()
    {
        var actual = await sut.FindUser("testUser", CancellationToken.None);
        actual.Should().NotBeNull();
        actual!.UserId.Should().Be(Guid.Parse("8B41C23E-123E-4F4A-93F0-BEBF9916C8B3"));
    }

    [Fact]
    public async Task ReturnNull_WhenDatabaseDoesntContainUserWithSameLogin()
    {
        var actual = await sut.FindUser("whatever", CancellationToken.None);
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ReturnNewlyCreatedSessionId()
    {
        var sessionId = await sut.CreateSession(
            Guid.Parse("8B41C23E-123E-4F4A-93F0-BEBF9916C8B3"),
            new DateTimeOffset(2023, 10, 12, 19, 25, 00, TimeSpan.Zero),
            CancellationToken.None);

        await using var dbContext = fixture.GetDbContext();
        (await dbContext.Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SessionId == sessionId)).Should().NotBeNull();
    }
}