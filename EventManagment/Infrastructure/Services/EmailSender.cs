using Application.DTOs;
using Application.Services;
using Application.Services.Abstractions;
using Infrastructure.models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services;

public sealed class EmailSender : IEmailSender
{
    private readonly EmailSenderOptions _options;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(
        IOptions<EmailSenderOptions> options,
        ILogger<EmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailData email)
    {
        try
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(
                new MailboxAddress(_options.IssuerName, _options.UserName));

            emailMessage.To.Add(
                new MailboxAddress(email.EmailToName, email.EmailToName));

            emailMessage.Subject = email.EmailSubject;

            emailMessage.Body = new BodyBuilder
            {
                TextBody = email.Message
            }.ToMessageBody();

            using var smtpClient = new SmtpClient();

            await smtpClient.ConnectAsync(
                _options.SmtpHost,
                _options.Port,
                SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(
                _options.UserName,
                _options.Password);

            await smtpClient.SendAsync(emailMessage);
            await smtpClient.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending email");
            throw;
        }
    }
}