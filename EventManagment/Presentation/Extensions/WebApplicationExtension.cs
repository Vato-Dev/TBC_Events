using Infrastructure.Extensions;
using Persistence.Data;
using Persistence.Extensions;

namespace Presentation.Extensions;

public static class WebApplicationExtension
{
    public static WebApplicationBuilder AddAllIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityServices().AddEntityFrameworkStores<AppDbContext>();
        return builder;
    }

    public static WebApplicationBuilder AddAllRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddRepositories();
        return builder;
    }
}