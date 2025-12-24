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
    }
}
