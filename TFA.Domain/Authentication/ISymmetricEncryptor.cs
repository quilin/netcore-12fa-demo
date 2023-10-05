namespace TFA.Domain.Authentication;

internal interface ISymmetricEncryptor
{
    Task<string> Encrypt(string plainText, byte[] key, CancellationToken cancellationToken);
}