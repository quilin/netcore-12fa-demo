namespace TFA.Forums.API.Models;

public class Comment
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required string AuthorLogin { get; set; }
}