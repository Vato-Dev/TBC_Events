using Application.Services.Abstractions;
using Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();

        return services;
    }
}