namespace Application.DTOs;

public sealed record RoleRequest
{
    public required string Email { get; set; }
}