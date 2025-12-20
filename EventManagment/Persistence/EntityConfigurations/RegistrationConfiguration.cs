using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.EntityConfigurations;

public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Registra__3214EC07B745E531");

        builder.HasIndex(e => new { e.EventId, e.StatusId }, "IX_Registrations_EventId_StatusId");

        builder.HasIndex(e => new { e.EventId, e.UserId }, "UQ_Registrations_EventUser_Active")
            .IsUnique()
            .HasFilter("([StatusId]>(3))");

        builder.Property(e => e.RegisteredAt).HasDefaultValueSql("(getutcdate())");

        builder.HasOne(d => d.Event).WithMany(p => p.Registrations)
            .HasForeignKey(d => d.EventId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Registrat__Event__5070F446");

        builder.HasOne(d => d.Status).WithMany(p => p.Registrations)
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Registrat__Statu__52593CB8");

        builder.HasOne(d => d.User).WithMany(p => p.Registrations)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Registrat__UserI__5165187F");
    }
}