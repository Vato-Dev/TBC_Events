using Application.Requests.Events;
using Application.Services.Implementations;

namespace Application.Services.Abstractions;

public interface IEventService
{
    Task<int> CreateEventAsync(CreateEventRequest request, CancellationToken cancellationToken);
    Task<int> CreateAndAddAgendaToEvent(int eventId, CreateAgendaRequest request, CancellationToken cancellationToken);
    Task<int> UpdateEventAsync(UpdateEventRequest request, CancellationToken cancellationToken);
    Task<int> UpdateAgendaItemAsync(UpdateAgendaRequest request, CancellationToken cancellationToken);
}