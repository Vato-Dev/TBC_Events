using Destructurama.Attributed;

namespace Application.DTOs;

public sealed record LoginRequest
{
    public required string Email { get; init; }
    [LogMasked]
    public required string Password { get; init; }
}