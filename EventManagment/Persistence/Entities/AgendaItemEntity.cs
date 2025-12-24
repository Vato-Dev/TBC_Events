using Domain.Models;

namespace Persistence.Entities;

public class AgendaItemEntity
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public virtual EventEntity Event { get; set; } = null!;

    public TimeOnly StartTime { get; set; }
    public TimeSpan Duration { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public AgendaItemType Type { get; set; }
    public string? Location { get; set; }
    
    public virtual ICollection<AgendaTrackEntity> Tracks { get; set; } = new List<AgendaTrackEntity>();
}