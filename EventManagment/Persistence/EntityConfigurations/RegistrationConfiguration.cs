using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class RegistrationConfiguration : IEntityTypeConfiguration<RegistrationEntity>
{
    public void Configure(EntityTypeBuilder<RegistrationEntity> builder)
    {
        builder.ToTable("Registrations");
        builder.HasKey(e => e.Id).HasName("PK__Registra__3214EC07B745E531");

        builder.HasIndex(e => new { e.EventId, e.StatusId }, "IX_Registrations_EventId_StatusId");

        builder.HasIndex(e => new { e.EventId, e.UserId }, "UQ_Registrations_EventUser_Active")
            .IsUnique()
            .HasFilter("([StatusId]>(3))");

        builder.Property(e => e.RegisteredAt).HasDefaultValueSql("(getutcdate())");

        builder.HasOne(d => d.EventEntity).WithMany(p => p.Registrations)
            .HasForeignKey(d => d.EventId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Registrat__Event__5070F446");

        builder.HasOne(d => d.StatusEntity).WithMany(p => p.Registrations)
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Registrat__Statu__52593CB8");

        builder.HasOne(d => d.UserEntity).WithMany(p => p.Registrations)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Registrat__UserI__5165187F");

        builder.HasData(
            // Confirmed registrations (match RegisteredUsers = 1)
            new RegistrationEntity
            {
                Id = 1,
                EventId = 1,
                UserId = 3,
                StatusId = 4, // Confirmed
                RegisteredAt = new DateTime(2025, 9, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new RegistrationEntity
            {
                Id = 2,
                EventId = 2,
                UserId = 4,
                StatusId = 4, // Confirmed
                RegisteredAt = new DateTime(2025, 11, 20, 10, 0, 0, DateTimeKind.Utc)
            },
            new RegistrationEntity
            {
                Id = 3,
                EventId = 4,
                UserId = 3,
                StatusId = 4, // Confirmed
                RegisteredAt = new DateTime(2025, 11, 10, 10, 0, 0, DateTimeKind.Utc)
            },
            new RegistrationEntity
            {
                Id = 4,
                EventId = 6,
                UserId = 4,
                StatusId = 4, // Confirmed
                RegisteredAt = new DateTime(2025, 12, 10, 10, 0, 0, DateTimeKind.Utc)
            }   ,

            // Waitlisted
            new RegistrationEntity
            {
                Id = 5,
                EventId = 2,
                UserId = 3,
                StatusId = 2, // Waitlisted
                RegisteredAt = new DateTime(2025, 11, 21, 10, 0, 0, DateTimeKind.Utc)
            },

            // Cancelled
            new RegistrationEntity
            {
                Id = 6,
                EventId = 1,
                UserId = 4,
                StatusId = 3, // Cancelled
                RegisteredAt = new DateTime(2025, 8, 25, 10, 0, 0, DateTimeKind.Utc),
                CancelledAt = new DateTime(2025, 8, 28, 10, 0, 0, DateTimeKind.Utc)
            }
        );

    }
}