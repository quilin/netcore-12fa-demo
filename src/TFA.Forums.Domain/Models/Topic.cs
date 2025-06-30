namespace TFA.Forums.Domain.Models;

public class Topic
{
    public Guid Id { get; set; }
    public Guid ForumId { get; set; }
    public Guid UserId { get; set; }
    public required string Title { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}