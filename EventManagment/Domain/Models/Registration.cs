namespace Domain.Models;

public class Registration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int UserId { get; set; }
    public int StatusId { get; set; }
    public DateTime? RegisteredAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}