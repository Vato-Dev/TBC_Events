using Application.DTOs;
using Application.Repositories;
using Domain.Models;
using Infrastructure.Services;

namespace Application.Services.Implementations;

public sealed class EventService(IEventRepository _repository) : IEventService
{
    public async Task<EventFiltersMeta> GetFiltersMetaAsync(int customerId, CancellationToken ct)
    {
        return await _repository.GetFiltersMetaAsync(customerId, ct);
    }
}