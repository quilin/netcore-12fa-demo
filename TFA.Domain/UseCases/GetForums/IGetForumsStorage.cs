using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetForums;

public interface IGetForumsStorage
{
    Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken);
}