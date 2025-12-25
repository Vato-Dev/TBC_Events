using Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs.RequestModels
{
    public sealed class EventsSearchRequestDto : IValidatableObject
    {
        [StringLength(200, MinimumLength = 1)]
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
        [Range(1, int.MaxValue)]
        public int? Page { get; init; }
        [Range(1, 200)]
        public int? PageSize { get; init; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.HasValue && To.HasValue && From.Value.Date > To.Value.Date)
            {
                yield return new ValidationResult(
                    "'From' must be earlier than or equal to 'To'.",
                    new[] { nameof(From), nameof(To) }
                );
            }
        }
    }
}
