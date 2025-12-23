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
    }
}
