using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.models;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using Persistence.Extensions;

namespace Presentation.Extensions;

public static class WebApplicationExtension
{
    public static WebApplicationBuilder AddEmailSenders(this WebApplicationBuilder builder)
    {
        builder.Services.AddEmailWithOtpService()
            .Configure<TwilioOptions>(builder.Configuration.GetSection(TwilioOptions.SectionName))
            .Configure<EmailSenderOptions>(builder.Configuration.GetSection(EmailSenderOptions.EmailSenderSettings));
        return builder;
    }

    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices();
        return builder;
    }
    public static WebApplicationBuilder AddAllIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddTokenService().Configure<JwtServiceOptions>(builder.Configuration.GetSection(JwtServiceOptions.Authentication));
        builder.Services.AddIdentityServices().AddEntityFrameworkStores<AppDbContext>();
        return builder;
    }

    public static WebApplicationBuilder AddAllRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddRepositories();
        return builder;
    }
}