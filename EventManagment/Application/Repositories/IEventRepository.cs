using Application.DTOs;

namespace Application.Repositories;

public interface IEventRepository
{
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct);
    Task<EventsSearchResult> GetAllAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default);
    Task<CategoriesResult> GetCategoriesAsync(int customerId, bool withCounts, CancellationToken ct = default);
}