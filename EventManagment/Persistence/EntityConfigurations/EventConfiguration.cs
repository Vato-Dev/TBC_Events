using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;
using Domain.Models;

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

        // -------------------- Events (owners) --------------------
        builder.HasData(
            new
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
                CreatedById = 2,
                ImageUrl = (string?)null
            },
            new
            {
                Id = 2,
                Title = "GraphQL Meetup: APIs in Practice",
                Description = "Community meetup and short talks",
                StartDateTime = new DateTime(2026, 1, 15, 17, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 1, 15, 19, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2025, 12, 20),
                RegistrationEnd = new DateOnly(2026, 1, 14),
                Capacity = 60,
                RegisteredUsers = 1,
                IsActive = true,
                CreatedAt = new DateTime(2025, 12, 10, 10, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 12, 10, 10, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2,
                ImageUrl = "https://pics.example.com/events/graphql.png"
            },
            new
            {
                Id = 3,
                Title = "Virtual: Clean Architecture Q&A",
                Description = "Live Q&A + examples",
                StartDateTime = new DateTime(2026, 2, 5, 16, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 2, 5, 17, 30, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2026, 1, 10),
                RegistrationEnd = new DateOnly(2026, 2, 4),
                Capacity = 200,
                RegisteredUsers = 0,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 5, 11, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 1, 5, 11, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2,
                ImageUrl = (string?)null
            },
            new
            {
                Id = 4,
                Title = "Hybrid: Docker for .NET Developers",
                Description = "Hands-on + streaming",
                StartDateTime = new DateTime(2026, 3, 12, 9, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 3, 12, 12, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2026, 2, 10),
                RegistrationEnd = new DateOnly(2026, 3, 11),
                Capacity = 40,
                RegisteredUsers = 1,
                IsActive = true,
                CreatedAt = new DateTime(2026, 2, 1, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 2, 1, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2,
                ImageUrl = (string?)null
            },
            new
            {
                Id = 5,
                Title = "Past: SQL Performance Clinic",
                Description = "Indexing and query tuning",
                StartDateTime = new DateTime(2025, 10, 22, 13, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2025, 10, 22, 15, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2025, 10, 1),
                RegistrationEnd = new DateOnly(2025, 10, 21),
                Capacity = 25,
                RegisteredUsers = 0,
                IsActive = true,
                CreatedAt = new DateTime(2025, 9, 15, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 1, 12, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2,
                ImageUrl = (string?)null
            },
            new
            {
                Id = 6,
                Title = "Virtual: Unit Testing Masterclass",
                Description = "xUnit + Moq + patterns",
                StartDateTime = new DateTime(2026, 4, 8, 15, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 4, 8, 18, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2026, 3, 10),
                RegistrationEnd = new DateOnly(2026, 4, 7),
                Capacity = 300,
                RegisteredUsers = 1,
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 1, 10, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 3, 1, 10, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2,
                ImageUrl = "https://pics.example.com/events/testing.png"
            },
            new
            {
                Id = 7,
                Title = "In-Person: Hack Night",
                Description = "Bring a project, ship something",
                StartDateTime = new DateTime(2026, 5, 20, 18, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 5, 20, 21, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2026, 4, 20),
                RegistrationEnd = new DateOnly(2026, 5, 19),
                Capacity = 80,
                RegisteredUsers = 0,
                IsActive = true,
                CreatedAt = new DateTime(2026, 4, 1, 8, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 4, 1, 8, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2,
                ImageUrl = (string?)null
            },
            new
            {
                Id = 8,
                Title = "Hybrid: Cloud Deployment Workshop",
                Description = "CI/CD + deploy walkthrough",
                StartDateTime = new DateTime(2026, 6, 11, 10, 0, 0, DateTimeKind.Utc),
                EndDateTime = new DateTime(2026, 6, 11, 14, 0, 0, DateTimeKind.Utc),
                RegistrationStart = new DateOnly(2026, 5, 10),
                RegistrationEnd = new DateOnly(2026, 6, 10),
                Capacity = 50,
                RegisteredUsers = 0,
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 1, 9, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 1, 9, 0, 0, DateTimeKind.Utc),
                EventTypeId = 1,
                CreatedById = 2,
                ImageUrl = "https://pics.example.com/events/cloud.png"
            }
        );

        // -------------------- Location (owned) --------------------
        builder.OwnsOne(e => e.Location).HasData(
            new { EventEntityId = 1, LocationType = LocationType.InPerson, RoomNumber = 4, FloorNumber = 5, AdditionalInformation = "Sigma" },
            new { EventEntityId = 2, LocationType = LocationType.InPerson, RoomNumber = 12, FloorNumber = 2, AdditionalInformation = "Snacks provided" },
            new { EventEntityId = 3, LocationType = LocationType.Virtual, RoomNumber = 0, FloorNumber = 0, AdditionalInformation = "Join link in email" },
            new { EventEntityId = 4, LocationType = LocationType.Hybrid, RoomNumber = 7, FloorNumber = 1, AdditionalInformation = "Streaming available" },
            new { EventEntityId = 5, LocationType = LocationType.InPerson, RoomNumber = 3, FloorNumber = 4, AdditionalInformation = "Bring laptop" },
            new { EventEntityId = 6, LocationType = LocationType.Virtual, RoomNumber = 0, FloorNumber = 0, AdditionalInformation = "Recording will be shared" },
            new { EventEntityId = 7, LocationType = LocationType.InPerson, RoomNumber = 21, FloorNumber = 6, AdditionalInformation = "Open seating" },
            new { EventEntityId = 8, LocationType = LocationType.Hybrid, RoomNumber = 9, FloorNumber = 3, AdditionalInformation = "Workshop materials included" }
        );

        // -------------------- Address (owned inside Location) --------------------
        builder.OwnsOne(e => e.Location)
               .OwnsOne(l => l.Address)
               .HasData(
            new { LocationEntityEventEntityId = 1, VenueName = "Building", Street = "Gerdasd", City = "Tbilisi" },
            new { LocationEntityEventEntityId = 2, VenueName = "Tech Hub", Street = "Rustaveli Ave", City = "Tbilisi" },
            new { LocationEntityEventEntityId = 3, VenueName = "Online", Street = "N/A", City = "Remote" },
            new { LocationEntityEventEntityId = 4, VenueName = "Conference Center", Street = "Chavchavadze", City = "Tbilisi" },
            new { LocationEntityEventEntityId = 5, VenueName = "DB Lab", Street = "Kazbegi Ave", City = "Tbilisi" },
            new { LocationEntityEventEntityId = 6, VenueName = "Online", Street = "N/A", City = "Remote" },
            new { LocationEntityEventEntityId = 7, VenueName = "Community Space", Street = "Tamarashvili", City = "Tbilisi" },
            new { LocationEntityEventEntityId = 8, VenueName = "Cloud Lab", Street = "Pekini Ave", City = "Tbilisi" }
        );

    }
}