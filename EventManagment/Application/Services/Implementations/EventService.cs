

using Application.Exceptions;
using Application.Repositories;
using Application.Requests.Events;
using Application.Services.Abstractions;
using Domain.Models;
using Mapster;

namespace Application.Services.Implementations;

public class EventService(IEventRepository repository , ICurrentUserService currentUser) : IEventService
{
    public Task<int> CreateEventAsync(CreateEventRequest request, CancellationToken cancellationToken)
    {
      var newEvent = request.Adapt<Event>();
      newEvent.IsActive = true;
      newEvent.CreatedAt = DateTime.UtcNow;
      newEvent.UpdatedAt = DateTime.UtcNow;
      newEvent.CreatedById = currentUser.UserId;
     return repository.CreateEventAsync(newEvent,request.TagIds,cancellationToken); //to aptimize it
    }
    public async Task<int> CreateAndAddAgendaToEvent(int eventId,CreateAgendaRequest request, CancellationToken cancellationToken)
    {
        var @event = await repository.GetEventByIdAsync(eventId, cancellationToken);
        if (@event == null) throw new NotFoundException();

        var agenda = request.Adapt<AgendaItem>();

        @event.AddAgendaItem(agenda);

       var result =  await repository.AddEventAgendaAsync(@event.Id, agenda, cancellationToken);
       if(result is null) throw new NotFoundException();
       return result.Value;
    }

    public async Task<int> UpdateEventAsync(UpdateEventRequest request, CancellationToken cancellationToken)
    {
       var result =  await repository.UpdateEventAsync(request, cancellationToken);
       if(result is null) throw new Exception("Event not found");
       return result.Value;
    }
    
    public async Task<int> UpdateAgendaItemAsync(UpdateAgendaRequest request, CancellationToken cancellationToken)
    { 
        
        var result =  await repository.UpdateAgendaItemAsync(request, cancellationToken);
         if(result is null)
             throw new NotFoundException();
         return result.Value;
    }
}



