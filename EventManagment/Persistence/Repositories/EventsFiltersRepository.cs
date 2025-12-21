using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;

public sealed class EventsFiltersRepository(AppDbContext context) : IEventsFiltersRepository
{
    public async Task<EventFiltersMeta> GetFiltersMetaAsync(int userId, CancellationToken ct)
    {
        var activeEvents = context.Events
            .AsNoTracking()
            .Where(e => e.IsActive);

        // Categories (count active events)
        var eventTypes = await context.EventTypes
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

        // LOCATIONS (count active events)
        var locations = await activeEvents
            .Where(e => e.Location != null && e.Location != "")
            .GroupBy(e => e.Location)
            .Select(g => new LookupCount(
                null,
                g.Key!,
                g.Count()
            ))
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);

        // REGISTRATION STATUSES (count registrations for ACTIVE events)
        var registrationStatuses = await context.RegistrationStatuses
            .AsNoTracking()
            .GroupJoin(
                context.Registrations
                    .AsNoTracking()
                    .Join(
                        activeEvents,
                        r => r.EventId,
                        e => e.Id,
                        (r, _) => r
                    ),
                rs => rs.Id,
                r => r.StatusId,
                (rs, regs) => new LookupCount(rs.Id, rs.Name, regs.Count())
            )
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);

        // Available: remaining > 5
        // Limited : remaining 1..5
        // Full    : remaining <= 0
        var activeEventCapacities = await activeEvents
            .Select(e => new
            {
                e.Id,
                Capacity = (int?)(e.Capacity),
            })
            .ToListAsync(ct);

        var registeredCountsByEvent = await context.Registrations
            .AsNoTracking()
            .Join(activeEvents, r => r.EventId, e => e.Id, (r, _) => r)
            .Join(context.RegistrationStatuses.AsNoTracking(),
                  r => r.StatusId,
                  rs => rs.Id,
                  (r, rs) => new { r.EventId, StatusName = rs.Name })
            .Where(x => x.StatusName != "Waitlisted")
            .GroupBy(x => x.EventId)
            .Select(g => new { EventId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.EventId, x => x.Count, ct);

        int available = 0, limited = 0, full = 0;

        foreach (var e in activeEventCapacities)
        {
            var capacity = e.Capacity ?? 0;
            registeredCountsByEvent.TryGetValue(e.Id, out var taken);
            var remaining = capacity - taken;

            if (remaining > 5) available++;
            else if (remaining >= 1) limited++;
            else full++;
        }

        var capacityAvailability = new List<LookupCount>
    {
        new(null, "Available Spots", available),
        new(null, "Limited (1-5 spots)", limited),
        new(null, "Full (Waitlist)", full),
    };

        // MY STATUS (customer-specific)
        // Registered / Waitlisted / Not Registered (based on ACTIVE events)
        var waitlistStatusId = await context.RegistrationStatuses
            .AsNoTracking()
            .Where(rs => rs.Name == "Waitlisted")
            .Select(rs => (int?)rs.Id)
            .FirstOrDefaultAsync(ct);

        var myRegs = await context.Registrations
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Join(activeEvents, r => r.EventId, e => e.Id, (r, _) => r)
            .Select(r => new { r.EventId, r.StatusId })
            .ToListAsync(ct);

        var myEventIds = myRegs.Select(x => x.EventId).Distinct().ToHashSet();
        var myWaitlisted = waitlistStatusId.HasValue
            ? myRegs.Count(x => x.StatusId == waitlistStatusId.Value)
            : 0;

        var myRegistered = myRegs.Count - myWaitlisted;
        var totalActiveEvents = activeEventCapacities.Count;
        var myNotRegistered = totalActiveEvents - myEventIds.Count;

        var myStatuses = new List<LookupCount>
    {
        new(null, "Registered", myRegistered),
        new(null, "Waitlisted", myWaitlisted),
        new(null, "Not Registered", myNotRegistered),
    };

        return new EventFiltersMeta
        {
            EventTypes = eventTypes,
            Locations = locations,
            RegistrationStatuses = registrationStatuses,
            CapacityAvailability = capacityAvailability,
            MyStatuses = myStatuses
        };
    }




}

