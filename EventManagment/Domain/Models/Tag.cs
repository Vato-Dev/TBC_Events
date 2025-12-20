namespace Domain.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Category { get; set; }
}