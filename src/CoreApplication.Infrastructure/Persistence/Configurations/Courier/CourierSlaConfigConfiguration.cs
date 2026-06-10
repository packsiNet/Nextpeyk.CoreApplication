using CoreApplication.Domain.Entities.Courier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Courier;

public class CourierSlaConfigConfiguration : IEntityTypeConfiguration<CourierSlaConfig>
{
    public void Configure(EntityTypeBuilder<CourierSlaConfig> builder)
    {
        builder.ToTable("CourierSlaConfig");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CourierSlaConfigId");
        builder.Property(x => x.SuccessRateMin).HasColumnType("decimal(5,4)");
        builder.Property(x => x.OnTimeMin).HasColumnType("decimal(5,4)");
        builder.Property(x => x.ReturnRateMax).HasColumnType("decimal(5,4)");

        builder.HasOne(x => x.Courier)
            .WithMany(x => x.SlaConfigs)
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
