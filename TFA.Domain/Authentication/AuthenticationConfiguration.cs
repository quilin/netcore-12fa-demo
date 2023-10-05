namespace TFA.Domain.Authentication;

public class AuthenticationConfiguration
{
    public string Base64Key { get; set; }

    public byte[] Key => Convert.FromBase64String(Base64Key);
}