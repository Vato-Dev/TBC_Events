using Application.DTOs;

namespace Presentation.DTOs.ResponseModels
{
    public sealed class EventsSearchResponse
    {
        public IReadOnlyList<EventCardDto> Items { get; init; } = Array.Empty<EventCardDto>();
        public int? Page { get; init; }
        public int? PageSize { get; init; }
        public int TotalCount { get; init; }
    }

    public sealed class EventCardDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = "";
        public string Description { get; init; }
        public DateTime StartsAt { get; init; }
        public DateTime EndsAt { get; init; }
        public string? ImageUrl { get; init; }
        public string? Location { get; init; }
        public int EventTypeId { get; init; }
        public string EventTypeName { get; init; } = "";
        public MyStatus? MyStatus { get; init; }
        public int? TotalRegistered { get; init; }
        public int? SpotsLeft { get; init; }                 // e.g. 33 (null if unlimited/unknown)
        public int Capacity { get; init; }
        public CapacityAvailability? CapacityAvailability { get; init; }
    }
}
