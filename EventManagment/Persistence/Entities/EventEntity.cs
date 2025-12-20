namespace Persistence.Entities;

public class EventEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string Location { get; set; } = null!;

    public int Capacity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int EventTypeId { get; set; }

    public int CreatedById { get; set; }

    public virtual UserEntity CreatedBy { get; set; } = null!;

    public virtual ICollection<EventTagEntity> EventTags { get; set; } = new List<EventTagEntity>();

    public virtual EventTypeEntity EventTypeEntity { get; set; } = null!;

    public virtual ICollection<RegistrationEntity> Registrations { get; set; } = new List<RegistrationEntity>();
}
