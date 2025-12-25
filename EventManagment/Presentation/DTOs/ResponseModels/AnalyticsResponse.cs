using Application.DTOs;
namespace Presentation.DTOs.ResponseModels
{
    public sealed class AnalyticsResponse
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public AnalyticsKpisDto Kpis { get; set; } = new();
        public IReadOnlyList<RegistrationTrendPointDto> RegistrationTrend { get; set; } = Array.Empty<RegistrationTrendPointDto>();
        public IReadOnlyList<CategoryDistributionDto> CategoryDistribution { get; set; } = Array.Empty<CategoryDistributionDto>();
        public IReadOnlyList<TopEventByDemandDto> TopEventsByDemand { get; set; } = Array.Empty<TopEventByDemandDto>();
        public IReadOnlyList<DepartmentParticipationDto> DepartmentParticipation { get; set; } = Array.Empty<DepartmentParticipationDto>();
    }
}
