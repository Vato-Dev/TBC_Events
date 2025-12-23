using Application.DTOs;

namespace Infrastructure.Services;

public interface IEventService
{
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct = default);
    Task<EventsSearchResult> GetEventsAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default);
    Task<CategoriesResult> GetCategoriesAsync(int userId, bool withCounts, CancellationToken ct);

}
