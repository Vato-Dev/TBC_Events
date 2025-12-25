using Domain.Models;

namespace Application.Repositories;

public interface IEventRepository
{
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct);
    Task<EventsSearchResult> GetAllAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default);
    Task<CategoriesResult> GetCategoriesAsync(int customerId, bool withCounts, CancellationToken ct = default);
    Task<EventDetails?> GetEventDetailsAsync(int userId, int eventId, CancellationToken cancellationToken);
    Task RegisterOnEventAsync(int userId, int eventId, CancellationToken ct);
    Task UnregisterFromEventAsync(int userId, int eventId, CancellationToken ct);
    public Task<int> CreateEventAsync(Event @event,List<int> tagIds, CancellationToken cancellationToken);
    public Task<Event> GetEventByIdAsync(int id, CancellationToken cancellationToken);
    public Task<int?> AddEventAgendaAsync(int eventId, AgendaItem agenda, CancellationToken ct);
    Task<int?> UpdateEventAsync(UpdateEventRequest request, CancellationToken cancellationToken);
    Task<int?> UpdateAgendaItemAsync(UpdateAgendaRequest request, CancellationToken cancellationToken);
    Task DeleteEventAsync(int eventId, CancellationToken cancellationToken);
}