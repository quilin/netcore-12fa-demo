using MediatR;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

internal class GetForumsUseCase(
    IGetForumsStorage storage) : IRequestHandler<GetForumsQuery, IEnumerable<Forum>>
{
    public Task<IEnumerable<Forum>> Handle(GetForumsQuery query, CancellationToken cancellationToken) => 
        storage.GetForums(cancellationToken);
}