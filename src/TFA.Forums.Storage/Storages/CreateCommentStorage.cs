using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.UseCases.CreateComment;

namespace TFA.Forums.Storage.Storages;

internal class CreateCommentStorage(
    ForumDbContext dbContext,
    IMapper mapper,
    IGuidFactory guidFactory,
    IMomentProvider momentProvider) : ICreateCommentStorage
{
    public Task<Topic?> FindTopic(Guid topicId, CancellationToken cancellationToken) => dbContext.Topics
        .Where(t => t.TopicId == topicId)
        .ProjectTo<Topic>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

    public async Task<Comment> Create(Guid topicId, Guid userId, string text, CancellationToken cancellationToken)
    {
        var commentId = guidFactory.Create();
        await dbContext.Comments.AddAsync(new Entities.Comment
        {
            CommentId = commentId,
            TopicId = topicId,
            UserId = userId,
            CreatedAt = momentProvider.Now,
            Text = text
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Comments
            .Where(c => c.CommentId == commentId)
            .ProjectTo<Comment>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}