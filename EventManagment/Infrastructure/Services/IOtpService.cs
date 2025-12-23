using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Services;

public interface IOtpService
{
    Task<string> GenerateOtpAsync(string phoneNumber);
    Task<bool> ValidateOtpAsync(string phoneNumber, string code);
    
}