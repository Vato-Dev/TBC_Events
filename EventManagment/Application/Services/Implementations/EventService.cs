

using Application.Exceptions;
using Application.DTOs;
using Application.Repositories;
using Application.Requests.Events;
using Application.Services.Abstractions;
using Domain.Models;
using Mapster;
using System.Text;

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

    public async Task<byte[]> GetEventsAsCsvAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default)
    {
        var result = await repository.GetAllAsync(customerId, filters, ct);
        var sb = new StringBuilder();

        // CSV header (NO MyStatus)
        sb.AppendLine(
            "Title,StartDateTime,EndDateTime,Location,Capacity,RegisteredUsers,Description"
        );

        foreach (var ev in result.Items)
        {
            sb.AppendLine(
                $"\"{ev.Title}\"," +
                $"{ev.StartsAt:O}," +
                $"{ev.EndsAt:O}," +
                $"\"{ev.Location}\"," +
                $"{ev.Capacity}," +
                $"{ev.TotalRegistered}," +
                $"{ev.Description}"
            );
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

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
    
    public async Task DeleteEventAsync(int eventId, CancellationToken cancellationToken)
    {
        await repository.DeleteEventAsync(eventId, cancellationToken);
    }

    public async Task<Event> GetEventByIdAsyncc(int eventId, CancellationToken cancellationToken)
    {
        var @event =  await repository.GetEventByIdAsync(eventId, cancellationToken);
        if(@event is null) throw new NotFoundException();
        return @event;
    }

    public async Task<EventRegistrationsGroupedDto> GetEventRegistrationsGroupedAsync(int eventId, CancellationToken ct = default)
    {
        return await repository.GetEventRegistrationsGroupedAsync(eventId, ct);
    }
    public Task ConfirmWaitlistedAsync(int eventId, int userId, CancellationToken ct = default)
        => repository.ConfirmWaitlistedAsync(eventId, userId, ct);

    public Task RejectWaitlistedAsync(int eventId, int userId, CancellationToken ct = default)
        => repository.RejectWaitlistedAsync(eventId, userId, ct);

}

