using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.EntityConfigurations;

public class EventTagConfiguration : IEntityTypeConfiguration<EventTag>
{
    public void Configure(EntityTypeBuilder<EventTag> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__EventTag__3214EC07F32EF299");

        builder.HasIndex(e => e.TagId, "IX_EventTags_TagId");

        builder.HasIndex(e => new { e.EventId, e.TagId }, "UQ_EventTags").IsUnique();

        builder.HasOne(d => d.Event).WithMany(p => p.EventTags)
            .HasForeignKey(d => d.EventId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__EventTags__Event__571DF1D5");

        builder.HasOne(d => d.Tag).WithMany(p => p.EventTags)
            .HasForeignKey(d => d.TagId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__EventTags__TagId__5812160E");    }
}