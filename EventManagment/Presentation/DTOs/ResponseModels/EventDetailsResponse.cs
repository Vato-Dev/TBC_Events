using Application.DTOs;

namespace Presentation.DTOs.ResponseModels
{
    public sealed class EventDetailsResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public string? ImageUrl { get; init; }

        public DateTime StartDateTime { get; init; }
        public DateTime EndDateTime { get; init; }

        public DateOnly RegistrationStart { get; init; }
        public DateOnly RegistrationEnd { get; init; }

        public int Capacity { get; init; }
        public int RegisteredUsers { get; init; }
        public bool IsActive { get; init; }

        public EventTypeDto EventType { get; init; } = null!;
        public OrganizerDto Organizer { get; init; } = null!;
        public LocationDto Location { get; init; } = null!;

        public IReadOnlyList<TagDto> Tags { get; init; } = Array.Empty<TagDto>();
        public IReadOnlyList<AgendaItemDto> Agenda { get; init; } = Array.Empty<AgendaItemDto>();
    }
}
