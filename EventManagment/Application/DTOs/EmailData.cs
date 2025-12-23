namespace Application.DTOs;

public class EmailData
{
    public required string EmailToName { get; set; }
    public required string EmailSubject { get; set; }
    public required string Message { get; set; }
}