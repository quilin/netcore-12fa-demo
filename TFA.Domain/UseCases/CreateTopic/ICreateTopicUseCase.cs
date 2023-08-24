using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> Execute(CreateTopicCommand command, CancellationToken cancellationToken);
}