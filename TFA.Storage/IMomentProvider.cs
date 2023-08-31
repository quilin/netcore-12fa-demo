namespace TFA.Storage;

internal interface IMomentProvider
{
    DateTimeOffset Now { get; }
}

internal class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}