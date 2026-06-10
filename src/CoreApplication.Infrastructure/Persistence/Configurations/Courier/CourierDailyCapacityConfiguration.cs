using CoreApplication.Domain.Entities.Courier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Courier;

public class CourierDailyCapacityConfiguration : IEntityTypeConfiguration<CourierDailyCapacity>
{
    public void Configure(EntityTypeBuilder<CourierDailyCapacity> builder)
    {
        builder.ToTable("CourierDailyCapacity");
        builder.HasKey(x => new { x.CourierId, x.Date });

        builder.HasOne(x => x.Courier)
            .WithMany()
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
