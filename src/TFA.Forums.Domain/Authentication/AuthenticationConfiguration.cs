namespace TFA.Forums.Domain.Authentication;

public class AuthenticationConfiguration
{
    public required string Base64Key { get; set; }

    public byte[] Key => Convert.FromBase64String(Base64Key);
}