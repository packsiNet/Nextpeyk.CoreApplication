using CoreApplication.Domain.Entities.Courier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Courier;

public class CourierActivityTimeConfiguration : IEntityTypeConfiguration<CourierActivityTime>
{
    public void Configure(EntityTypeBuilder<CourierActivityTime> builder)
    {
        builder.ToTable("CourierActivityTimes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedByIp).HasMaxLength(50);

        builder.HasOne(x => x.Courier)
            .WithMany(x => x.ActivityTimes)
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
