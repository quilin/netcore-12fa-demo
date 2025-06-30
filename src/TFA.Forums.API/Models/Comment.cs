namespace TFA.Forums.API.Models;

public class Comment
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string AuthorLogin { get; set; }
}