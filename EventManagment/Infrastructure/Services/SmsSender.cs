using Infrastructure.models;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Services;

public sealed class SmsSender :  ISmsSender
{
    private readonly TwilioOptions _options;

    public SmsSender(IOptions<TwilioOptions> options)
    {
        _options = options.Value;
        TwilioClient.Init(_options.AccountSid, _options.AuthToken);
    }

    public async Task SendAsync(string phone, string message, CancellationToken ct = default)
    {
        await MessageResource.CreateAsync(
            to: new PhoneNumber(phone),
            from: new PhoneNumber(_options.FromPhone),
            body: message
        );
    }
}