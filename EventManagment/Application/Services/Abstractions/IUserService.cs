using Application.DTOs;
using Application.IdentityModels.Results;

namespace Application.Services.Abstractions;

public interface IUserService
{
    Task<AuthResult> AuthenticateAsync(LoginRequest request);
    Task<bool> SendOtpCodeAsync(string phoneNumber);
    Task<RegisterResult> RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request);
    Task ForgotPasswordAsync(RoleRequest request);
    Task<AuthResult> ResetPasswordAsync(ResetPasswordRequest request);
    Task<AuthResult> AssignAdminRoleAsync(RoleRequest request, CancellationToken cancellationToken);
    Task<AuthResult> RemoveAdminRoleAsync(RoleRequest request, CancellationToken cancellationToken);
    Task<AuthResult> AssignOrganizerRoleAsync(RoleRequest request, CancellationToken cancellationToken);
    Task<AuthResult> RemoveOrganizerRoleAsync(RoleRequest request, CancellationToken cancellationToken);
    
}