using Microsoft.Extensions.DependencyInjection;
using Quartz;
namespace Infrastructure.BackGroundJobs;

public static class ConfigureBackgroundJobs 
{
    public static void AddQuaertzJobs(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var interval = TimeSpan.FromMinutes(15);

            config
                .AddJob<NotifyJob>(NotifyJob.Key, job => { job.StoreDurably(); })
                .AddTrigger(trigger => trigger
                    .ForJob(NotifyJob.Key)
                    .WithSimpleSchedule(schedule => schedule
                        .WithInterval(interval)
                        .RepeatForever()));
            config.UseInMemoryStore();

        });
        services.AddQuartzHostedService();
    }
}