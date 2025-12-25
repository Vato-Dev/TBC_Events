using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class AgendaItemConfiguration 
    : IEntityTypeConfiguration<AgendaItemEntity>
{
    public void Configure(EntityTypeBuilder<AgendaItemEntity> builder)
    {
        builder.ToTable("AgendaItems");

        builder.HasKey(a => a.Id);
        
        builder.HasOne(a => a.Event)
            .WithMany(e => e.Agendas)
            .HasForeignKey(a => a.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Tracks)
            .WithOne(t => t.AgendaItem)
            .HasForeignKey(t => t.AgendaItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => new { a.EventId, a.StartTime });

        builder.HasData(
            // Event 1: Past Workshop: EF Core Basics
            new
            {
                Id = 101,
                EventId = 1,
                StartTime = new TimeOnly(10, 00),
                Duration = TimeSpan.FromMinutes(15),
                Title = "Registration & Coffee",
                Description = "Pick up materials and settle in.",
                Type = AgendaItemType.Activity,
                Location = "Lobby"
            },
            new
            {
                Id = 102,
                EventId = 1,
                StartTime = new TimeOnly(10, 15),
                Duration = TimeSpan.FromMinutes(45),
                Title = "EF Core Fundamentals",
                Description = "DbContext, tracking, and basic mappings.",
                Type = AgendaItemType.Keynote,
                Location = "Room 4"
            },
            new
            {
                Id = 103,
                EventId = 1,
                StartTime = new TimeOnly(11, 05),
                Duration = TimeSpan.FromMinutes(50),
                Title = "Hands-on Lab",
                Description = "Build a small model + migrations + queries.",
                Type = AgendaItemType.Workshop,
                Location = "Room 4"
            },

            // Event 2: GraphQL Meetup: APIs in Practice
            new
            {
                Id = 201,
                EventId = 2,
                StartTime = new TimeOnly(17, 00),
                Duration = TimeSpan.FromMinutes(20),
                Title = "Welcome & Networking",
                Description = "Snacks + meet the community.",
                Type = AgendaItemType.Activity,
                Location = "Main Hall"
            },
            new
            {
                Id = 202,
                EventId = 2,
                StartTime = new TimeOnly(17, 20),
                Duration = TimeSpan.FromMinutes(40),
                Title = "Talks: GraphQL in Production",
                Description = "Short talks from multiple speakers.",
                Type = AgendaItemType.Panel,
                Location = "Main Hall"
            },
            new
            {
                Id = 203,
                EventId = 2,
                StartTime = new TimeOnly(18, 10),
                Duration = TimeSpan.FromMinutes(40),
                Title = "Open Q&A",
                Description = "Bring your questions.",
                Type = AgendaItemType.Panel,
                Location = "Main Hall"
            },

            // Event 3: Virtual: Clean Architecture Q&A
            new
            {
                Id = 301,
                EventId = 3,
                StartTime = new TimeOnly(16, 00),
                Duration = TimeSpan.FromMinutes(60),
                Title = "Clean Architecture Q&A",
                Description = "Live Q&A + examples.",
                Type = AgendaItemType.Panel,
                Location = "Online"
            },
            new
            {
                Id = 302,
                EventId = 3,
                StartTime = new TimeOnly(17, 00),
                Duration = TimeSpan.FromMinutes(30),
                Title = "Wrap-up & Resources",
                Description = "Links and next steps.",
                Type = AgendaItemType.Ceremony,
                Location = "Online"
            },

            // Event 4: Hybrid: Docker for .NET Developers
            new
            {
                Id = 401,
                EventId = 4,
                StartTime = new TimeOnly(09, 00),
                Duration = TimeSpan.FromMinutes(30),
                Title = "Intro + Setup",
                Description = "Verify Docker desktop + repo clone.",
                Type = AgendaItemType.Keynote,
                Location = "Room 7 / Stream"
            },
            new
            {
                Id = 402,
                EventId = 4,
                StartTime = new TimeOnly(09, 30),
                Duration = TimeSpan.FromMinutes(120),
                Title = "Hands-on Workshop",
                Description = "Containers, compose, debugging.",
                Type = AgendaItemType.Workshop,
                Location = "Room 7 / Stream"
            },

            // Event 5: Past: SQL Performance Clinic
            new
            {
                Id = 501,
                EventId = 5,
                StartTime = new TimeOnly(13, 00),
                Duration = TimeSpan.FromMinutes(60),
                Title = "Query Tuning Clinic",
                Description = "Read plans, fix slow queries.",
                Type = AgendaItemType.Workshop,
                Location = "Room 3"
            },
            new
            {
                Id = 502,
                EventId = 5,
                StartTime = new TimeOnly(14, 00),
                Duration = TimeSpan.FromMinutes(60),
                Title = "Indexing Deep Dive",
                Description = "Index strategy and pitfalls.",
                Type = AgendaItemType.Keynote,
                Location = "Room 3"
            },

            // Event 6: Virtual: Unit Testing Masterclass
            new
            {
                Id = 601,
                EventId = 6,
                StartTime = new TimeOnly(15, 00),
                Duration = TimeSpan.FromMinutes(90),
                Title = "Unit Testing Patterns",
                Description = "xUnit, Moq, and common patterns.",
                Type = AgendaItemType.Workshop,
                Location = "Online"
            },
            new
            {
                Id = 602,
                EventId = 6,
                StartTime = new TimeOnly(16, 30),
                Duration = TimeSpan.FromMinutes(90),
                Title = "Hands-on Lab",
                Description = "Refactor tests + coverage improvements.",
                Type = AgendaItemType.Workshop,
                Location = "Online"
            },

            // Event 7: In-Person: Hack Night
            new
            {
                Id = 701,
                EventId = 7,
                StartTime = new TimeOnly(18, 00),
                Duration = TimeSpan.FromMinutes(30),
                Title = "Kickoff & Team Forming",
                Description = "Pitch ideas and form groups.",
                Type = AgendaItemType.Activity,
                Location = "Community Space"
            },
            new
            {
                Id = 702,
                EventId = 7,
                StartTime = new TimeOnly(18, 30),
                Duration = TimeSpan.FromMinutes(150),
                Title = "Build Session",
                Description = "Ship something small.",
                Type = AgendaItemType.Activity,
                Location = "Community Space"
            },

            // Event 8: Hybrid: Cloud Deployment Workshop
            new
            {
                Id = 801,
                EventId = 8,
                StartTime = new TimeOnly(10, 00),
                Duration = TimeSpan.FromMinutes(45),
                Title = "CI/CD Overview",
                Description = "Pipeline basics and environments.",
                Type = AgendaItemType.Keynote,
                Location = "Cloud Lab / Stream"
            },
            new
            {
                Id = 802,
                EventId = 8,
                StartTime = new TimeOnly(10, 45),
                Duration = TimeSpan.FromMinutes(195),
                Title = "Deploy Walkthrough",
                Description = "Deploy end-to-end.",
                Type = AgendaItemType.Workshop,
                Location = "Cloud Lab / Stream"
            }
        );
    }
}
