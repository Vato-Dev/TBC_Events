using Application.DTOs;

namespace Infrastructure.Services;

public interface IEventService
{
    /// <summary>
    /// Returns available filters for Events page (event types, tags, locations, registration statuses)
    /// with counts for each filter item.
    /// </summary>
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct = default);
}
