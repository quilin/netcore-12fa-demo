using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.UseCases.CreateComment;

public interface ICreateCommentStorage : IStorage
{
    Task<Topic?> FindTopic(Guid topicId, CancellationToken cancellationToken);
    Task<Comment> Create(Guid topicId, Guid userId, string text, CancellationToken cancellationToken);
}