using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class RoleConfiguration :  IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Roles");
        
        builder.HasKey(e => e.Id).HasName("PK__Roles__3214EC07C792DF41");

        builder.HasIndex(e => e.Name, "UQ__Roles__737584F654656A52").IsUnique();

        builder.Property(e => e.Description).HasMaxLength(200);
        builder.Property(e => e.Name).HasMaxLength(50);
    }
}