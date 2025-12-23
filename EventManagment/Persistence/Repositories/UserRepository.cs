using Application.Repositories;
using Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;

namespace Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        var entity = user.Adapt<UserEntity>();
        await context.DomainUsers.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserRoleAsync(int userId, UserRole newRole, CancellationToken ct)
    {
        await context.DomainUsers
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.Role, newRole), ct);
    }

    public async Task RemoveUserRoleAsync(int userId, CancellationToken ct)
    {
        await context.DomainUsers
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(u => u.Role,
                    UserRole.Employee), //Because we have only one role per user now , admin has rights by default (it's not very correct) just time...
                ct);
    }
}