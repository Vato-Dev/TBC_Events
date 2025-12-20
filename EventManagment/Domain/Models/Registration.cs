using System;
using System.Collections.Generic;

namespace Persistence.Models;

public class Registration
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

    public DateTime? RegisteredAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual RegistrationStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
