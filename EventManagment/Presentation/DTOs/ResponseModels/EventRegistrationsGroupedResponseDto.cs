using Application.DTOs;

namespace Presentation.DTOs.ResponseModels
{
    public class EventRegistrationsGroupedResponseDto
    {
        public int EventId { get; init; }
        public Dictionary<MyStatus, RegistrationStatusGroupDto> Groups { get; init; } = new();
    }
}