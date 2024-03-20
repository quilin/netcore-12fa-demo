using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.UseCases.CreateForum;

public interface ICreateForumStorage
{
    public Task<Forum> Create(string title, CancellationToken cancellationToken);
}