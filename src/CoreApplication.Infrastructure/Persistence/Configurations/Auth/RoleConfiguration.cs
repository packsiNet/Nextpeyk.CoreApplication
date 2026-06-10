using CoreApplication.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Auth;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("RoleId");
        builder.Property(x => x.RoleName).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.RoleName).IsUnique();

        builder.HasData(
            new Role { Id = 1, RoleName = "Admin", CreatedAt = new DateTime(2025, 1, 1), IsActive = true },
            new Role { Id = 2, RoleName = "CourierManager", CreatedAt = new DateTime(2025, 1, 1), IsActive = true },
            new Role { Id = 3, RoleName = "CourierOperator", CreatedAt = new DateTime(2025, 1, 1), IsActive = true },
            new Role { Id = 4, RoleName = "Driver", CreatedAt = new DateTime(2025, 1, 1), IsActive = true },
            new Role { Id = 5, RoleName = "Sender", CreatedAt = new DateTime(2025, 1, 1), IsActive = true }
        );
    }
}
