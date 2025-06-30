namespace TFA.Forums.Domain.Models;

public class Forum
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
}