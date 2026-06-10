using CoreApplication.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Auth;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("UserAccount");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("UserAccountId");
        builder.Property(x => x.UserName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Password).HasMaxLength(256).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
        builder.Property(x => x.NationalCode).HasMaxLength(20);
        builder.Property(x => x.SecurityStamp).HasMaxLength(256);
        builder.Property(x => x.CreatedByIp).HasMaxLength(50);
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasIndex(x => x.UserName).IsUnique();

        builder.HasOne(x => x.Courier)
            .WithMany(x => x.UserAccounts)
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Sender)
            .WithMany(x => x.UserAccounts)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Fleet)
            .WithOne(x => x.UserAccount)
            .HasForeignKey<Domain.Entities.Fleet.FleetEntity>(x => x.UserAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
