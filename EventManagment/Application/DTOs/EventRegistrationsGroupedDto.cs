using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public sealed class EventRegistrationsGroupedDto
    {
        public int EventId { get; init; }

        public Dictionary<MyStatus, RegistrationStatusGroupDto> Groups { get; init; } = new();
    }

    public sealed class RegistrationStatusGroupDto
    {
        public int TotalCount { get; init; }
        public List<RegistrationUserDto> Users { get; init; } = new();
    }

    public sealed class RegistrationUserDto
    {
        public int Id { get; init; }
        public string Email { get; init; } = null!;
        public string FullName { get; init; } = null!;
        public Department? Department { get; init; }
        public MyStatus Status { get; init; } = MyStatus.NOT_REGISTERED;
        public DateTime CreatedAt { get; init; }
    }

}
