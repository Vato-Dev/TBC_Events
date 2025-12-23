using Destructurama.Attributed;

namespace Application.DTOs;

public sealed record ResetPasswordRequest
{
    public required string Email { get; init; }
    [LogMasked]
    public required string Code { get; init; }
    [LogMasked]
    public required string NewPassword { get; init; }
}