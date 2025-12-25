using Infrastructure.BackGroundJobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

public static class ConfigureBackgroundJobs 
{
    public static void AddQuaertzJobs(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            config.UseMicrosoftDependencyInjectionJobFactory(); 

            var interval = TimeSpan.FromMinutes(15);

            config
                .AddJob<NotifyJob>(opts => opts.WithIdentity(NotifyJob.Key))
                .AddTrigger(opts => opts
                    .ForJob(NotifyJob.Key)
                    .WithIdentity("Notify_Events_Trigger")
                    .WithSimpleSchedule(x => x
                        .WithInterval(interval)
                        .RepeatForever()));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}