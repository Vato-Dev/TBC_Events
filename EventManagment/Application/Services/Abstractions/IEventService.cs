using Application.DTOs;

namespace Application.Services.Abstractions;

public interface IEventService
{
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct = default);
    Task<EventsSearchResult> GetEventsAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default);
    Task<CategoriesResult> GetCategoriesAsync(int userId, bool withCounts, CancellationToken ct);
    Task<EventDetails?> GetEventDetailsAsync(int customerId, int eventId, CancellationToken ct);
    Task RegisterOnEventAsync(int customerId, int eventId, CancellationToken ct = default);
    Task UnregisterFromEventAsync(int customerId, int eventId, CancellationToken ct = default);

}
