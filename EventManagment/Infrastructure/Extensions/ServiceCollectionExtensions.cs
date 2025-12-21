using Application.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.IdentityModels;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
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

        options.SignIn.RequireConfirmedAccount = true;
        options.SignIn.RequireConfirmedEmail = true;
    }
}