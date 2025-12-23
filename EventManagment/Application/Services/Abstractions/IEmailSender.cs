using Application.DTOs;

namespace Application.Services.Abstractions;

public interface IEmailSender
{
    Task SendEmailAsync(EmailData email);
}