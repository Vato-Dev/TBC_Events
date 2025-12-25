using Application.DTOs;
using Application.Services.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using Quartz;

namespace Infrastructure.BackGroundJobs;

[DisallowConcurrentExecution]//Some guy from C# community suggested
public class NotifyJob(AppDbContext repository,IEmailSender emailSender) : IJob
{
    public static readonly JobKey Key = new JobKey("Notify_Events_Bgining");
    public const string ConfirmedStatus =  "Confirmed";
    public const string WailingStatus =  "Waitlisted";
    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTime.Now;
        var upcomingEvents = await repository.Events
            .Where(e => e.StartDateTime > now && e.StartDateTime <= now.AddHours(24))
            .Where(e => (e.NotificationSettings & (NotificationSettings.OneHourBeforeReminder | NotificationSettings.TwentyForHourBeforeReminder)) != 0)
            .Include(e => e.Registrations)
            .ThenInclude(r => r.UserEntity) 
            .Include(e => e.Registrations)
            .ThenInclude(r => r.StatusEntity)
            .ToListAsync(context.CancellationToken);
        
        foreach (var ev in upcomingEvents)
        {
            TimeSpan timeToStart = ev.StartDateTime - now;

            if (ev.NotificationSettings.HasFlag(NotificationSettings.TwentyForHourBeforeReminder))
            {
                if (timeToStart <= TimeSpan.FromHours(24) && timeToStart > TimeSpan.FromHours(23))
                {
                    await SendNotifications(ev, "Reminder: Event starts in 24 hours!");
                }
            }

            if (ev.NotificationSettings.HasFlag(NotificationSettings.OneHourBeforeReminder))
            {
                if (timeToStart <= TimeSpan.FromHours(1) && timeToStart > TimeSpan.FromMinutes(0))
                {
                    await SendNotifications(ev, "Reminder: Event starts in 1 hour!");
                }
            }
        }
    }

    private async Task SendNotifications(EventEntity ev, string subject)
    {
        var recipients = ev.Registrations
            .Where(r => r.StatusEntity.Name.Equals("Confirmed", StringComparison.OrdinalIgnoreCase))
            .Select(r => r.UserEntity.Email)
            .ToList();

        foreach (var email in recipients)
        {
            await emailSender.SendEmailAsync(new EmailData
                { EmailToName = email, EmailSubject = subject, Message = $"Event {ev.Title} is starting soon!"});
        }
    }
}
