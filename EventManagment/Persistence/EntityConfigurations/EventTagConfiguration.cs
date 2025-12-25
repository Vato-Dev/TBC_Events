using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class EventTagConfiguration : IEntityTypeConfiguration<EventTagEntity>
{
    public void Configure(EntityTypeBuilder<EventTagEntity> builder)
    {
        builder.ToTable("EventTags");
        
        builder.HasKey(e => e.Id).HasName("PK__EventTag__3214EC07F32EF299");

        builder.HasIndex(e => e.TagId, "IX_EventTags_TagId");

        builder.HasIndex(e => new { e.EventId, e.TagId }, "UQ_EventTags").IsUnique();

        builder.HasOne(d => d.EventEntity).WithMany(p => p.EventTags)
            .HasForeignKey(d => d.EventId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__EventTags__Event__571DF1D5");

        builder.HasOne(d => d.TagEntity).WithMany(p => p.EventTags)
            .HasForeignKey(d => d.TagId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__EventTags__TagId__5812160E");

        builder.HasData(
            new EventTagEntity { Id = 1, EventId = 1, TagId = 5 }, // tech
            new EventTagEntity { Id = 2, EventId = 2, TagId = 3 }, // free-food
            new EventTagEntity { Id = 3, EventId = 2, TagId = 4 }  // networking
        );

    }
}