using MediatR;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.UseCases.GetForums;

internal class GetForumsUseCase(
    IGetForumsStorage storage) : IRequestHandler<GetForumsQuery, IEnumerable<Forum>>
{
    public Task<IEnumerable<Forum>> Handle(GetForumsQuery query, CancellationToken cancellationToken) => 
        storage.GetForums(cancellationToken);
}