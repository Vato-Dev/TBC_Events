using Domain.Models;

namespace Persistence.Entities;

public class LocationEntity
{
    public LocationType LocationType { get; set; }
    public AddressEntity  Address { get; set; } = null!;
    public int RoomNumber { get; set; } 
    public int FloorNumber { get; set; }
    public string? AdditionalInformation { get; set; } = null!;
}

public class AddressEntity
{
    public string VenueName { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
}