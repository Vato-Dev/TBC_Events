namespace Infrastructure.models;

public sealed class EmailSenderOptions
{
    public const string EmailSenderSettings = "MailSettings";
    public required string IssuerName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string SmtpHost { get; set; }
    public required int Port { get; set; }
}