namespace Application.Requests.Events;

public sealed record AgendaTrackDTO
{
    public string Title { get; set; } = null!;
    public string? Speaker { get; set; }
    public string? Room { get; set; }
}