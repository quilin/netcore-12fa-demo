namespace TFA.Domain.Authorization;

public class IntentionManagerException : Exception
{
    public IntentionManagerException() : base("Action is not allowed")
    {
    }
}