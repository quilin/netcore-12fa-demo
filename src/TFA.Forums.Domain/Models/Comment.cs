namespace TFA.Forums.Domain.Models;

public class Comment
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required string Text { get; set; }
    public required string AuthorLogin { get; set; }
}