using Domain.Models;

namespace Presentation.DTOs.ResponseModels
{
    public sealed class EventFiltersMetaResponseDto
    {
        public IReadOnlyList<LookupDto> EventTypes { get; init; } = Array.Empty<LookupDto>();
        public IReadOnlyList<LookupDto> RegistrationStatuses { get; init; } = Array.Empty<LookupDto>();
        public IReadOnlyList<LookupDto> Locations { get; init; } = Array.Empty<LookupDto>();
        public IReadOnlyList<LookupDto> CapacityAvailability { get; init; } = Array.Empty<LookupDto>();
        public IReadOnlyList<LookupDto> MyStatuses { get; init; } = Array.Empty<LookupDto>();


    }

    public sealed record LookupDto(int? Id, string Name, int Count);

}
