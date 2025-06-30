namespace TFA.Forums.Domain.UseCases.SignIn;

public class RecognisedUser
{
    public Guid UserId { get; set; }
    public required byte[] Salt { get; set; }
    public required byte[] PasswordHash { get; set; }
}