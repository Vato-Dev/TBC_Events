using Application.DTOs;
using Application.Exceptions;
using Application.IdentityModels.Results;
using Application.Models;
using Application.Repositories;
using Application.Services.Abstractions;
using Domain.Models;

namespace Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;

    public UserService(
        IIdentityService identityService,
        IUserRepository userRepository, ICurrentUserService currentUser)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<AuthResult> AuthenticateAsync(LoginRequest request)
    {
        return await _identityService.AuthenticateAsync(request.Email, request.Password);
    }

    public async Task<RegisterResult> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var registerResult = await _identityService.RegisterAsync(
            request.Email,
            request.Password,
            request.UserName,
            request.PhoneNumber,
            request.OneTimePassword,
            ct
        );

        if (!registerResult.Succeeded)
        {
            return registerResult;
        }

        var userEntity = new User
        {
            Id = registerResult.UserId!.Value,
            Email = request.Email,
            FullName = request.UserName,
            Department = request.Department,
            Role = UserRole.Employee,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userRepository.CreateUserAsync(userEntity, ct);
        
        return registerResult;
    }

    public async Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var result = await _identityService.ChangePasswordWithOldPasswordAsync(
            request.Email,
            request.CurrentPassword,
            request.NewPassword
        );

        return result;
    }

  
    public async Task ForgotPasswordAsync(RoleRequest request)
    {
        await _identityService.ForgotPassword(request.Email);
    }

    /// <summary>
    /// Sends One Time Password on Phone Number
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> SendOtpCodeAsync(string phoneNumber)
    {
        await _identityService.PreregisterOtpSendAsync(phoneNumber);
        return true;
    }

    /// <summary>
    /// Takes Code validates it and resets password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<AuthResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        return await _identityService.NewPasswordAsync(
            request.Email,
            request.Code,
            request.NewPassword
        );
    }

    public async Task<AuthResult> AssignAdminRoleAsync(RoleRequest request, CancellationToken cancellationToken)
    {
        ValidateCurrentUser();
        var result = await _identityService.AssignAdminRoleAsync(request.Email);
        await _userRepository.UpdateUserRoleAsync(result.UserId, result.Role, cancellationToken);
        return result;
    }
    public async Task<AuthResult> RemoveAdminRoleAsync(RoleRequest request,CancellationToken cancellationToken)
    { 
        ValidateCurrentUser();
        var result = await _identityService.RemoveAdminRoleAsync(request.Email,_currentUser.UserId);
        await _userRepository.RemoveUserRoleAsync(result.UserId, cancellationToken);
        return result;
    }

    public async Task<AuthResult> AssignOrganizerRoleAsync(RoleRequest request, CancellationToken cancellationToken)
    {
        ValidateCurrentUser();
        var result =  await _identityService.AssignOrganizerRoleAsync(request.Email);
        await _userRepository.UpdateUserRoleAsync(result.UserId, result.Role, cancellationToken);
        return result;
    }

    public async Task<AuthResult> RemoveOrganizerRoleAsync(RoleRequest request,CancellationToken cancellationToken)
    {
        ValidateCurrentUser();
        var result = await _identityService.RemoveOrganizerRoleAsync(request.Email, _currentUser.UserId);
        await _userRepository.RemoveUserRoleAsync(result.UserId, cancellationToken);
        return result;
    }

    private void ValidateCurrentUser()
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException("User not authenticated");

        if (!_currentUser.Roles.Contains(Roles.Admin))
            throw new ForbiddenAccessException("Not enough rights");
    }
}
