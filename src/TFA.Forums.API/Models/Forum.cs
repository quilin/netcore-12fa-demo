namespace TFA.Forums.API.Models;

public class Forum
{
    public Guid Id { get; set; }

    public required string Title { get; set; }
}