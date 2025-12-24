using Application.Services;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using Infrastructure.models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.IdentityModels;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmailWithOtpService(this IServiceCollection services)
    {
     //   services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ISmsSender, SmsSender>(); // ar sheicvala performance singleton rom gavxade daje piriqiT (wesit egac da email sender unda iyos singleton)
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }

    public static IServiceCollection AddTokenService(this IServiceCollection services)
     =>
        services.AddSingleton(typeof(TokenService));
    
    public static IdentityBuilder AddIdentityServices(this IServiceCollection services)
        => services.AddScoped<IIdentityService, IdentityService>()
            .AddIdentity<ApplicationUser, ApplicationRole>(DefaultSetupAction)
            .AddDefaultTokenProviders();
  
    private static void DefaultSetupAction(IdentityOptions options)
    {
        options.Password = new PasswordOptions
        {
            RequireDigit = true,
            RequireLowercase = true,
            RequireNonAlphanumeric = true,
            RequireUppercase = true,
            RequiredLength = 8,
            RequiredUniqueChars = 2,
        };

        options.Lockout = new LockoutOptions { AllowedForNewUsers = true, DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3), MaxFailedAccessAttempts = 3, };
        
        //options.SignIn.RequireConfirmedAccount = true;
        //options.SignIn.RequireConfirmedEmail = true;
    }
}