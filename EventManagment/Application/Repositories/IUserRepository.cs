using Domain.Models;

namespace Application.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken);
    Task UpdateUserRoleAsync(int userId, UserRole newRole, CancellationToken ct);
    Task RemoveUserRoleAsync(int userId, CancellationToken ct);
}