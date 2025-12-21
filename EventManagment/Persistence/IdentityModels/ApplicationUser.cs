
using Microsoft.AspNetCore.Identity;

namespace Persistence.IdentityModels;

public class ApplicationUser : IdentityUser<int>
{
    public DateTime? LastOtpSentTime { get; set; }
}