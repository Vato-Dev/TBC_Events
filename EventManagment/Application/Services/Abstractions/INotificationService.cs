namespace Application.Services.Abstractions;

public interface INotificationService
{
    Task NotifyParticipantsAboutUpdate(int eventId, string eventTitle);
}