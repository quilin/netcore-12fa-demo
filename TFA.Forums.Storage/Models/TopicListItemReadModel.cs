namespace TFA.Forums.Storage.Models;

internal class TopicListItemReadModel
{
    public Guid TopicId { get; set; }
    public Guid ForumId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Title { get; set; }
    public int TotalCommentsCount { get; set; }
    public DateTimeOffset? LastCommentCreatedAt { get; set; }
    public Guid? LastCommentId { get; set; }
}