using MediatR;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

internal class GetForumsUseCase : IRequestHandler<GetForumsQuery, IEnumerable<Forum>>
{
    private readonly IGetForumsStorage storage;

    public GetForumsUseCase(
        IGetForumsStorage storage)
    {
        this.storage = storage;
    }

    public Task<IEnumerable<Forum>> Handle(GetForumsQuery query, CancellationToken cancellationToken) =>
        storage.GetForums(cancellationToken);
}