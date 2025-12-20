using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id).HasName("PK__Users__3214EC07F44ED148");

        builder.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

        builder.HasIndex(e => e.Email, "UQ__Users__A9D10534D7C521D4").IsUnique();

        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
        builder.Property(e => e.Email).HasMaxLength(256);
        builder.Property(e => e.FullName).HasMaxLength(200);
        builder.Property(e => e.IsActive).HasDefaultValue(true);

        builder.HasOne(d => d.RoleEntity).WithMany(p => p.Users)
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Users__RoleId__44FF419A");    }
}