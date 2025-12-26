using Domain.Extensions;
using Infrastructure.models;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Services;

public sealed class SmsSender :  ISmsSender
{
    private readonly TwilioOptions _options;
    private static readonly string TwilioKey = "TWILIO__KEY".FromEnvRequired();
    private static readonly string TwilioAcc = "TWILIO__SID".FromEnvRequired();
    public SmsSender(IOptions<TwilioOptions> options)
    {
        _options = options.Value;
        TwilioClient.Init(TwilioAcc,TwilioKey);
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