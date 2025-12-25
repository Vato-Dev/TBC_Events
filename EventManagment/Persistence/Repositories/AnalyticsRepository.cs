using Application.DTOs;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AnalyticsRepository(AppDbContext context) : IAnalyticsRepository
    {
        public async Task<AnalyticsDto> GetAnalyticsAsync(AnalyticsFilters filters, CancellationToken ct)
        {
            var (from, to) = ResolveDateRange(filters);

            var eventsQ = BuildEventsQuery(filters, from, to);

            // Previous period (same length) for deltas/trends where needed
            var (prevFrom, prevTo) = GetPreviousPeriod(from, to);
            var prevEventsQ = BuildEventsQuery(filters, prevFrom, prevTo);

            var kpisTask = await GetKpisAsync(eventsQ, prevEventsQ, from, to, prevFrom, prevTo, ct);
            var regTrendTask = await GetRegistrationTrendAsync(eventsQ, from, to, ct);
            var categoryTask = await GetCategoryDistributionAsync(eventsQ, ct);
            var topEventsTask = await GetTopEventsByDemandAsync(eventsQ, ct);
            var deptTask = await GetDepartmentParticipationAsync(eventsQ, prevEventsQ, from, to, prevFrom, prevTo, ct);

            return new AnalyticsDto
            {
                From = from,
                To = to,
                Kpis = kpisTask,
                RegistrationTrend = regTrendTask,
                CategoryDistribution = categoryTask,
                TopEventsByDemand = topEventsTask,
                DepartmentParticipation = deptTask
            };
        }

        // ----------------------------
        // Base filtering / date helpers
        // ----------------------------

        private static (DateTime From, DateTime To) ResolveDateRange(AnalyticsFilters filters)
        {
            // sensible defaults (Last 30 days)
            var to = (filters.To ?? DateTime.UtcNow).ToUniversalTime();
            var from = (filters.From ?? to.AddDays(-30)).ToUniversalTime();

            // normalize in case caller flips them
            if (from > to) (from, to) = (to, from);

            return (from, to);
        }

        private static (DateTime PrevFrom, DateTime PrevTo) GetPreviousPeriod(DateTime from, DateTime to)
        {
            var span = to - from;
            var prevTo = from;
            var prevFrom = from - span;
            return (prevFrom, prevTo);
        }

        private IQueryable<EventEntity> BuildEventsQuery(AnalyticsFilters filters, DateTime from, DateTime to)
        {
            var q = context.Events
                .AsNoTracking()
                .Where(e => e.StartDateTime >= from && e.StartDateTime <= to);

            if (filters.EventTypeIds is { Count: > 0 })
                q = q.Where(e => filters.EventTypeIds!.Contains(e.EventTypeId));

            if (filters.Locations is { Count: > 0 })
                q = q.Where(e => filters.Locations!.Contains(e.Location.Address.VenueName));

            return q;
        }

        // -------------
        // KPI section
        // -------------

        private async Task<AnalyticsKpisDto> GetKpisAsync(
            IQueryable<EventEntity> eventsQ,
            IQueryable<EventEntity> prevEventsQ,
            DateTime from, DateTime to,
            DateTime prevFrom, DateTime prevTo,
            CancellationToken ct)
        {
            // Total registrations (non-cancelled) in selected events
            var totalRegsTask = await CountRegistrationsAsync(eventsQ, ct);
            var prevTotalRegsTask = await CountRegistrationsAsync(prevEventsQ, ct);

            // Active participants = distinct users with a non-cancelled registration
            var activeParticipantsTask =  await CountDistinctParticipantsAsync(eventsQ, ct);
            var prevActiveParticipantsTask = await CountDistinctParticipantsAsync(prevEventsQ, ct);

            // Cancellation rate = cancelled / total (all) for selected events
            var cancelsTask = await CountCancelledRegistrationsAsync(eventsQ, ct);
            var allRegsTask = await CountAllRegistrationsAsync(eventsQ, ct);

            var totalRegs = totalRegsTask;
            var prevTotalRegs = prevTotalRegsTask;

            var activeParticipants = activeParticipantsTask;
            var prevActiveParticipants = prevActiveParticipantsTask;

            var cancelled = cancelsTask;
            var allRegs = allRegsTask;

            var cancellationRate = allRegs == 0 ? 0m : (decimal)cancelled / allRegs;

            return new AnalyticsKpisDto
            {
                TotalRegistrations = totalRegs,
                TotalRegistrationsChangePct = PercentChange(prevTotalRegs, totalRegs),

                ActiveParticipants = activeParticipants,
                ActiveParticipantsChangePct = PercentChange(prevActiveParticipants, activeParticipants),

                CancellationRate = cancellationRate,
                // If you want change vs previous period cancellation rate, compute prev cancelled/all similarly.
            };
        }

        private Task<int> CountAllRegistrationsAsync(IQueryable<EventEntity> eventsQ, CancellationToken ct) =>
            context.Registrations
                .AsNoTracking()
                .Where(r => eventsQ.Select(e => e.Id).Contains(r.EventId))
                .CountAsync(ct);

        private Task<int> CountCancelledRegistrationsAsync(IQueryable<EventEntity> eventsQ, CancellationToken ct) =>
            context.Registrations
                .AsNoTracking()
                .Where(r => eventsQ.Select(e => e.Id).Contains(r.EventId))
                .Where(r => r.CancelledAt != null) // adjust if you prefer StatusId/StatusEntity.Name
                .CountAsync(ct);

        private Task<int> CountRegistrationsAsync(IQueryable<EventEntity> eventsQ, CancellationToken ct) =>
            context.Registrations
                .AsNoTracking()
                .Where(r => eventsQ.Select(e => e.Id).Contains(r.EventId))
                .Where(r => r.CancelledAt == null)
                .CountAsync(ct);

        private Task<int> CountDistinctParticipantsAsync(IQueryable<EventEntity> eventsQ, CancellationToken ct) =>
            context.Registrations
                .AsNoTracking()
                .Where(r => eventsQ.Select(e => e.Id).Contains(r.EventId))
                .Where(r => r.CancelledAt == null)
                .Select(r => r.UserId)
                .Distinct()
                .CountAsync(ct);

        private static decimal? PercentChange(int previous, int current)
        {
            if (previous <= 0) return null; // or return 0; depends on your UI
            return (decimal)(current - previous) / previous;
        }

        // --------------------
        // Registration Trend
        // --------------------

        private async Task<IReadOnlyList<RegistrationTrendPointDto>> GetRegistrationTrendAsync(
            IQueryable<EventEntity> eventsQ,
            DateTime from, DateTime to,
            CancellationToken ct)
        {
            // Trend by month based on registrations' RegisteredAt.
            // If you instead want trend by event month, group by Event.StartDateTime.
            var eventIds = eventsQ.Select(e => e.Id);

            var points = await context.Registrations
                .AsNoTracking()
                .Where(r => eventIds.Contains(r.EventId))
                .Where(r => r.RegisteredAt != null)
                .Where(r => r.RegisteredAt >= from && r.RegisteredAt <= to)
                .Where(r => r.CancelledAt == null)
                .GroupBy(r => new { r.RegisteredAt!.Value.Year, r.RegisteredAt!.Value.Month })
                .Select(g => new RegistrationTrendPointDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Registrations = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync(ct);

            return points;
        }

        // ----------------------
        // Category distribution
        // ----------------------

        private async Task<IReadOnlyList<CategoryDistributionDto>> GetCategoryDistributionAsync(
            IQueryable<EventEntity> eventsQ,
            CancellationToken ct)
        {
            var rows = await eventsQ
                .Join(context.EventTypes.AsNoTracking(),
                      e => e.EventTypeId,
                      t => t.Id,
                      (e, t) => new { e.Id, EventTypeName = t.Name })
                .GroupBy(x => x.EventTypeName)
                .Select(g => new CategoryDistributionDto
                {
                    Category = g.Key,
                    EventsCount = g.Count()
                })
                .OrderByDescending(x => x.EventsCount)
                .ToListAsync(ct);

            return rows;
        }

        // --------------------
        // Top events by demand
        // --------------------

        private async Task<IReadOnlyList<TopEventByDemandDto>> GetTopEventsByDemandAsync(
            IQueryable<EventEntity> eventsQ,
            CancellationToken ct)
        {
            var eventIds = eventsQ.Select(e => e.Id);

            // Aggregate registrations per event (non-cancelled)
            var regAgg = context.Registrations
                .AsNoTracking()
                .Where(r => eventIds.Contains(r.EventId))
                .Where(r => r.CancelledAt == null)
                .GroupBy(r => r.EventId)
                .Select(g => new { EventId = g.Key, Registrations = g.Count() });

            // Left join to keep events with 0 regs
            var top = await eventsQ
                .Select(e => new
                {
                    e.Id,
                    e.Title,
                    e.StartDateTime,
                    e.Capacity,
                    e.EventTypeId
                })
                .GroupJoin(regAgg, e => e.Id, a => a.EventId, (e, a) => new { e, a = a.FirstOrDefault() })
                .Join(context.EventTypes.AsNoTracking(),
                      x => x.e.EventTypeId,
                      t => t.Id,
                      (x, t) => new TopEventByDemandDto
                      {
                          EventId = x.e.Id,
                          EventName = x.e.Title,
                          Category = t.Name,
                          Date = x.e.StartDateTime,
                          Registrations = x.a == null ? 0 : x.a.Registrations,
                          Capacity = x.e.Capacity,
                          Utilization = x.e.Capacity <= 0
                              ? 0m
                              : (decimal)(x.a == null ? 0 : x.a.Registrations) / x.e.Capacity,
                          Waitlist = x.e.Capacity <= 0
                              ? (x.a == null ? 0 : x.a.Registrations)
                              : Math.Max(0, (x.a == null ? 0 : x.a.Registrations) - x.e.Capacity)
                      })
                .OrderByDescending(x => x.Utilization)
                .ThenByDescending(x => x.Registrations)
                .Take(10)
                .ToListAsync(ct);

            return top;
        }

        // --------------------------
        // Department participation
        // --------------------------
        private async Task<IReadOnlyList<DepartmentParticipationDto>> GetDepartmentParticipationAsync(
    IQueryable<EventEntity> eventsQ,
    IQueryable<EventEntity> prevEventsQ,
    DateTime from, DateTime to,
    DateTime prevFrom, DateTime prevTo,
    CancellationToken ct)
        {
            var eventIds = await GetEventIdsAsync(eventsQ, ct);
            var prevEventIds = await GetEventIdsAsync(prevEventsQ, ct);

            var employeesPerDept = await GetEmployeesPerDepartmentAsync(ct);
            var activeNow = await GetActiveParticipantsPerDepartmentAsync(eventIds, ct);
            var regsNow = await GetRegistrationsPerDepartmentAsync(eventIds, ct);
            var activePrev = await GetActiveParticipantsPerDepartmentAsync(prevEventIds, ct);

            return BuildDepartmentParticipationRows(employeesPerDept, activeNow, regsNow, activePrev);
        }

        private static Task<List<int>> GetEventIdsAsync(IQueryable<EventEntity> q, CancellationToken ct) =>
            q.Select(e => e.Id).ToListAsync(ct);

        private Task<List<(Department Department, int TotalEmployees)>> GetEmployeesPerDepartmentAsync(CancellationToken ct) =>
            context.DomainUsers
                .AsNoTracking()
                .GroupBy(u => u.Department)
                .Select(g => new ValueTuple<Department, int>(g.Key, g.Count()))
                .ToListAsync(ct);

        private Task<List<(Department Department, int ActiveParticipants)>> GetActiveParticipantsPerDepartmentAsync(
            List<int> eventIds,
            CancellationToken ct)
        {
            return context.Registrations
                .AsNoTracking()
                .Where(r => eventIds.Contains(r.EventId))
                .Where(r => r.CancelledAt == null)
                .Join(context.DomainUsers.AsNoTracking(),
                      r => r.UserId,
                      u => u.Id,
                      (r, u) => new { u.Department, r.UserId })
                .Distinct()
                .GroupBy(x => x.Department)
                .Select(g => new ValueTuple<Department, int>(g.Key, g.Count()))
                .ToListAsync(ct);
        }

        private Task<List<(Department Department, int TotalRegistrations)>> GetRegistrationsPerDepartmentAsync(
            List<int> eventIds,
            CancellationToken ct)
        {
            return context.Registrations
                .AsNoTracking()
                .Where(r => eventIds.Contains(r.EventId))
                .Where(r => r.CancelledAt == null)
                .Join(context.DomainUsers.AsNoTracking(),
                      r => r.UserId,
                      u => u.Id,
                      (r, u) => new { u.Department })
                .GroupBy(x => x.Department)
                .Select(g => new ValueTuple<Department, int>(g.Key, g.Count()))
                .ToListAsync(ct);
        }

        private static IReadOnlyList<DepartmentParticipationDto> BuildDepartmentParticipationRows(
            List<(Department Department, int TotalEmployees)> employeesPerDept,
            List<(Department Department, int ActiveParticipants)> activeNow,
            List<(Department Department, int TotalRegistrations)> regsNow,
            List<(Department Department, int ActiveParticipants)> activePrev)
        {
            var empDict = employeesPerDept.ToDictionary(x => x.Department, x => x.TotalEmployees);
            var activeNowDict = activeNow.ToDictionary(x => x.Department, x => x.ActiveParticipants);
            var regsNowDict = regsNow.ToDictionary(x => x.Department, x => x.TotalRegistrations);
            var activePrevDict = activePrev.ToDictionary(x => x.Department, x => x.ActiveParticipants);

            var result = new List<DepartmentParticipationDto>();

            foreach (var (dept, totalEmployees) in empDict.OrderBy(k => k.Key.ToString()))
            {
                activeNowDict.TryGetValue(dept, out var activeParticipantsCount);
                regsNowDict.TryGetValue(dept, out var totalRegistrationsCount);
                activePrevDict.TryGetValue(dept, out var prevActiveCount);

                var participationRate = totalEmployees == 0 ? 0m : (decimal)activeParticipantsCount / totalEmployees;
                var prevParticipationRate = totalEmployees == 0 ? 0m : (decimal)prevActiveCount / totalEmployees;

                var avgEventsPerEmployee = totalEmployees == 0 ? 0m : (decimal)totalRegistrationsCount / totalEmployees;

                result.Add(new DepartmentParticipationDto
                {
                    Department = dept,
                    TotalEmployees = totalEmployees,
                    ActiveParticipants = activeParticipantsCount,
                    ParticipationRate = participationRate,
                    TotalRegistrations = totalRegistrationsCount,
                    AvgEventsPerEmployee = avgEventsPerEmployee,
                    ParticipationRateChangePct = prevParticipationRate == 0m
                        ? null
                        : (participationRate - prevParticipationRate) / prevParticipationRate
                });
            }

            return result;
        }

    }

}
