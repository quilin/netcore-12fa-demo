using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.DomainEvents;

public class ForumDomainEvent
{
    private ForumDomainEvent()
    {
    }

    public ForumDomainEventType EventType { get; init; }

    public Guid TopicId { get; init; }

    public string Title { get; init; } = null!;
    
    public ForumComment? Comment { get; init; }

    public class ForumComment
    {
        public Guid CommentId { get; init; }
        public string Text { get; init; } = null!;
    }

    public static ForumDomainEvent TopicCreated(Topic topic) => new()
    {
        EventType = ForumDomainEventType.TopicCreated,
        TopicId = topic.Id,
        Title = topic.Title
    };

    public static ForumDomainEvent CommentCreated(Topic topic, Comment comment) => new()
    {
        EventType = ForumDomainEventType.CommentCreated,
        TopicId = topic.Id,
        Title = topic.Title,
        Comment = new ForumComment
        {
            CommentId = comment.Id,
            Text = comment.Text
        }
    };
}

public enum ForumDomainEventType
{
    TopicCreated = 100,
    TopicUpdated = 101,
    TopicDeleted = 102,

    CommentCreated = 200,
    CommentUpdated = 201,
    CommentDeleted = 202,
}