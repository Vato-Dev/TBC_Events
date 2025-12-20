using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.EntityConfigurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Events__3214EC07C821D3B6");

        builder.HasIndex(e => new { e.EventTypeId, e.StartDateTime, e.IsActive }, "IX_Events_EventTypeId_StartDateTime_IsActive");

        builder.HasIndex(e => e.StartDateTime, "IX_Events_StartDateTime_Active").HasFilter("([IsActive]=(1))");

        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
        builder.Property(e => e.ImageUrl).HasMaxLength(500);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.Location).HasMaxLength(200);
        builder.Property(e => e.Title).HasMaxLength(200);
        builder.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");
        

        builder.HasOne(d => d.CreatedBy).WithMany(p => p.Events)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Events__CreatedB__4D94879B");

        builder.HasOne(d => d.EventType).WithMany(p => p.Events)
            .HasForeignKey(d => d.EventTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Events__EventTyp__4CA06362");
        
    }
}