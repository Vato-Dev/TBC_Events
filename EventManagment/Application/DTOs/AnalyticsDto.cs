using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed class AnalyticsDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public AnalyticsKpisDto Kpis { get; set; } = new();
        public IReadOnlyList<RegistrationTrendPointDto> RegistrationTrend { get; set; } = Array.Empty<RegistrationTrendPointDto>();
        public IReadOnlyList<CategoryDistributionDto> CategoryDistribution { get; set; } = Array.Empty<CategoryDistributionDto>();
        public IReadOnlyList<TopEventByDemandDto> TopEventsByDemand { get; set; } = Array.Empty<TopEventByDemandDto>();
        public IReadOnlyList<DepartmentParticipationDto> DepartmentParticipation { get; set; } = Array.Empty<DepartmentParticipationDto>();
    }

    public sealed class AnalyticsKpisDto
    {
        public int TotalRegistrations { get; set; }
        public decimal? TotalRegistrationsChangePct { get; set; }

        public int ActiveParticipants { get; set; }
        public decimal? ActiveParticipantsChangePct { get; set; }

        public decimal CancellationRate { get; set; } // 0..1
    }

    public sealed class RegistrationTrendPointDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Registrations { get; set; }
    }

    public sealed class CategoryDistributionDto
    {
        public string Category { get; set; } = "";
        public int EventsCount { get; set; }
    }

    public sealed class TopEventByDemandDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = "";
        public string Category { get; set; } = "";
        public DateTime Date { get; set; }

        public int Registrations { get; set; }
        public int Capacity { get; set; }
        public decimal Utilization { get; set; } // 0..1
        public int Waitlist { get; set; }
    }

    public sealed class DepartmentParticipationDto
    {
        public Domain.Models.Department Department { get; set; }
        public int TotalEmployees { get; set; }
        public int ActiveParticipants { get; set; }
        public decimal ParticipationRate { get; set; } // 0..1
        public int TotalRegistrations { get; set; }
        public decimal AvgEventsPerEmployee { get; set; }
        public decimal? ParticipationRateChangePct { get; set; }
    }

}
