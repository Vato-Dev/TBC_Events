using Application.DTOs;
using Application.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Infrastructure.Services;

public class NotificationService(AppDbContext dbContext , IEmailSender emailSender ) : INotificationService
{
    public async Task NotifyParticipantsAboutUpdate(int eventId, string eventTitle)
    {
        var recipients = await dbContext.Registrations
            .Where(r => r.EventId == eventId && r.StatusEntity.Name == "Confirmed")
            .Select(r => new { r.UserEntity.Email, r.UserEntity.FullName })
            .ToListAsync();


        foreach (var user in recipients)
        {
            await emailSender.SendEmailAsync(new EmailData
            {
                EmailToName = user.FullName,
                EmailSubject = $"Event Update: {eventTitle}",
                Message = $"Hello Dear User ! Event - '{eventTitle}', you were subscribed at is updated . please check for updates"
            });
        }
    }
}