using Destructurama.Attributed;

namespace Application.DTOs;
//Todo replace request models from DTO folder
public sealed record ChangePasswordRequest
{
    public required string Email { get; init; }
    [LogMasked]
    public required string CurrentPassword { get; init; }
    [LogMasked]
    public required string NewPassword { get; init; }
}