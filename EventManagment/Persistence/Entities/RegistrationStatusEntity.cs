namespace Persistence.Entities;

public class RegistrationStatusEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<RegistrationEntity> Registrations { get; set; } = new List<RegistrationEntity>();
}
