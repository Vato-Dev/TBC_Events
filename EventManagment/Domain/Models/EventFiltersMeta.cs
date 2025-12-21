using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public sealed class EventFiltersMeta
{
    public IReadOnlyList<LookupCount> EventTypes { get; init; } = Array.Empty<LookupCount>();
    public IReadOnlyList<LookupCount> RegistrationStatuses { get; init; } = Array.Empty<LookupCount>();
    public IReadOnlyList<LookupCount> Locations { get; init; } = Array.Empty<LookupCount>();
    public IReadOnlyList<LookupCount> CapacityAvailability { get; init; } = Array.Empty<LookupCount>();
    public IReadOnlyList<LookupCount> MyStatuses { get; init; } = Array.Empty<LookupCount>();


}

public sealed record LookupCount(int? Id, string Name, int Count);
