using Application.DTOs;
using Application.Repositories;
using Infrastructure.Services;

namespace Application.Services.Implementations;

public sealed class EventService(IEventRepository _repository) : IEventService
{
    public async Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct)
    {
        return await _repository.GetFiltersMetaAsync(customerId, ct);
    }

    public async Task<EventsSearchResult> GetEventsAsync(int customerId, EventsSearchFilters filters, CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(customerId, filters, ct);
    }

    public async Task<CategoriesResult> GetCategoriesAsync(int customerId, bool withCounts, CancellationToken ct = default)
    {
        return await _repository.GetCategoriesAsync(customerId, withCounts, ct);
    }

}