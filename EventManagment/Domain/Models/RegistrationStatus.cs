using System;
using System.Collections.Generic;

namespace Persistence.Models;

public class RegistrationStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
