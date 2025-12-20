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
        builder.Property(e => e.Name).HasMaxLength(100);    }
}