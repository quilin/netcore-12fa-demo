namespace TFA.Forums.API.Models;

public class Topic
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}