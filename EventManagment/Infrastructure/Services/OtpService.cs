using System.Security.Cryptography;
using Application.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Services;

public sealed class OtpService : IOtpService
{
    private readonly IDistributedCache _cache;

    public OtpService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<string> GenerateOtpAsync(string phoneNumber)
    {

        var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");

        await _cache.SetStringAsync(
            $"otp:{phoneNumber}",
            code,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

        return code;
    }

    public async Task<bool> ValidateOtpAsync(string phoneNumber, string code)
    {
        var cachedCode = await _cache.GetStringAsync($"otp:{phoneNumber}");
        if (cachedCode == code)
        {
            await _cache.RemoveAsync($"otp:{phoneNumber}");
            return true;
        }

        return false;
    }
}