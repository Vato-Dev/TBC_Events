using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("DomainUsers");

        builder.HasKey(e => e.Id).HasName("PK__Users__3214EC07F44ED148");

        builder.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

        builder.HasIndex(e => e.Email, "UQ__Users__A9D10534D7C521D4").IsUnique();

        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
        builder.Property(e => e.Email).HasMaxLength(256);
        builder.Property(e => e.FullName).HasMaxLength(200);
        builder.Property(e => e.IsActive).HasDefaultValue(true);

        
        builder.Property(e => e.Department).HasConversion<int>().IsRequired();
       //     .HasConversion(to => to.ToString(), from => Enum.Parse<Department>(from, ignoreCase: true)); not safe 

       builder.Property(e => e.Role).HasConversion<int>().IsRequired().HasDefaultValue(UserRole.Employee);;
        
        builder.HasOne(u => u.ApplicationUser)
            .WithOne()
            .HasForeignKey<UserEntity>(u => u.Id);

        var seedTime = new DateTime(2025, 12, 24, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new UserEntity
            {
                Id = 1,
                Email = "admin@demo.com",
                FullName = "Demo Admin",
                IsActive = true,
                Role = UserRole.Admin,
                Department = Department.Engineering,
                CreatedAt = seedTime
            },
            new UserEntity
            {
                Id = 2,
                Email = "organizer@demo.com",
                FullName = "Demo Organizer",
                IsActive = true,
                Role = UserRole.Organizer,
                Department = Department.HR,
                CreatedAt = seedTime
            },
            new UserEntity
            {
                Id = 3,
                Email = "employee1@demo.com",
                FullName = "Demo Employee 1",
                IsActive = true,
                Role = UserRole.Employee,
                Department = Department.Design,
                CreatedAt = seedTime
            },
            new UserEntity
            {
                Id = 4,
                Email = "employee2@demo.com",
                FullName = "Demo Employee 2",
                IsActive = true,
                Role = UserRole.Employee,
                Department = Department.Marketing,
                CreatedAt = seedTime
            }
        );

    }
}