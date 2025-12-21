using Application.Repositories;
using Domain.Models;

namespace Infrastructure.Services;

public sealed class EventsFiltersService : IEventsFiltersService
{
    private readonly IEventsFiltersRepository _repository;

    public EventsFiltersService(IEventsFiltersRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct)
    {
        return await _repository.GetFiltersMetaAsync(customerId, ct);
    }
}