using Application.Services.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Quartz;

namespace Infrastructure.BackGroundJobs;

public class NotifyJob(AppDbContext repository,IEmailSender emailSender) : IJob
{
    public static readonly JobKey Key = new JobKey("Notify_Events_Bgining");
    public async Task Execute(IJobExecutionContext context)
    {
        var filter = NotificationSettings.OneHourBeforeReminder | NotificationSettings.TwentyForHourBeforeReminder;
        var eventsToSendMessages = await repository.Events.Where(x => (x.NotificationSettings & filter) != 0)
            .ToListAsync(context.CancellationToken);

        var groups = eventsToSendMessages
            .GroupBy(x => x.NotificationSettings)
            .ToDictionary(g => g.Key, g => g.ToList());
        
        var GetActiveRegistrations = repository.Registrations.Include(x=>x.StatusEntity).Where(x=>x.)

        foreach (var group in groups)
        {
            var notifications = group.Value;
            switch (group.Key)
            {
                
                case NotificationSettings.OneHourBeforeReminder:
                    var sortedListOfEvents24 = notifications.Where(x=> DateTime.UtcNow - x.StartDateTime < TimeSpan.FromHours(1)).ToList();
                    var message = $"Hello dear, We are happy to let you know that "
                    break;
                case NotificationSettings.TwentyForHourBeforeReminder:
                    var sortedListOfEvents1 = notifications.Where(x=> DateTime.UtcNow - x.StartDateTime < TimeSpan.FromHours(24)).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return Task.CompletedTask;
    }
}
