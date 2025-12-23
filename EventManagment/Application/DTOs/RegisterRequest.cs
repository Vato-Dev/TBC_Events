using Destructurama.Attributed;
using Domain.Models;

namespace Application.DTOs;

public sealed record RegisterRequest
{
    public required string Email { get; init; }
    [LogMasked]
    public required string Password { get; init; }
    public required string UserName { get; init; }
    [LogMasked]
    public required string PhoneNumber { get; init; }
    [LogMasked]
    public required string OneTimePassword { get; init; }
    public required Department Department { get; init; }
}