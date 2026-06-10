using CoreApplication.Domain.Entities.Courier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Courier;

public class CourierBoxSizeConfiguration : IEntityTypeConfiguration<CourierBoxSize>
{
    public void Configure(EntityTypeBuilder<CourierBoxSize> builder)
    {
        builder.ToTable("CourierBoxSize");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CourierBoxSizeId");
        builder.Property(x => x.PackagingPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasOne(x => x.Courier)
            .WithMany(x => x.BoxSizes)
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.BoxSize)
            .WithMany(x => x.CourierBoxSizes)
            .HasForeignKey(x => x.BoxSizeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
