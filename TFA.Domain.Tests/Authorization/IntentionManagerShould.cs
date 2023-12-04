using System.Net;
using FluentAssertions;
using Moq;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateForum;

namespace TFA.Domain.Tests.Authorization;

public class IntentionManagerShould
{
    [Fact]
    public void ReturnFalse_WhenNoMatchingResolver()
    {
        var sut = new IntentionManager(
            new IIntentionResolver[]
            {
                new Mock<IIntentionResolver<DomainErrorCode>>().Object,
                new Mock<IIntentionResolver<HttpStatusCode>>().Object
            },
            new Mock<IIdentityProvider>().Object);

        sut.IsAllowed(ForumIntention.Create).Should().BeFalse();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void ReturnMatchingResolverResult(bool expectedResolverResult, bool expected)
    {
        var resolver = new Mock<IIntentionResolver<ForumIntention>>();
        resolver
            .Setup(r => r.IsAllowed(It.IsAny<IIdentity>(), It.IsAny<ForumIntention>()))
            .Returns(expectedResolverResult);

        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider
            .Setup(p => p.Current)
            .Returns(new User(Guid.Parse("08B62CDE-F28F-462B-94E8-9E521F7218B4"), Guid.Empty));

        var sut = new IntentionManager(
            new IIntentionResolver[] { resolver.Object },
            identityProvider.Object);

        sut.IsAllowed(ForumIntention.Create).Should().Be(expected);
    }
}