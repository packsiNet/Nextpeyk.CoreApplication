using CoreApplication.Domain.Entities.Sla;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Sla;

public class CarrierSlaSnapshotConfiguration : IEntityTypeConfiguration<CarrierSlaSnapshot>
{
    public void Configure(EntityTypeBuilder<CarrierSlaSnapshot> builder)
    {
        builder.ToTable("CarrierSlaSnapshot");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AvgDeliveryHours).HasColumnType("decimal(10,2)");
        builder.Property(x => x.AvgCostPerParcel).HasColumnType("decimal(18,2)");
        builder.Property(x => x.SuccessRate).HasColumnType("decimal(5,4)");
        builder.Property(x => x.OnTimeDelivery).HasColumnType("decimal(5,4)");
        builder.Property(x => x.ReturnRate).HasColumnType("decimal(5,4)");
        builder.Property(x => x.DeliveryTimeScore).HasColumnType("decimal(5,4)");
        builder.Property(x => x.CostEfficiencyScore).HasColumnType("decimal(5,4)");
        builder.Property(x => x.SlaScore).HasColumnType("decimal(5,4)");

        builder.HasOne(x => x.Courier)
            .WithMany()
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SlaGlobalConfig)
            .WithMany(x => x.Snapshots)
            .HasForeignKey(x => x.SlaGlobalConfigId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.CourierId, x.PeriodType, x.PeriodStart });
    }
}
