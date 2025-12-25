using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection collection)
    {
        collection.AddScoped<IEventRepository, EventRepository>();
        collection.AddScoped<IUserRepository, UserRepository>();
        collection.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

        return collection;
    }
}