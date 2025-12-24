using Application.Services.Implementations;
using Domain.Models;

namespace Application.Requests.Events;

public sealed record CreateAgendaRequest
{
    public TimeOnly StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public AgendaItemType Type { get; set; }
    public string? Location { get; set; }
    public List<AgendaTrackDTO>? AgendaTracks { get; init; } = new();
}