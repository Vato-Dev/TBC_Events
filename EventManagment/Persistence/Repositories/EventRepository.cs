using Application.DTOs;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Mapster;
using Persistence.Data;
using Persistence.Entities;
using Persistence.Mappings;

namespace Persistence.Repositories;
public class EventRepository(AppDbContext context) : IEventRepository
{
    public async Task<CategoriesResult> GetCategoriesAsync(
        int customerId,
        bool withCounts,
        CancellationToken ct = default)
    {
        if (!withCounts)
        {
            var categories = await context.EventTypes
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Count = null
                })
                .ToListAsync(ct);

            return new CategoriesResult
            {
                Categories = categories
            };
        }

        var categoriesWithCounts = await context.EventTypes
            .AsNoTracking()
            .Where(c => c.IsActive)
            .GroupJoin(
                context.Events.AsNoTracking().Where(e => e.IsActive),
                c => c.Id,
                e => e.EventTypeId,
                (c, events) => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Count = events.Count()
                }
            )
            .OrderByDescending(c => c.Count)
            .ThenBy(c => c.Name)
            .ToListAsync(ct);

        return new CategoriesResult
        {
            Categories = categoriesWithCounts
        };
    }


    public async Task<EventsSearchResult> GetAllAsync(
            int customerId,
            EventsSearchFilters filters,
            CancellationToken ct = default)
    {
        // Defaults
        var page = filters.Page.GetValueOrDefault(1);
        if (page < 1) page = 1;

        var pageSize = filters.PageSize.GetValueOrDefault(20);
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 200) pageSize = 200;

        // Base query
        IQueryable<EventEntity> query = context.Set<EventEntity>()
            .AsNoTracking()
            .Include(e => e.EventTypeEntity)
            .Include(e => e.Registrations)
                .ThenInclude(r => r.StatusEntity);

        // IsActive filter
        if (filters.IsActive.HasValue)
        {
            query = query.Where(e => e.IsActive == filters.IsActive.Value);
        }

        // Search filter (Title + Description)
        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var s = filters.Search.Trim();
            query = query.Where(e =>
                EF.Functions.Like(e.Title, $"%{s}%") ||
                (e.Description != null && EF.Functions.Like(e.Description, $"%{s}%")));
        }

        // EventTypeIds
        if (filters.EventTypeIds is { Count: > 0 })
        {
            query = query.Where(e => filters.EventTypeIds!.Contains(e.EventTypeId));
        }

        // Locations
        if (filters.Locations is { Count: > 0 })
        {
            query = query.Where(e => filters.Locations!.Contains(e.Location));
        }

        if (filters.From.HasValue)
        {
            var fromDate = filters.From.Value;
            query = query.Where(e => e.StartDateTime.Date >= fromDate);
        }

        if (filters.To.HasValue)
        {
            var toDate = filters.To.Value;
            query = query.Where(e => e.StartDateTime.Date <= toDate);
        }

        // CapacityAvailability (computed by remaining spots)
        if (filters.CapacityAvailability is { Count: > 0 })
        {
            query = query.Where(e =>
                filters.CapacityAvailability!.Contains(
                    EventStatusMapper.MapCapacityAvailability(
                        e.Capacity -
                        e.Registrations.Count(r =>
                            r.CancelledAt == null &&
                            r.StatusEntity.Name == "Confirmed")
                    )
                )
            );
        }

        // MyStatuses (computed for this customerId)
        if (filters.MyStatuses is { Count: > 0 })
        {
            query = query.Where(e =>
                filters.MyStatuses!.Contains(
                    EventStatusMapper.MapMyStatus(
                        e.Registrations
                            .Where(r => r.UserId == customerId)
                            .OrderByDescending(r => r.RegisteredAt ?? DateTime.MinValue)
                            .Select(r => r.StatusEntity.Name)
                            .FirstOrDefault()
                    )
                )
            );
        }

        var totalCount = await query.CountAsync(ct);

        // Sorting
        var sortBy = filters.SortBy ?? EventsSortBy.START_DATE;
        var sortDir = filters.SortDirection ?? EventSortDirection.ASC;

        query = (sortBy, sortDir) switch
        {
            (EventsSortBy.TITLE, EventSortDirection.ASC) => query.OrderBy(e => e.Title),
            (EventsSortBy.TITLE, EventSortDirection.DESC) => query.OrderByDescending(e => e.Title),

            (EventsSortBy.REGISTRATIONS, EventSortDirection.ASC) =>
                query.OrderBy(e =>
                    e.Registrations.Count(r =>
                        r.CancelledAt == null &&
                        r.StatusEntity.Name == "Confirmed")),

            (EventsSortBy.REGISTRATIONS, EventSortDirection.DESC) =>
                query.OrderByDescending(e =>
                    e.Registrations.Count(r =>
                        r.CancelledAt == null &&
                        r.StatusEntity.Name == "Confirmed")),

            (EventsSortBy.START_DATE, EventSortDirection.DESC) => query.OrderByDescending(e => e.StartDateTime),
            _ => query.OrderBy(e => e.StartDateTime),
        };

        // Page slice
        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        // Projection to DTO
        var items = await query
            .Select(e => new EventCard
            {
                Id = e.Id,
                Title = e.Title,
                StartsAt = e.StartDateTime,
                EndsAt = e.EndDateTime,
                Location = e.Location,
                EventTypeId = e.EventTypeId,
                EventTypeName = e.EventTypeEntity.Name,
                Capacity = e.Capacity,

                // Remaining spots and capacity
                SpotsLeft = e.Capacity -
                            e.Registrations.Count(r =>
                                r.CancelledAt == null &&
                                r.StatusEntity.Name == "Confirmed"),

                CapacityAvailability = EventStatusMapper.MapCapacityAvailability(
                    e.Capacity -
                    e.Registrations.Count(r =>
                        r.CancelledAt == null &&
                        r.StatusEntity.Name == "Confirmed")
                ),

                // MyStatus for current user
                MyStatus = EventStatusMapper.MapMyStatusNullable(
                    e.Registrations
                        .Where(r => r.UserId == customerId)
                        .OrderByDescending(r => r.RegisteredAt ?? DateTime.MinValue)
                        .Select(r => r.StatusEntity.Name)
                        .FirstOrDefault()
                )
            })
            .ToListAsync(ct);

        return new EventsSearchResult
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }


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
            .Where(x => x.StatusName == "Confirmed")
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
        // Resolve required status IDs once
        var statuses = await context.RegistrationStatuses
            .AsNoTracking()
            .Where(rs =>
                rs.Name == "Confirmed" ||
                rs.Name == "Waitlisted" ||
                rs.Name == "Cancelled")
            .Select(rs => new { rs.Id, rs.Name })
            .ToListAsync(ct);

        var confirmedId = statuses.FirstOrDefault(x => x.Name == "Confirmed")?.Id;
        var waitlistedId = statuses.FirstOrDefault(x => x.Name == "Waitlisted")?.Id;
        var cancelledId = statuses.FirstOrDefault(x => x.Name == "Cancelled")?.Id;

        // Registrations by this user on active events, excluding cancelled
        var myRegs = await context.Registrations
            .AsNoTracking()
            .Where(r =>
                r.UserId == userId &&
                r.StatusId != cancelledId)
            .Join(
                activeEvents,
                r => r.EventId,
                e => e.Id,
                (r, _) => new { r.EventId, r.StatusId }
            )
            .ToListAsync(ct);

        var myEventIds = myRegs
            .Select(x => x.EventId)
            .Distinct()
            .ToHashSet();

        var myRegistered = confirmedId.HasValue
            ? myRegs.Count(x => x.StatusId == confirmedId.Value)
            : 0;

        var myWaitlisted = waitlistedId.HasValue
            ? myRegs.Count(x => x.StatusId == waitlistedId.Value)
            : 0;

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
