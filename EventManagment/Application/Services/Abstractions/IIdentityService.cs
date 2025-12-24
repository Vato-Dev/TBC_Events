using Application.IdentityModels.Results;

namespace Application.Services.Abstractions;

public interface IIdentityService
{
    Task<AuthResult> AuthenticateAsync(string email, string password);

    Task<RegisterResult> RegisterAsync(string email, string password, string userName, string phoneNumber,
        string oneTimePassword, CancellationToken ct);

    Task<ChangePasswordResult> ChangePasswordWithOldPasswordAsync(string email, string passwordCurrent,
        string newPassword);

    Task PreregisterOtpSendAsync(string phoneNumber);
    Task ForgotPassword(string email, string baseUrl);
    Task<AuthResult> NewPasswordAsync(string email, string code, string newPassword);
    Task<AuthResult> AssignAdminRoleAsync(string email);
    Task<AuthResult> RemoveAdminRoleAsync(string email, int requesterId);

    Task<AuthResult> AssignOrganizerRoleAsync(string email);
    Task<AuthResult> RemoveOrganizerRoleAsync(string email, int requesterId);
}