using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.EntityConfigurations;

public class RegistrationStatusConfiguration : IEntityTypeConfiguration<RegistrationStatus>
{
    public void Configure(EntityTypeBuilder<RegistrationStatus> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Registra__3214EC077CE0B27D");

        builder.HasIndex(e => e.Name, "UQ__Registra__737584F6E6264F98").IsUnique();

        builder.Property(e => e.Description).HasMaxLength(200);
        builder.Property(e => e.Name).HasMaxLength(50);
        
    }
}