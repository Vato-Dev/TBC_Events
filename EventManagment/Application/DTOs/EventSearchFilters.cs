using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{

    public sealed class EventsSearchFilters
    {
        public string? Search { get; init; }

        public IReadOnlyList<int>? EventTypeIds { get; init; }
        public IReadOnlyList<string>? Locations { get; init; }

        public DateTime? From { get; init; }
        public DateTime? To { get; init; }

        public IReadOnlyList<CapacityAvailability>? CapacityAvailability { get; init; }
        public IReadOnlyList<MyStatus>? MyStatuses { get; init; }
        public EventsSortBy? SortBy { get; init; }
        public EventSortDirection? SortDirection { get; init; }
        public bool? IsActive { get; init; }

        public int? Page { get; init; }
        public int? PageSize { get; init; }
    }
    public enum EventsSortBy
    {
        START_DATE = 0,
        TITLE = 1,
        REGISTRATIONS = 2,
    }

    public enum EventSortDirection
    {
        ASC = 0,
        DESC = 1
    }

    public enum CapacityAvailability
    {
        AVAILABLE = 0, // remaining > 5
        LIMITED = 1,   // 1..5
        FULL = 2       // <= 0
    }

    public enum MyStatus
    {
        CONFIRMED = 0,
        WAITLISTED = 1,
        CANCELLED = 2,
        NOT_REGISTERED = 3
    }

}
