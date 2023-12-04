namespace TFA.Domain.Authentication;

public class Session
{
    public Guid UserId { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}