namespace Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Department Department { get; set; }
    
    public UserRole Role { get; set; }
    public List<Event> Events { get; set; } = new();
    public List<Registration> Registrations { get; set; } = new();
}

public enum Department
{
    Marketing = 1,
    Engineering ,
    Product ,
    HR ,
    Finance ,
    Design ,
    Operations , 
    Special = 99
}
public enum UserRole
{
    Unknown = 0, //For CLR it can't set 1 by default from config because default is 0 , just to get rid of annoying warning
    Employee = 1,
    Organizer = 2,
    Admin = 3
}