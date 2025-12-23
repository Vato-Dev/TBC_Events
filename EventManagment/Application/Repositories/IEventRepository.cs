using Application.DTOs;

using Domain.Models;

namespace Application.Repositories;

public interface IEventRepository
{
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct);
    Task<EventsSearchResult> GetAllAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default);
    Task<CategoriesResult> GetCategoriesAsync(int customerId, bool withCounts, CancellationToken ct = default);
    public Task<int> CreateEventAsync(Event @event,List<int> tagIds, CancellationToken cancellationToken);
    public Task<Event> GetEventByIdAsync(int id, CancellationToken cancellationToken);
    public Task UpdateEventAsync(Event @event, CancellationToken cancellationToken);
}