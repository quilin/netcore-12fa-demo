using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.UseCases.GetForums;

public interface IGetForumsStorage
{
    Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken);
}