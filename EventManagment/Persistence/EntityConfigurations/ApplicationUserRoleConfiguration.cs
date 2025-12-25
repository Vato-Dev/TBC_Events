using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ApplicationUserRoleConfiguration
    : IEntityTypeConfiguration<IdentityUserRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
    {
        builder.HasData(
            new IdentityUserRole<int> { UserId = 1, RoleId = 3 }, // Admin
            new IdentityUserRole<int> { UserId = 2, RoleId = 2 }, // Organizer
            new IdentityUserRole<int> { UserId = 3, RoleId = 1 }, // Employee
            new IdentityUserRole<int> { UserId = 4, RoleId = 1 }  // Employee
        );
    }
}
