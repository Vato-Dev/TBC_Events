using Application.Requests.Events;
using Application.DTOs;
using Domain.Models;

namespace Application.Services.Abstractions;

public interface IEventService
{
    Task<int> CreateEventAsync(CreateEventRequest request, CancellationToken cancellationToken);
    Task<int> CreateAndAddAgendaToEvent(int eventId, CreateAgendaRequest request, CancellationToken cancellationToken);
    Task<int> UpdateEventAsync(UpdateEventRequest request, CancellationToken cancellationToken);
    Task<int> UpdateAgendaItemAsync(UpdateAgendaRequest request, CancellationToken cancellationToken);
    Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct = default);
    Task<EventsSearchResult> GetEventsAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default);
    Task<CategoriesResult> GetCategoriesAsync(int userId, bool withCounts, CancellationToken ct);
    Task<EventDetails?> GetEventDetailsAsync(int customerId, int eventId, CancellationToken ct);
    Task RegisterOnEventAsync(int customerId, int eventId, CancellationToken ct = default);
    Task UnregisterFromEventAsync(int customerId, int eventId, CancellationToken ct = default);
    Task<EventRegistrationsGroupedDto> GetEventRegistrationsGroupedAsync(int eventId, CancellationToken ct = default);
    Task ConfirmWaitlistedAsync(int eventId, int userId, CancellationToken ct = default);
    Task RejectWaitlistedAsync(int eventId, int userId, CancellationToken ct = default);

    Task DeleteEventAsync(int eventId, CancellationToken cancellationToken);
    Task<Event> GetEventByIdAsyncc(int eventId, CancellationToken cancellationToken);

}
