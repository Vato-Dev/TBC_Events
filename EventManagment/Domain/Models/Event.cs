using Domain.Exceptions;

namespace Domain.Models;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public DateOnly RegistrationStart { get; set; }
    public DateOnly RegistrationEnd { get; set; }
    public Location Location { get; set; } = null!;
    public int Capacity { get; set; } 
    public int RegisteredUsers { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }//i don't really know if domain model needs this.
    public DateTime UpdatedAt { get; set; }
    public int EventTypeId { get; set; }
    public int CreatedById { get; set; }
    public List<AgendaItem> Agendas { get; private  set; } = new();
    public List<Tag> Tags { get; private set; } = new();
    public void AddTag(Tag tag)
    {
        Tags.Add(tag);
    }
    public void AddAgendaItem(AgendaItem item)
    {
        if (Agendas.Any(a => a.StartTime == item.StartTime))
            throw new DomainException("Agenda overlap");
        Agendas.Add(item);
    }
}

public class AgendaItem
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public AgendaItemType Type { get; set; }
    public string? Location { get; set; }
    public List<AgendaTrack> Tracks { get; private set; } = new();
    public void AddTrackItem(AgendaTrack track)
    {
        Tracks.Add(track);
    }
}
public class AgendaTrack
{
    public int Id { get; set; }
    public int AgendaItemId { get; set; }   

    public string Title { get; set; } = null!;
    public string? Speaker { get; set; }
    public string? Room { get; set; }
}

public enum AgendaItemType
{
    Registration,
    Keynote,
    Workshop,
    Break,
    Lunch,
    Activity,
    Panel,
    Ceremony
}
public sealed class Location
{
    public LocationType LocationType { get; set; }
    public Address  Address { get; set; } = null!;
    public int RoomNumber { get; set; } 
    public int FloorNumber { get; set; }
    public string? AdditionalInformation { get; set; } = null!;
}

public sealed class Address
{
    public string VenueName { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
  
}
public enum LocationType
{
    InPerson,
    Virtual,
    Hybrid
}
