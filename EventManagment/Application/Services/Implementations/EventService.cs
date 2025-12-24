using Application.DTOs;
using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Models;
using Mapster;

namespace Application.Services.Implementations;

public sealed class EventService(IEventRepository repository, ICurrentUserService currentUser) : IEventService
{

    public async Task CreateEventAsync(CreateEventRequest request, CancellationToken ct)
    {
        var newEvent = request.Adapt<Event>();

        newEvent.IsActive = true;
        newEvent.CreatedAt = DateTime.UtcNow;
        newEvent.UpdatedAt = DateTime.UtcNow;
        newEvent.CreatedById = currentUser.UserId;
        newEvent.EventTypeId = request.EventTypeId;

        await repository.CreateEventAsync(newEvent, request.TagIds, ct);
    }

    public Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct)
        => repository.GetFiltersMetaAsync(customerId, ct);

    public Task<EventsSearchResult> GetEventsAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default)
        => repository.GetAllAsync(customerId, filters, ct);

    public Task<CategoriesResult> GetCategoriesAsync(int customerId, bool withCounts, CancellationToken ct = default)
        => repository.GetCategoriesAsync(customerId, withCounts, ct);

    public async Task CreateAndAddAgendaToEvent(int eventId, CreateAgendaRequest request, CancellationToken ct)
    {
        var eventToAddAgenda = await repository.GetEventByIdAsync(eventId, ct);

        var agenda = request.Adapt<AgendaItem>();

        if (request.AgendaTracks is { Count: > 0 })
        {
            foreach (var track in request.AgendaTracks)
            {
                agenda.AddTrackItem(track.Adapt<AgendaTrack>());
            }
        }

        eventToAddAgenda.AddAgendaItem(agenda);

        await repository.UpdateEventAsync(eventToAddAgenda, ct);
    }
}

public sealed record CreateAgendaRequest
{
    public TimeOnly StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public AgendaItemType Type { get; set; }
    public string? Location { get; set; }
    public List<AgendaTrackRequest>? AgendaTracks { get; init; } = new();
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
    public List<int> TagIds { get; init; } = new();
    public int EventTypeId { get; init; }
}
