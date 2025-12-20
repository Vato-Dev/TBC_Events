using System;
using System.Collections.Generic;

namespace Persistence.Models;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual Role Role { get; set; } = null!;
}
