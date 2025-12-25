namespace Application.Services.Abstractions;

public interface ICurrentUserService
{
    int UserId { get; }
    bool IsAuthenticated { get; }
    string? UserName { get; }
    IReadOnlyCollection<string> Roles { get; }
}