using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class AgendaTrackConfiguration 
    : IEntityTypeConfiguration<AgendaTrackEntity>
{
    public void Configure(EntityTypeBuilder<AgendaTrackEntity> builder)
    {
        builder.ToTable("AgendaTracks");
        builder.HasOne(t => t.AgendaItem)
            .WithMany(a => a.Tracks)
            .HasForeignKey(t => t.AgendaItemId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasKey(t => t.Id);

        builder.HasData(
    // Event 1 – EF Core Basics
    new
    {
        Id = 1001,
        AgendaItemId = 103,
        Title = "Track A: Migrations",
        Speaker = "Beka Ghvaberidze",
        Room = "Room 4"
    },
    new
    {
        Id = 1002,
        AgendaItemId = 103,
        Title = "Track B: LINQ Queries",
        Speaker = "Giorgi Tamarashvili",
        Room = "Room 4"
    },

    // Event 2 – GraphQL Meetup
    new
    {
        Id = 2001,
        AgendaItemId = 202,
        Title = "GraphQL Schema Design",
        Speaker = "Lasha Daushvili",
        Room = "Main Hall"
    },
    new
    {
        Id = 2002,
        AgendaItemId = 202,
        Title = "Caching & Performance",
        Speaker = "Dimitri Dondoladze",
        Room = "Main Hall"
    },

    // Event 4 – Docker for .NET Developers
    new
    {
        Id = 4001,
        AgendaItemId = 402,
        Title = "Docker Compose & Environment Variables",
        Speaker = "James Richardson",
        Room = "Room 7 / Stream"
    },
    new
    {
        Id = 4002,
        AgendaItemId = 402,
        Title = "Debugging Containers in .NET",
        Speaker = "Elena Novak",
        Room = "Room 7 / Stream"
    },

    // Event 6 – Unit Testing Masterclass
    new
    {
        Id = 6001,
        AgendaItemId = 601,
        Title = "Mocks vs Fakes",
        Speaker = "Lasha Daushvili",
        Room = "Online"
    },
    new
    {
        Id = 6002,
        AgendaItemId = 602,
        Title = "Refactoring Legacy Tests",
        Speaker = "Beka Ghvaberidze",
        Room = "Online"
    },

    // Event 8 – Cloud Deployment Workshop
    new
    {
        Id = 8001,
        AgendaItemId = 802,
        Title = "CI/CD Pipeline Setup",
        Speaker = "Andrei Popescu",
        Room = "Cloud Lab / Stream"
    },
    new
    {
        Id = 8002,
        AgendaItemId = 802,
        Title = "Deployment & Rollback Strategies",
        Speaker = "Natalie Wong",
        Room = "Cloud Lab / Stream"
    }
);
    }
}
