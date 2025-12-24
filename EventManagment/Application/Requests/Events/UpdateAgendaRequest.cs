using Application.DTOs;

namespace Application.Requests.Events;

public sealed record UpdateAgendaRequest
{
    public int AgendaId { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeSpan Duration { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public string? Location { get; init; }
    public List<UpdateAgendaTrackDTO> Tracks { get; init; } = new();
}