using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EventDetails
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public string? ImageUrl { get; init; }

        public DateTime StartDateTime { get; init; }
        public DateTime EndDateTime { get; init; }

        public DateTime RegistrationStart { get; init; }
        public DateTime RegistrationEnd { get; init; }

        public int Capacity { get; init; }
        public int RegisteredUsers { get; init; }
        public int CurrentWaitlist { get; init; }
        public bool IsActive { get; init; }

        public EventTypeDto EventType { get; init; } = null!;
        public OrganizerDto Organizer { get; init; } = null!;
        public LocationDto Location { get; init; } = null!;

        public IReadOnlyList<TagDto> Tags { get; init; } = Array.Empty<TagDto>();
        public IReadOnlyList<AgendaItemDto> Agenda { get; init; } = Array.Empty<AgendaItemDto>();
    }


    public sealed class EventTypeDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
    }

    public sealed class OrganizerDto
    {
        public int Id { get; init; }
        public string FullName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? Department { get; init; } // optional if you want to display it
    }

    public sealed class TagDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Category { get; init; }
    }

    public sealed class LocationDto
    {
        public string LocationType { get; init; } = null!;
        public string VenueName { get; init; } = null!;
        public string Street { get; init; } = null!;
        public string City { get; init; } = null!;
        public int RoomNumber { get; init; }
        public int FloorNumber { get; init; }
        public string? AdditionalInformation { get; init; }
    }

    public sealed class AgendaItemDto
    {
        public int Id { get; init; }
        public TimeOnly StartTime { get; init; }
        public TimeSpan Duration { get; init; }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public string Type { get; init; } = null!;
        public string? Location { get; init; }

        public IReadOnlyList<AgendaTrackDto> Tracks { get; init; } = Array.Empty<AgendaTrackDto>();
    }

    public sealed class AgendaTrackDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public string? Speaker { get; init; }
        public string? Room { get; init; }
    }
}
