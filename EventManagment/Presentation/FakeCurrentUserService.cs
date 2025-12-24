using Application.Services.Abstractions;

namespace Presentation;

public class FakeCurrentUserService : ICurrentUserService
{
    public int UserId => 1; 
    public string UserName => "DevUser";
    public bool IsAuthenticated => true;
    public IReadOnlyCollection<string> Roles { get; } =  new List<string> { "Admin" };
}