namespace Persistence.Entities;

public class EventTagEntity
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TagId { get; set; }

    public virtual EventEntity EventEntity { get; set; } = null!;

    public virtual TagEntity TagEntity { get; set; } = null!;
}
