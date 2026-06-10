using CoreApplication.Domain.Entities.Courier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Courier;

public class CourierZoneConfiguration : IEntityTypeConfiguration<CourierZone>
{
    public void Configure(EntityTypeBuilder<CourierZone> builder)
    {
        builder.ToTable("CourierZone");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CourierZoneId");
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Boundary).HasMaxLength(4000);
        builder.Property(x => x.CenterPoint).HasMaxLength(100);
        builder.Property(x => x.Radius).HasColumnType("decimal(18,6)");
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.CreatedByIp).HasMaxLength(50);
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasOne(x => x.Courier)
            .WithMany(x => x.Zones)
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.City)
            .WithMany(x => x.CourierZones)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
