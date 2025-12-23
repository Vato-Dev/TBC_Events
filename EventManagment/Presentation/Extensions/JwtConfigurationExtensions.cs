using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Extensions;

public static class JwtConfigurationExtensions
{
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new()
                 {
                     ValidateAudience = true,
                     ValidateIssuer = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,

                     ValidIssuer = configuration.GetJwtIssuer(),
                     ValidAudience = configuration.GetJwtAudience(),
                     IssuerSigningKey = configuration.GetIssuerSigningKey(),


                     ClockSkew = TimeSpan.FromMinutes(3)
                 };
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = ctx =>
                     {
                         Console.WriteLine("JWT failed: " + ctx.Exception.Message);
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = ctx =>
                     {
                         Console.WriteLine("JWT valid for: " + ctx.Principal.Identity?.Name);
                         return Task.CompletedTask;
                     }
                 };
    });
        return services;
    }

    public static string GetJwtIssuer(this IConfiguration configuration)
    {
        return configuration.GetValueOrThrow("Authentication:Issuer");
    }
    
    public static string GetJwtAudience(this IConfiguration configuration)
    {
        return configuration.GetValueOrThrow("Authentication:Audience");
    }
        
    public static string GetJwtSecretKey(this IConfiguration configuration)
    {
        return configuration.GetValueOrThrow("Authentication:SecretKey");
    }        
    public static SecurityKey GetIssuerSigningKey(this IConfiguration configuration)
    {
        return new SymmetricSecurityKey(Convert.FromBase64String(configuration.GetJwtSecretKey()));
    }


    public static string GetValueOrThrow(this IConfiguration configuration, string key)
    {
        return configuration[key]! ; //?? throw new ConfigurationException($"{key} was not provided in configuration file");
    }
}