namespace Application.DTOs;

public sealed record UpdateAgendaTrackDTO
{
    public int? Id { get; init; }
    public string Title { get; set; } = null!;
    public string? Speaker { get; set; }
    public string? Room { get; set; }
}