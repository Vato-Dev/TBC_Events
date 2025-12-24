using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.IdentityModels;

namespace Persistence.EntityConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Required Identity fields
        builder.Property(u => u.UserName).HasMaxLength(256);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(256);

        // Seed users
        var hasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(
            CreateUser(1, "admin@demo.com", "Admin123!", hasher),
            CreateUser(2, "organizer@demo.com", "Organizer123!", hasher),
            CreateUser(3, "employee1@demo.com", "Employee123!", hasher),
            CreateUser(4, "employee2@demo.com", "Employee123!", hasher)
        );
    }

    private static ApplicationUser CreateUser(
        int id,
        string email,
        string password,
        PasswordHasher<ApplicationUser> hasher)
    {
        var user = new ApplicationUser
        {
            Id = id,
            UserName = email,
            NormalizedUserName = email.ToUpper(),
            Email = email,
            NormalizedEmail = email.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            LastOtpSentTime = null
        };

        user.PasswordHash = hasher.HashPassword(user, password);
        return user;
    }
}
