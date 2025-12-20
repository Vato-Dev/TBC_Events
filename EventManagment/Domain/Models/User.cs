namespace Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
}