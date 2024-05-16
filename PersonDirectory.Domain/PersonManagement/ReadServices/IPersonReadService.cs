using PersonDirectory.Domain.PersonManagement.ReadModels;
using PersonDirectory.Domain.PersonManagement.ReadServices.DTOs;

namespace PersonDirectory.Domain.PersonManagement.ReadServices;

public interface IPersonReadService
{
    Task<PersonReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<PersonReadModel>> SearchPersonsAsync(PersonDetailsSerachDto searchModel, CancellationToken cancellationToken);
}
