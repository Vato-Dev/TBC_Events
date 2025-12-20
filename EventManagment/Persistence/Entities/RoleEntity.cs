namespace Persistence.Entities;

public class RoleEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
