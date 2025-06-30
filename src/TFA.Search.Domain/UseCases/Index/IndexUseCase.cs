using MediatR;

namespace TFA.Search.Domain.UseCases.Index;

internal class IndexUseCase(IIndexStorage storage) : IRequestHandler<IndexCommand>
{
    public Task Handle(IndexCommand request, CancellationToken cancellationToken)
    {
        var (entityId, searchEntityType, title, text) = request;
        return storage.Index(entityId, searchEntityType, title, text, cancellationToken);
    }
}