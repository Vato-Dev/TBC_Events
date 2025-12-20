using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.EntityConfigurations;

public class RoleConfiguration :  IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Roles__3214EC07C792DF41");

        builder.HasIndex(e => e.Name, "UQ__Roles__737584F654656A52").IsUnique();

        builder.Property(e => e.Description).HasMaxLength(200);
        builder.Property(e => e.Name).HasMaxLength(50);
    }
}