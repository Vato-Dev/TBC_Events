using System;
using System.Collections.Generic;

namespace Persistence.Models;

public class Event
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
    public string TestPRop { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();

    public virtual EventType EventType { get; set; } = null!;

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
