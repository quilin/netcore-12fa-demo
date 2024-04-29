using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.UseCases.GetTopics;
using TFA.Forums.Storage.Models;

namespace TFA.Forums.Storage.Storages;

internal class GetTopicsStorage(
    ForumDbContext dbContext,
    IMapper mapper) : IGetTopicsStorage
{
    public async Task<(IEnumerable<Topic> resources, int totalCount)> GetTopics(
        Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {
        var query = dbContext.Topics.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync(cancellationToken);

        var resources = await dbContext.Database.SqlQuery<TopicListItemReadModel>($@"
            SELECT
                t.""TopicId"" as ""TopicId"",
                t.""ForumId"" as ""ForumId"",
                t.""UserId"" as ""UserId"",
                t.""Title"" as ""Title"",
                t.""CreatedAt"" as ""CreatedAt"",
                COALESCE(c.TotalCommentsCount, 0) as ""TotalCommentsCount"",
                c.""CreatedAt"" as ""LastCommentCreatedAt"",
                c.""CommentId"" as ""LastCommentId""
            FROM ""Topics"" as t
            LEFT JOIN (
                SELECT
                    ""TopicId"",
                    ""CommentId"",
                    ""CreatedAt"",
                    COUNT(*) OVER (PARTITION BY ""TopicId"") as TotalCommentsCount,
                    ROW_NUMBER() OVER (PARTITION BY ""TopicId"" ORDER BY ""CreatedAt"" DESC) rn
                FROM ""Comments""
            ) as c ON t.""TopicId"" = c.""TopicId"" AND c.rn = 1
            WHERE t.""ForumId"" = {forumId}
            ORDER BY
                COALESCE(c.""CreatedAt"", t.""CreatedAt"") DESC
            LIMIT {take} OFFSET {skip}")
            .ProjectTo<Topic>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

        return (resources, totalCount);
    }
}