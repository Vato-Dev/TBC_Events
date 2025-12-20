using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Data;

public class AppDbContext : DbContext
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

    public virtual DbSet<RoleEntity> Roles { get; set; }

    public virtual DbSet<TagEntity> Tags { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

    }
}
