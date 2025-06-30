using MediatR;
using TFA.Search.Domain.Models;

namespace TFA.Search.Domain.UseCases.Index;

public record IndexCommand(Guid EntityId, SearchEntityType EntityType, string? Title, string? Text) : IRequest;