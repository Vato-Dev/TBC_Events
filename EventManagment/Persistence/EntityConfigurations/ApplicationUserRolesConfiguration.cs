using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.IdentityModels;

namespace Persistence.EntityConfigurations;

public class ApplicationUserRolesConfiguration  : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole { Id = 1, Name = "Employee" , NormalizedName = "EMPLOYEE"},
            new ApplicationRole { Id = 2, Name = "Organizer" , NormalizedName = "ORGANIZER"},
            new ApplicationRole { Id = 3, Name = "Admin" , NormalizedName = "ADMIN" });
    }
}