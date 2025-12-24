using Application.Requests.Events;
using Application.Services.Implementations;
using Domain.Models;

namespace Application.Repositories;

public interface IEventRepository
{
    public Task<int> CreateEventAsync(Event @event,List<int> tagIds, CancellationToken cancellationToken);
    public Task<Event> GetEventByIdAsync(int id, CancellationToken cancellationToken);
    public Task<int?> AddEventAgendaAsync(int eventId, AgendaItem agenda, CancellationToken ct);
    Task<int?> UpdateEventAsync(UpdateEventRequest request, CancellationToken cancellationToken);
    Task<int?> UpdateAgendaItemAsync(UpdateAgendaRequest request, CancellationToken cancellationToken);
}