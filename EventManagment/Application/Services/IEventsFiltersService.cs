using Domain.Models;

namespace Infrastructure.Services;

public interface IEventsFiltersService
{
    /// <summary>
    /// Returns available filters for Events page (event types, tags, locations, registration statuses)
    /// with counts for each filter item.
    /// </summary>
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct = default);
}
