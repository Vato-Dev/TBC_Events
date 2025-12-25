using System.Security.Claims;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    public int UserId { get; }
    public bool IsAuthenticated { get; }
    public string? UserName { get; }
    public IReadOnlyCollection<string> Roles { get; }

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User;
        if (user == null)
        {
            UserId = 0;
            Roles = [];
            return;
        }

        IsAuthenticated = user?.Identity?.IsAuthenticated == true;

        if (!IsAuthenticated)
        {
            UserId = 0;
            Roles = [];
            return;
        }

        UserId = int.Parse(
            user!.FindFirstValue("Sid")!
        );
        UserName = user!.FindFirstValue("Preferred_name");

        Roles = user!.FindAll("Role")
            .Select(c => c.Value)
            .ToArray();
    }
}