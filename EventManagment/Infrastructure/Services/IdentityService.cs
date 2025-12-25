using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.IdentityModels.Results;
using Application.Models;
using Application.Services.Abstractions;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence.IdentityModels;

namespace Infrastructure.Services;

public sealed class IdentityService( //Todo since cancellation Token Does not work with Identity add checking manually in beginning of every method 
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOtpService otpService,
    TokenService tokenService,  
    IEmailSender emailSender,
    ISmsSender smsSender)
    : IIdentityService
{
    public async Task<AuthResult> AuthenticateAsync(string email, string password) //Response request models
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            //ToDo Log 
            return AuthResult.Failed([new ApplicationError("UserNotFound", "User not found" )]);
        }
        var result = await signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!result.Succeeded)
        {
            //ToDo Log 
            return AuthResult.Failed([new ApplicationError("Not Allowed","Could be Invalid Email or Password, Try again")]);
        }

        return AuthResult.Succeed(await CreateTokenResponse(user), user.Id, user.UserName!);
    }

    public async Task<RegisterResult> RegisterAsync(string email, string password, string userName, [Phone]string phoneNumber,
        string oneTimePassword,Department department,  CancellationToken ct) //CancellationToken gadmoveci raxan standartia ar aqvs supporti samwuxarod da IsCancellationRequested gamoyeneba anti-pattern aris aq
    {
        var user = new ApplicationUser { Email = email, UserName = userName, PhoneNumber = phoneNumber , PhoneNumberConfirmed = true, LastOtpSentTime = DateTime.UtcNow};
        
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded) 
            return RegisterResult.Failed(result.Errors.Adapt<ApplicationError[]>());
        
        return !result.Succeeded ? RegisterResult.Failed(result.Errors.Adapt<ApplicationError[]>()) : RegisterResult.Success(user.Id , await CreateTokenResponse(user));
    }

    public async Task PreregisterOtpSendAsync([Phone]string phoneNumber)
    {
        var generatedToken = await otpService.GenerateOtpAsync(phoneNumber);
        await smsSender.SendAsync(phoneNumber,
            "Your One-Time Password is: " + generatedToken + "Please do not send it to anyone");
    }
    
    public async Task<ChangePasswordResult> ChangePasswordWithOldPasswordAsync(string email, string passwordCurrent , string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return ChangePasswordResult.Failed([new ApplicationError("Not Found", "User not found" )]);
        }

        if (!await userManager.CheckPasswordAsync(user, passwordCurrent))
        {
            return ChangePasswordResult.InvalidPassword();
        }

        var result = await userManager.ChangePasswordAsync(user, passwordCurrent, newPassword);
        if (!result.Succeeded)
        {
            return ChangePasswordResult.Failed(result.Errors.Adapt<ApplicationError[]>());
        }
        
        await userManager.UpdateSecurityStampAsync(user);
        return ChangePasswordResult.Succeed(await CreateTokenResponse(user));
    }

    public async Task ForgotPassword(string email, string clientUri)
    {
        var  user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return ;
        }
        var token =  await userManager.GeneratePasswordResetTokenAsync(user);
        
        var callbackUrl = $"{clientUri}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";        
        
        await emailSender.SendEmailAsync(new EmailData
        {
            EmailSubject = $"Reset Password Request for {user.UserName}", 
            EmailToName = user.Email!,
            Message =
                $"A password reset was requested from your account. If it was not you, please change your password. If you requested this action please on this Link {callbackUrl}"
        });

        await CreateTokenResponse(user); //to invalidate previous token // it does not work like this lol i forgot

    }
    public async Task<AuthResult> NewPasswordAsync(string email, string code , string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
           return AuthResult.Failed([new ApplicationError("UserNotFound", "User not found" )]);
        }
        var result = await userManager.ResetPasswordAsync(user, code , newPassword);
        if (!result.Succeeded)
        {
            return AuthResult.Failed(result.Errors.Adapt<ApplicationError[]>());
        }
        await userManager.UpdateSecurityStampAsync(user);
        return AuthResult.Succeed(await CreateTokenResponse(user));
    }

    #region Roles Assign/Remove

    public async Task<AuthResult> AssignAdminRoleAsync(string email) 
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return  AuthResult.Failed([new ApplicationError("UserNotFound", "User not found" )]);
        }

        var userRoles = await userManager.GetRolesAsync(user);

        if (userRoles.Contains(Roles.Admin))
        {
            return  AuthResult.Failed([new ApplicationError("User Has Role", "User Already Has This Rights" )]);
        }
        
        var result = await userManager.AddToRoleAsync(user, Roles.Admin);
        if (!result.Succeeded)
            return  AuthResult.Failed(result.Errors.Adapt<ApplicationError[]>());

        
        await userManager.UpdateSecurityStampAsync(user);
        return AuthResult.Succeed(await CreateTokenResponse(user), user.Id, UserRole.Admin);//Incorrect should be ext method with mapping or [Description] attribute and get role method
    }
    public async Task<AuthResult> RemoveAdminRoleAsync(string assignRoleToEmail, int requesterId)
    {
        var user = await userManager.FindByEmailAsync(assignRoleToEmail);
        if (user is null)
        {
            return  AuthResult.Failed([new ApplicationError("UserNotFound", "User not found" )]);
        }
        
        if (user.Id == requesterId)
        {
            return AuthResult.Failed([new ApplicationError("ForbiddenAction", "You cannot remove this user's role")]);
        }
        
        var userRoles = await userManager.GetRolesAsync(user);
        if (!userRoles.Contains(Roles.Admin))
        {
            return AuthResult.Succeed(new TokensReponse());//Not requires error because nothing happened actually just return nothing
        }
        
        var result = await userManager.RemoveFromRoleAsync(user, Roles.Admin);
        if (!result.Succeeded)
            return AuthResult.Failed(result.Errors.Adapt<ApplicationError[]>());
        
        await userManager.UpdateSecurityStampAsync(user);
        return AuthResult.Succeed(await CreateTokenResponse(user) , user.Id, UserRole.Admin);
    }
    public async Task<AuthResult> AssignOrganizerRoleAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return AuthResult.Failed([
                new ApplicationError("UserNotFound", "User not found")
            ]);
        }

        var userRoles = await userManager.GetRolesAsync(user);
        if (userRoles.Contains(Roles.Organizer))
        {
            return AuthResult.Failed([
                new ApplicationError("UserHasRole", "User already has Organizers role")
            ]);
        }

        var result = await userManager.AddToRoleAsync(user, Roles.Organizer);
        if (!result.Succeeded)
        {
            return AuthResult.Failed(result.Errors.Adapt<ApplicationError[]>());
        }

        await userManager.UpdateSecurityStampAsync(user);
        return AuthResult.Succeed(await CreateTokenResponse(user), user.Id, UserRole.Organizer);
    }
    public async Task<AuthResult> RemoveOrganizerRoleAsync(string email, int requesterId)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return AuthResult.Failed([
                new ApplicationError("UserNotFound", "User not found")
            ]);
        }

        if (user.Id == requesterId)
        {
            return AuthResult.Failed([new ApplicationError("ForbiddenAction", "You cannot remove this user's organizer role")]);
        }
        var userRoles = await userManager.GetRolesAsync(user);
        
        if (!userRoles.Contains(Roles.Organizer))
        {
            return AuthResult.Succeed(new TokensReponse());//Not requires error because nothing happened actually just return nothing
        }

        var result = await userManager.RemoveFromRoleAsync(user, Roles.Organizer);
        if (!result.Succeeded)
        {
            return AuthResult.Failed(result.Errors.Adapt<ApplicationError[]>());
        }

        await userManager.UpdateSecurityStampAsync(user);
        return AuthResult.Succeed(await CreateTokenResponse(user), user.Id, UserRole.Organizer);
    }   
    
    #endregion


    private async Task<TokensReponse> CreateTokenResponse(ApplicationUser user)
    {

        var roles = await userManager.GetRolesAsync(user);
        return new TokensReponse
        {
            AccessToken = tokenService.GenerateAccessToken(user,roles),
        };
    }
}