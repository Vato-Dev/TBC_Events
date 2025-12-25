using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class EventTypeConfiguration : IEntityTypeConfiguration<EventTypeEntity>
{

    public void Configure(EntityTypeBuilder<EventTypeEntity> builder)
    {
        builder.ToTable("EventTypes");

        builder.HasKey(e => e.Id).HasName("PK__EventTyp__3214EC0726DF4889");

        builder.HasIndex(e => e.Name, "UQ__EventTyp__737584F6755CCC5F").IsUnique();

        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.Name).HasMaxLength(100);

        builder.HasData(
            new EventTypeEntity { Id = 1, Name = "Workshop", Description = "Hands-on session", IsActive = true },
            new EventTypeEntity { Id = 2, Name = "Meetup", Description = "Community meetup", IsActive = true },
            new EventTypeEntity { Id = 3, Name = "Conference", Description = "Large multi-session event", IsActive = true },
            new EventTypeEntity { Id = 4, Name = "Webinar", Description = "Online session", IsActive = true },
            new EventTypeEntity { Id = 5, Name = "Team Building", Description = "Internal team activity", IsActive = true }
        );
    }
}