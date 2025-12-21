using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.IdentityModels;

namespace Persistence.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EventEntity> Events { get; set; }

    public virtual DbSet<EventTagEntity> EventTags { get; set; }

    public virtual DbSet<EventTypeEntity> EventTypes { get; set; }

    public virtual DbSet<RegistrationEntity> Registrations { get; set; }

    public virtual DbSet<RegistrationStatusEntity> RegistrationStatuses { get; set; }
    
    public virtual DbSet<TagEntity> Tags { get; set; }

    public virtual DbSet<UserEntity> DomainUsers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

    }
}
