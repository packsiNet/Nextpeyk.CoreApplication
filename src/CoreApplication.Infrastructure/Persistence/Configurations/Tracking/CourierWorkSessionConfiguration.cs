using CoreApplication.Domain.Entities.Tracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Tracking;

public class CourierWorkSessionConfiguration : IEntityTypeConfiguration<CourierWorkSession>
{
    public void Configure(EntityTypeBuilder<CourierWorkSession> builder)
    {
        builder.ToTable("CourierWorkSession");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CourierWorkSessionId");
        builder.Property(x => x.TotalDistanceMeters).HasColumnType("float");

        builder.HasIndex(x => new { x.FleetId, x.SessionDate }).IsUnique();
        builder.HasIndex(x => new { x.CourierId, x.SessionDate });

        builder.HasOne(x => x.Fleet)
            .WithMany()
            .HasForeignKey(x => x.FleetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Courier)
            .WithMany()
            .HasForeignKey(x => x.CourierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
