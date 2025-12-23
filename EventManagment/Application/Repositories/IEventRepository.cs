using Domain.Models;

namespace Application.Repositories;

public interface IEventRepository
{
    public Task<int> CreateEventAsync(Event @event,List<int> tagIds, CancellationToken cancellationToken);
    public Task<Event> GetEventByIdAsync(int id, CancellationToken cancellationToken);
    public Task UpdateEventAsync(Event @event, CancellationToken cancellationToken);
}