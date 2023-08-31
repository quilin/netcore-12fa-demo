using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

internal class GetForumsUseCase : IGetForumsUseCase
{
    private readonly IGetForumsStorage storage;

    public GetForumsUseCase(
        IGetForumsStorage storage)
    {
        this.storage = storage;
    }

    public Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken) =>
        storage.GetForums(cancellationToken);
}