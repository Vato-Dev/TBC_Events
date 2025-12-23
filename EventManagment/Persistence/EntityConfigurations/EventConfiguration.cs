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

    }
}