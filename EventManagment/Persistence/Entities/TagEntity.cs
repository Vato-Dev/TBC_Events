namespace Persistence.Entities;

public class TagEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public virtual ICollection<EventTagEntity> EventTags { get; set; } = new List<EventTagEntity>();
}
