namespace Application.Services.Abstractions;

public interface ICurrentUserService
{
    int UserId { get; }
    bool IsAuthenticated { get; }
    IReadOnlyCollection<string> Roles { get; }
}