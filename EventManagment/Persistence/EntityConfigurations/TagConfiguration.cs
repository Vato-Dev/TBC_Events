using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.EntityConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Tags__3214EC07845EC1D8");

        builder.HasIndex(e => e.Name, "UQ__Tags__737584F69205C21B").IsUnique();

        builder.Property(e => e.Category).HasMaxLength(50);
        builder.Property(e => e.Name).HasMaxLength(50);
        
    }
}