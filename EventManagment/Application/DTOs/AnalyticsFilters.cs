using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed class AnalyticsFilters
    {
        public IReadOnlyList<int>? EventTypeIds { get; init; }
        public IReadOnlyList<string>? Locations { get; init; }
        public DateTime? From { get; init; }
        public DateTime? To { get; init; }
        public string? Granularity { get; init; } // "week" | "month" | "year"
    }

}
