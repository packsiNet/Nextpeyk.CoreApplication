using CoreApplication.Domain.Entities.Shipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Shipment;

public class ParcelConfiguration : IEntityTypeConfiguration<Parcel>
{
    public void Configure(EntityTypeBuilder<Parcel> builder)
    {
        builder.ToTable("Parcel");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ParcelId");
        builder.Property(x => x.Barcode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Length).HasColumnType("decimal(10,3)");
        builder.Property(x => x.Width).HasColumnType("decimal(10,3)");
        builder.Property(x => x.Height).HasColumnType("decimal(10,3)");
        builder.Property(x => x.Weight).HasColumnType("decimal(10,3)");
        builder.Property(x => x.Value).HasColumnType("decimal(18,2)");
        builder.Property(x => x.SenderFirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.SenderLastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.SenderPhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.SenderLatitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.SenderLongitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.SenderAddress).HasMaxLength(500).IsRequired();
        builder.Property(x => x.ReceiverFirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ReceiverLastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ReceiverPhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ReceiverLatitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.ReceiverLongitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.ReceiverAddress).HasMaxLength(500).IsRequired();
        builder.Property(x => x.CreatedByIp).HasMaxLength(50);
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasIndex(x => x.Barcode).IsUnique();

        builder.HasOne(x => x.Sender)
            .WithMany()
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
