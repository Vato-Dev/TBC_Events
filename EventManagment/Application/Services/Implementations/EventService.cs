using Application.DTOs;
using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Models;
using Mapster;
using Infrastructure.Services;

namespace Application.Services.Implementations;

public class EventService(IEventRepository repository, ICurrentUserService currentUser) : IEventService
{

    public async Task CreateEventAsync(CreateEventRequest request,CancellationToken cancellationToken)
    public async Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct)
    {
        
      var newEvent = request.Adapt<Event>();
      newEvent.IsActive = true;
      newEvent.CreatedAt = DateTime.UtcNow;
      newEvent.UpdatedAt = DateTime.UtcNow;
      newEvent.CreatedById = currentUser.UserId;
      newEvent.EventTypeId = request.EventTypeId;
      await repository.CreateEventAsync(newEvent,request.TagIds,cancellationToken);
        return await _repository.GetFiltersMetaAsync(customerId, ct);
    }

    public async Task CreateAndAddAgendaToEvent(int eventId,CreateAgendaRequest request, CancellationToken cancellationToken)
    {
        var eventToAddAgenda = await repository.GetEventByIdAsync(eventId, cancellationToken);
        var agenda = request.Adapt<AgendaItem>();
        if (request.AgendaTracks is not null && request.AgendaTracks!.Any())
        {
            foreach (var track in request.AgendaTracks)
    public async Task<EventsSearchResult> GetEventsAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default)
            {
                agenda.AddTrackItem(track.Adapt<AgendaTrack>());
            }
        }
        eventToAddAgenda.AddAgendaItem(agenda);
        await repository.UpdateEventAsync(eventToAddAgenda, cancellationToken);
        return await _repository.GetAllAsync(customerId, filters, ct);
    }
    
}

public sealed record CreateAgendaRequest
    public async Task<CategoriesResult> GetCategoriesAsync(int customerId, bool withCounts, CancellationToken ct = default)
{
    public TimeOnly StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public AgendaItemType Type { get; set; }
    public string? Location { get; set; }
    public List<AgendaTrackRequest>? AgendaTracks { get; init; } = new();
        return await _repository.GetCategoriesAsync(customerId, withCounts, ct);
}

public sealed record AgendaTrackRequest
{
    public string Title { get; set; } = null!;
    public string? Speaker { get; set; }
    public string? Room { get; set; }
}
public sealed record CreateEventRequest
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
    public DateOnly RegistrationStart { get; init; }
    public DateOnly RegistrationEnd { get; init; }
    public Location Location { get; init; } = null!;
    public int Capacity { get; init; } 
    public string? ImageUrl { get; init; }
    public List<int> TagIds { get; init; }
    public int EventTypeId { get; init; }
}