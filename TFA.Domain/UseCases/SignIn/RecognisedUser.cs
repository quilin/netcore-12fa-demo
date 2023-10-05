namespace TFA.Domain.UseCases.SignIn;

public class RecognisedUser
{
    public Guid UserId { get; set; }
    public byte[] Salt { get; set; }
    public byte[] PasswordHash { get; set; }
}