using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Courier;

public class CourierConfiguration : IEntityTypeConfiguration<Domain.Entities.Courier.Courier>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Courier.Courier> builder)
    {
        builder.ToTable("Courier");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CourierId");
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Logo).HasMaxLength(500);
        builder.Property(x => x.SupportPhoneNumber).HasMaxLength(20);
        builder.Property(x => x.SupportFullName).HasMaxLength(200);
        builder.Property(x => x.Website).HasMaxLength(200);
        builder.Property(x => x.SystemName).HasMaxLength(100);
        builder.Property(x => x.EconomicCode).HasMaxLength(50);
        builder.Property(x => x.NationalCode).HasMaxLength(20);
        builder.Property(x => x.BasePrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.CodPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.FreightCollectPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.MaximumShipmentWeight).HasColumnType("decimal(18,3)");
        builder.Property(x => x.MaximumValueOfTheShipment).HasColumnType("decimal(18,2)");
        builder.Property(x => x.CreatedByIp).HasMaxLength(50);
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasOne(x => x.Setting)
            .WithOne(x => x.Courier)
            .HasForeignKey<Domain.Entities.Courier.CourierSetting>(x => x.CourierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
