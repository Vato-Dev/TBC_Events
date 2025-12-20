using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventTag> EventTags { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<RegistrationStatus> RegistrationStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-AIFM9F2\\SQLEXPRESS;Database=EventsDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events__3214EC07C821D3B6");

            entity.HasIndex(e => new { e.EventTypeId, e.StartDateTime, e.IsActive }, "IX_Events_EventTypeId_StartDateTime_IsActive");

            entity.HasIndex(e => e.StartDateTime, "IX_Events_StartDateTime_Active").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.Events)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__CreatedB__4D94879B");

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__EventTyp__4CA06362");
        });

        modelBuilder.Entity<EventTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventTag__3214EC07F32EF299");

            entity.HasIndex(e => e.TagId, "IX_EventTags_TagId");

            entity.HasIndex(e => new { e.EventId, e.TagId }, "UQ_EventTags").IsUnique();

            entity.HasOne(d => d.Event).WithMany(p => p.EventTags)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventTags__Event__571DF1D5");

            entity.HasOne(d => d.Tag).WithMany(p => p.EventTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventTags__TagId__5812160E");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventTyp__3214EC0726DF4889");

            entity.HasIndex(e => e.Name, "UQ__EventTyp__737584F6755CCC5F").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registra__3214EC07B745E531");

            entity.HasIndex(e => new { e.EventId, e.StatusId }, "IX_Registrations_EventId_StatusId");

            entity.HasIndex(e => new { e.EventId, e.UserId }, "UQ_Registrations_EventUser_Active")
                .IsUnique()
                .HasFilter("([StatusId]>(3))");

            entity.Property(e => e.RegisteredAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Event).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Event__5070F446");

            entity.HasOne(d => d.Status).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Statu__52593CB8");

            entity.HasOne(d => d.User).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__UserI__5165187F");
        });

        modelBuilder.Entity<RegistrationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registra__3214EC077CE0B27D");

            entity.HasIndex(e => e.Name, "UQ__Registra__737584F6E6264F98").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07C792DF41");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F654656A52").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC07845EC1D8");

            entity.HasIndex(e => e.Name, "UQ__Tags__737584F69205C21B").IsUnique();

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07F44ED148");

            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534D7C521D4").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
