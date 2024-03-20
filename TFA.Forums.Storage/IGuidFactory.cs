namespace TFA.Forums.Storage;

internal interface IGuidFactory
{
    Guid Create();
}

internal class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}