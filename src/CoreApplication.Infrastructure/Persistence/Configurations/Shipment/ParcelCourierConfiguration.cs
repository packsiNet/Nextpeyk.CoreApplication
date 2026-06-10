using CoreApplication.Domain.Entities.Shipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Shipment;

public class ParcelCourierConfiguration : IEntityTypeConfiguration<ParcelCourier>
{
    public void Configure(EntityTypeBuilder<ParcelCourier> builder)
    {
        builder.ToTable("ParcelCourier");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ParcelCourierId");
        builder.Property(x => x.CollectFirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CollectLastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CollectPhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.CollectAddress).HasMaxLength(500).IsRequired();
        builder.Property(x => x.CollectLatitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.CollectLongitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.DistributeFirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DistributeLastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DistributePhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.DistributeAddress).HasMaxLength(500).IsRequired();
        builder.Property(x => x.DistributeLatitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.DistributeLongitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.CreatedByIp).HasMaxLength(50);
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasOne(x => x.Parcel)
            .WithMany(x => x.ParcelCouriers)
            .HasForeignKey(x => x.ParcelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Courier)
            .WithMany()
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Cost)
            .WithOne(x => x.ParcelCourier)
            .HasForeignKey<ParcelCost>(x => x.ParcelCourierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
