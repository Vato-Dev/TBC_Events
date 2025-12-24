using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(e => e.Id).HasName("PK__Events__3214EC07C821D3B6");

        builder.HasIndex(e => new { e.EventTypeId, e.StartDateTime, e.IsActive }, "IX_Events_EventTypeId_StartDateTime_IsActive");

        builder.HasIndex(e => e.StartDateTime, "IX_Events_StartDateTime_Active").HasFilter("([IsActive]=(1))");

        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
        builder.Property(e => e.ImageUrl).HasMaxLength(500);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.Title).HasMaxLength(200);
        builder.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");
        builder.Property(e => e.RegistrationStart)
            .HasColumnType("date");

        builder.Property(e => e.RegistrationEnd)
            .HasColumnType("date");
        
        builder.OwnsOne(e=>e.Location,
            l =>
            {
                l.OwnsOne(loc=>loc.Address);
                l.OwnsOne(loc => loc.Address, 
                    address => address.HasIndex(i=> new{ i.VenueName , i.City}));
            });
        builder.HasOne(d => d.CreatedBy).WithMany(p => p.Events)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Events__CreatedB__4D94879B");

        builder.HasOne(d => d.EventTypeEntity).WithMany(p => p.Events)
            .HasForeignKey(d => d.EventTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Events__EventTyp__4CA06362");

        builder.HasData(
            new EventEntity
            {
                Id = 1,
                Title = "Past Workshop: EF Core Basics",
                Description = "Introductory workshop (past event)",
                StartDateTime = new DateTime(2025, 9, 10, 10, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2025, 9, 10, 12, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2025, 8, 20),
                RegistrationEnd = new DateOnly(2025, 9, 9),
                Capacity = 10,
                RegisteredUsers = 1,
                IsActive = true,
                CreatedAt = new DateTime(2025, 8, 1, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 1, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2
            },

            new EventEntity
            {
                Id = 2,
                Title = "Networking Lunch",
                Description = "Casual networking + food",
                StartDateTime = new DateTime(2025, 12, 5, 12, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2025, 12, 5, 13, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2025, 11, 15),
                RegistrationEnd = new DateOnly(2025, 12, 4),
                Capacity = 30,
                RegisteredUsers = 1,
                IsActive = true,
                CreatedAt = new DateTime(2025, 11, 1, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 11, 1, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 2,
                CreatedById = 2
            },

            new EventEntity
            {
                Id = 3,
                Title = "Team Building Walk",
                Description = "Outdoor team-building activity",
                StartDateTime = new DateTime(2026, 1, 10, 9, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 1, 10, 11, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2025, 12, 15),
                RegistrationEnd = new DateOnly(2026, 1, 9),
                Capacity = 30,
                RegisteredUsers = 0,
                IsActive = true,
                CreatedAt = new DateTime(2025, 12, 1, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 12, 1, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 4,
                CreatedById = 2
            },

            new EventEntity
            {
                Id = 4,
                Title = "Security Awareness Training",
                Description = "Mandatory internal training",
                StartDateTime = new DateTime(2025, 11, 25, 10, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2025, 11, 25, 11, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2025, 11, 1),
                RegistrationEnd = new DateOnly(2025, 11, 24),
                Capacity = 100,
                RegisteredUsers = 1,
                IsActive = true,
                CreatedAt = new DateTime(2025, 10, 20, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 20, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 3,
                CreatedById = 2
            },

            new EventEntity
            {
                Id = 5,
                Title = "Lunch & Learn: Clean Code",
                Description = "Best practices discussion",
                StartDateTime = new DateTime(2026, 2, 3, 12, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 2, 3, 13, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2026, 1, 10),
                RegistrationEnd = new DateOnly(2026, 2, 2),
                Capacity = 30,
                RegisteredUsers = 0,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 1, 1, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2
            },

            new EventEntity
            {
                Id = 6,
                Title = "Wellness Morning Yoga",
                Description = "Optional wellness session",
                StartDateTime = new DateTime(2025, 12, 20, 8, 30, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2025, 12, 20, 9, 30, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2025, 12, 1),
                RegistrationEnd = new DateOnly(2025, 12, 19),
                Capacity = 10,
                RegisteredUsers = 1,
                IsActive = true,
                CreatedAt = new DateTime(2025, 11, 20, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 11, 20, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 2,
                CreatedById = 2
            },

            new EventEntity
            {
                Id = 7,
                Title = "Product Roadmap Presentation",
                Description = "Upcoming features overview",
                StartDateTime = new DateTime(2026, 3, 5, 14, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 3, 5, 15, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2026, 2, 10),
                RegistrationEnd = new DateOnly(2026, 3, 4),
                Capacity = 100,
                RegisteredUsers = 0,
                IsActive = true,
                CreatedAt = new DateTime(2026, 2, 1, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 2, 1, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 3,
                CreatedById = 2
            },

            new EventEntity
            {
                Id = 8,
                Title = "Community Volunteering Day",
                Description = "Giving back together",
                StartDateTime = new DateTime(2025, 10, 15, 9, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2025, 10, 15, 13, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2025, 9, 20),
                RegistrationEnd = new DateOnly(2025, 10, 14),
                Capacity = 30,
                RegisteredUsers = 0,
                IsActive = true,
                CreatedAt = new DateTime(2025, 9, 1, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 1, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 4,
                CreatedById = 2
            }
        );
    }
}