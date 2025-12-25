using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class RegistrationStatusConfiguration : IEntityTypeConfiguration<RegistrationStatusEntity>
{
    public void Configure(EntityTypeBuilder<RegistrationStatusEntity> builder)
    {
        builder.ToTable("RegistrationStatuses");
        
        builder.HasKey(e => e.Id).HasName("PK__Registra__3214EC077CE0B27D");

        builder.HasIndex(e => e.Name, "UQ__Registra__737584F6E6264F98").IsUnique();

        builder.Property(e => e.Description).HasMaxLength(200);
        builder.Property(e => e.Name).HasMaxLength(50);

        builder.HasData(
            new RegistrationStatusEntity { Id = 1, Name = "Pending", Description = "Awaiting approval" },
            new RegistrationStatusEntity { Id = 2, Name = "Waitlisted", Description = "No spots currently available" },
            new RegistrationStatusEntity { Id = 3, Name = "Cancelled", Description = "Registration cancelled" },
            new RegistrationStatusEntity { Id = 4, Name = "Confirmed", Description = "Spot confirmed" }
        );

    }
}