using CoreApplication.Domain.Entities.Sla;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Sla;

public class SlaGlobalConfigConfiguration : IEntityTypeConfiguration<SlaGlobalConfig>
{
    public void Configure(EntityTypeBuilder<SlaGlobalConfig> builder)
    {
        builder.ToTable("SlaGlobalConfig");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BenchmarkCostPerParcel).HasColumnType("decimal(18,2)");
        builder.Property(x => x.WeightSuccessRate).HasColumnType("decimal(5,4)");
        builder.Property(x => x.WeightOnTimeDelivery).HasColumnType("decimal(5,4)");
        builder.Property(x => x.WeightReturnRate).HasColumnType("decimal(5,4)");
        builder.Property(x => x.WeightDeliveryTime).HasColumnType("decimal(5,4)");
        builder.Property(x => x.WeightCost).HasColumnType("decimal(5,4)");

        builder.HasData(new SlaGlobalConfig
        {
            Id = 1,
            BenchmarkCostPerParcel = 100_000,
            WeightSuccessRate = 0.30m,
            WeightOnTimeDelivery = 0.30m,
            WeightReturnRate = 0.20m,
            WeightDeliveryTime = 0.10m,
            WeightCost = 0.10m,
            EffectiveFrom = new DateTime(2025, 1, 1),
            CreatedByUserId = 1,
            CreatedAt = new DateTime(2025, 1, 1)
        });
    }
}
