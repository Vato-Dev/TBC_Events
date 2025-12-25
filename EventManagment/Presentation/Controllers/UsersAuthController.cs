using Application.DTOs;
using Application.Models;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/users-auth")]
public class UsersAuthController(IUserService userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await userService.AuthenticateAsync(request);
        if (result.Succeeded)
        {
            return Ok(result.Tokens);
        }

        return BadRequest(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("preregister")]
    public async Task<IActionResult> PreRegister(string phoneNumber, CancellationToken cancellationToken)
    {
        var result = await userService.SendOtpCodeAsync(phoneNumber);
        if (result)
        {
            return NoContent();
        }

        return BadRequest(); //aq uxeshad miweria bad request imitom rom error iqneba da Middlware daaswrebs yoveltvis
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await userService.RegisterAsync(request, cancellationToken);
        if (result.Succeeded)
        {
            return Ok(result.Tokens);
        }

        return BadRequest(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("send-reset-password-link")]
    public async Task<IActionResult> SendResetPasswordCode(ForgotPasswordLinkRequest request,
        CancellationToken cancellationToken)
    {
        await userService.ForgotPasswordAsync(request);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userService.ResetPasswordAsync(request);
        if (result.Succeeded)
        {
            return Ok(result.Tokens);
        }

        return BadRequest(result.Errors);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await userService.ChangePasswordAsync(request);
        if (result.Succeeded)
        {
            return Ok(result.Tokens);
        }

        return BadRequest(result.Errors);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Roles = Roles.Admin)]
    [HttpPut("assign-admin")]
    public async Task<IActionResult> AssignAdminRole(
        [FromBody] RoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userService.AssignAdminRoleAsync(
            request,
            cancellationToken
        );

        if (result.Succeeded)
            return Ok(result.Tokens);

        return BadRequest(result.Errors);
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Roles = Roles.Admin)]
    [HttpPut("remove-admin")]
    public async Task<IActionResult> RemoveAdminRole(
        [FromBody] RoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userService.RemoveAdminRoleAsync(    
            request,
            cancellationToken
        );

        if (result.Succeeded)
            return Ok(result.Tokens);

        return BadRequest(result.Errors);
    }   
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Roles = Roles.Admin)]
    [HttpPut("assign-organizer")]
    public async Task<IActionResult> AssignOrganizerRole(
        [FromBody] RoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userService.AssignOrganizerRoleAsync(
            request,
            cancellationToken
        );

        if (result.Succeeded)
            return Ok(result.Tokens);

        return BadRequest(result.Errors);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Roles = Roles.Admin)]
    [HttpPut("remove-organizer")]
    public async Task<IActionResult> RemoveOrganizerRole(
        [FromBody] RoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userService.RemoveOrganizerRoleAsync(
            request,
            cancellationToken
        );

        if (result.Succeeded)
            return Ok(result.Tokens);

        return BadRequest(result.Errors);
    }

}