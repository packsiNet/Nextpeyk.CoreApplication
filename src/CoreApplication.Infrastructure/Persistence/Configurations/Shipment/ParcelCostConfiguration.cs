using CoreApplication.Domain.Entities.Shipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Shipment;

public class ParcelCostConfiguration : IEntityTypeConfiguration<ParcelCost>
{
    public void Configure(EntityTypeBuilder<ParcelCost> builder)
    {
        builder.ToTable("ParcelCost");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ParcelCostId");
        builder.Property(x => x.PickupCost).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DistributionCost).HasColumnType("decimal(18,2)");
        builder.Ignore(x => x.TotalCost);
    }
}
