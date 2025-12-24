using Domain.Models;

namespace Application.Requests.Events;

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