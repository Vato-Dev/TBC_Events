namespace Persistence.Entities;

public class EventTypeEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();
}
