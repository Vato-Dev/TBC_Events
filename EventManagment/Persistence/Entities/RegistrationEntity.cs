namespace Persistence.Entities;

public class RegistrationEntity
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

    public DateTime? RegisteredAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public virtual EventEntity EventEntity { get; set; } = null!;

    public virtual RegistrationStatusEntity StatusEntity { get; set; } = null!;

    public virtual UserEntity UserEntity { get; set; } = null!;
}
