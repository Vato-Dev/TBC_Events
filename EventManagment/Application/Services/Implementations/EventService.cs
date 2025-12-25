

using Application.Exceptions;
using Application.DTOs;
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

    public Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct)
        => repository.GetFiltersMetaAsync(customerId, ct);

    public Task<EventsSearchResult> GetEventsAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default)
        => repository.GetAllAsync(customerId, filters, ct);

    public Task<CategoriesResult> GetCategoriesAsync(int customerId, bool withCounts, CancellationToken ct = default)
        => repository.GetCategoriesAsync(customerId, withCounts, ct);

    public Task<EventDetails?> GetEventDetailsAsync(int customerId, int eventId, CancellationToken ct) 
        => repository.GetEventDetailsAsync(customerId, eventId, ct);
    public Task RegisterOnEventAsync(int customerId, int eventId, CancellationToken ct = default)
    => repository.RegisterOnEventAsync(customerId, eventId, ct);

    public Task UnregisterFromEventAsync(int customerId, int eventId, CancellationToken ct = default)
        => repository.UnregisterFromEventAsync(customerId, eventId, ct);

    public async Task<int> CreateAndAddAgendaToEvent(int eventId, CreateAgendaRequest request, CancellationToken ct)
    {
        var @event = await repository.GetEventByIdAsync(eventId, ct);
        if (@event == null) throw new NotFoundException();

        var agenda = request.Adapt<AgendaItem>();

        @event.AddAgendaItem(agenda);

       var result =  await repository.AddEventAgendaAsync(@event.Id, agenda, ct);
       if(result is null) throw new NotFoundException();
       return result.Value;
    }

    public async Task<int> UpdateEventAsync(UpdateEventRequest request, CancellationToken ct)
    {
       var result =  await repository.UpdateEventAsync(request, ct);
       if(result is null) throw new Exception("Event not found");
       return result.Value;
    }
    
    public async Task<int> UpdateAgendaItemAsync(UpdateAgendaRequest request, CancellationToken ct)
    { 
        
        var result =  await repository.UpdateAgendaItemAsync(request, ct);
         if(result is null)
             throw new NotFoundException();
         return result.Value;
    }
}

