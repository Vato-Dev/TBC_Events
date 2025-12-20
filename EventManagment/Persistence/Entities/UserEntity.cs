namespace Persistence.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }
    
    public virtual ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();

    public virtual ICollection<RegistrationEntity> Registrations { get; set; } = new List<RegistrationEntity>();

    public virtual RoleEntity RoleEntity { get; set; } = null!;
}
