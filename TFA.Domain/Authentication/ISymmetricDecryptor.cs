namespace TFA.Domain.Authentication;

internal interface ISymmetricDecryptor
{
    Task<string> Decrypt(string encryptedText, byte[] key, CancellationToken cancellationToken);
}