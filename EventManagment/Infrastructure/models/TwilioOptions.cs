namespace Infrastructure.models;

public sealed class TwilioOptions
{
    public const string SectionName = "Twilio";
    public required string FromPhone { get; init; }
}   