using Domain.Models;
using Persistence.IdentityModels;

namespace Persistence.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;
    
    public bool IsActive { get; set; }
    public UserRole Role { get; set; }
    public Department Department { get; set; }

    public DateTime? CreatedAt { get; set; }
    
    public virtual ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();

    public virtual ICollection<RegistrationEntity> Registrations { get; set; } = new List<RegistrationEntity>();

    
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;

}
