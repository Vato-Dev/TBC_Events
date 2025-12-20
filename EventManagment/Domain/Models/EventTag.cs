using System;
using System.Collections.Generic;

namespace Persistence.Models;

public class EventTag
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TagId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
