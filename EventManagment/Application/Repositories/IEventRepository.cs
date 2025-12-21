using Application.DTOs;

namespace Application.Repositories;

public interface IEventRepository
{
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct);
}