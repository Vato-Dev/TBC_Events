using Application.DTOs;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;

namespace Persistence.Repositories;
public class EventRepository(AppDbContext context) : IEventRepository
{
    public async Task<EventFiltersMeta> GetFiltersMetaAsync(int userId, CancellationToken ct)
    {
        IQueryable<EventEntity> activeEvents = context.Events
            .AsNoTracking()
            .Where(e => e.IsActive);

        var eventTypes = await GetEventTypesAsync(activeEvents, ct);
        var locations = await GetLocationsAsync(activeEvents, ct);
        var registrationStatuses = await GetRegistrationStatusesAsync(activeEvents, ct);
        var capacityAvailability = await GetCapacityAvailabilityAsync(activeEvents, ct);
        var myStatuses = await GetMyStatusesAsync(activeEvents, userId, ct);

        return new EventFiltersMeta
        {
            EventTypes = eventTypes,
            Locations = locations,
            RegistrationStatuses = registrationStatuses,
            CapacityAvailability = capacityAvailability,
            MyStatuses = myStatuses
        };
    }

    private Task<List<LookupCount>> GetEventTypesAsync(IQueryable<EventEntity> activeEvents, CancellationToken ct)
    {
        return context.EventTypes
            .AsNoTracking()
            .Where(et => et.IsActive)
            .GroupJoin(
                activeEvents,
                et => et.Id,
                e => e.EventTypeId,
                (et, events) => new LookupCount(et.Id, et.Name, events.Count())
            )
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    private Task<List<LookupCount>> GetLocationsAsync(IQueryable<EventEntity> activeEvents, CancellationToken ct)
    {
        return activeEvents
            .Where(e => e.Location != null && e.Location != "")
            .GroupBy(e => e.Location)
            .Select(g => new LookupCount(null, g.Key!, g.Count()))
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    private Task<List<LookupCount>> GetRegistrationStatusesAsync(IQueryable<EventEntity> activeEvents, CancellationToken ct)
    {
        var regsOnActiveEvents =
            context.Registrations
                .AsNoTracking()
                .Join(activeEvents, r => r.EventId, e => e.Id, (r, _) => r);

        return context.RegistrationStatuses
            .AsNoTracking()
            .GroupJoin(
                regsOnActiveEvents,
                rs => rs.Id,
                r => r.StatusId,
                (rs, regs) => new LookupCount(rs.Id, rs.Name, regs.Count())
            )
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    private async Task<List<LookupCount>> GetCapacityAvailabilityAsync(IQueryable<EventEntity> activeEvents, CancellationToken ct)
    {
        var capacities = await activeEvents
            .Select(e => new { e.Id, Capacity = (int?)e.Capacity })
            .ToListAsync(ct);

        var takenByEvent = await context.Registrations
            .AsNoTracking()
            .Join(activeEvents, r => r.EventId, e => e.Id, (r, _) => r)
            .Join(context.RegistrationStatuses.AsNoTracking(),
                r => r.StatusId,
                rs => rs.Id,
                (r, rs) => new { r.EventId, StatusName = rs.Name }
            )
            .Where(x => x.StatusName != "Waitlisted")
            .GroupBy(x => x.EventId)
            .Select(g => new { EventId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.EventId, x => x.Count, ct);

        int available = 0, limited = 0, full = 0;

        foreach (var e in capacities)
        {
            var capacity = e.Capacity ?? 0;
            takenByEvent.TryGetValue(e.Id, out var taken);
            var remaining = capacity - taken;

            if (remaining > 5) available++;
            else if (remaining >= 1) limited++;
            else full++;
        }

        return new List<LookupCount>
        {
            new(null, "Available Spots", available),
            new(null, "Limited (1-5 spots)", limited),
            new(null, "Full (Waitlist)", full),
        };
    }

    private async Task<List<LookupCount>> GetMyStatusesAsync(
        IQueryable<EventEntity> activeEvents,
        int userId,
        CancellationToken ct)
    {
        var waitlistedId = await context.RegistrationStatuses
            .AsNoTracking()
            .Where(rs => rs.Name == "Waitlisted")
            .Select(rs => (int?)rs.Id)
            .FirstOrDefaultAsync(ct);

        // registrations by this user on active events only
        var myRegs = await context.Registrations
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Join(activeEvents, r => r.EventId, e => e.Id, (r, _) => new { r.EventId, r.StatusId })
            .ToListAsync(ct);

        var myEventIds = myRegs.Select(x => x.EventId).Distinct().ToHashSet();
        var myWaitlisted = waitlistedId.HasValue ? myRegs.Count(x => x.StatusId == waitlistedId.Value) : 0;
        var myRegistered = myRegs.Count - myWaitlisted;

        var totalActiveEvents = await activeEvents.CountAsync(ct);
        var myNotRegistered = totalActiveEvents - myEventIds.Count;

        return new List<LookupCount>
        {
            new(null, "Registered", myRegistered),
            new(null, "Waitlisted", myWaitlisted),
            new(null, "Not Registered", myNotRegistered),
        };
    }
}
