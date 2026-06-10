using CoreApplication.Domain.Entities.Shipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Shipment;

public class ParcelCourierFleetConfiguration : IEntityTypeConfiguration<ParcelCourierFleet>
{
    public void Configure(EntityTypeBuilder<ParcelCourierFleet> builder)
    {
        builder.ToTable("ParcelCourierFleet");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ParcelCourierFleetId");
        builder.Property(x => x.ReceiverName).HasMaxLength(200);
        builder.Property(x => x.Signature).HasMaxLength(2000);
        builder.Property(x => x.PODCode).HasMaxLength(50);
        builder.Property(x => x.ReturnNote).HasMaxLength(500);
        builder.Property(x => x.CreatedByIp).HasMaxLength(50);
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasOne(x => x.ParcelCourier)
            .WithMany(x => x.ParcelCourierFleets)
            .HasForeignKey(x => x.ParcelCourierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Fleet)
            .WithMany(x => x.ParcelCourierFleets)
            .HasForeignKey(x => x.FleetId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
