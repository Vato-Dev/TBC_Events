using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed class EventsSearchResult
    {
        public IReadOnlyList<EventCard> Items { get; init; } = Array.Empty<EventCard>();
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
    }

    public sealed class EventCard
    {
        public int Id { get; init; }
        public string Title { get; init; } = "";
        public string Description { get; init; }
        public DateTime StartsAt { get; init; }
        public DateTime EndsAt { get; init; }
        public string? ImageUrl { get; init; }
        public string Location { get; init; }
        public int EventTypeId { get; init; }
        public string EventTypeName { get; init; } = "";
        public MyStatus? MyStatus { get; init; }
        public int? TotalRegistered { get; init; }
        public int? SpotsLeft { get; init; }
        public int Capacity { get; init; }
        public CapacityAvailability? CapacityAvailability { get; init; }
    }

}
