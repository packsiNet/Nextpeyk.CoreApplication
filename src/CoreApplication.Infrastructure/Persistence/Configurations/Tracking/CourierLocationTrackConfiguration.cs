using CoreApplication.Domain.Entities.Tracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Tracking;

public class CourierLocationTrackConfiguration : IEntityTypeConfiguration<CourierLocationTrack>
{
    public void Configure(EntityTypeBuilder<CourierLocationTrack> builder)
    {
        builder.ToTable("CourierLocationTrack");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CourierLocationTrackId").ValueGeneratedOnAdd();
        builder.Property(x => x.Location).HasColumnType("geography");
        builder.Property(x => x.Latitude).HasColumnType("float");
        builder.Property(x => x.Longitude).HasColumnType("float");
        builder.Property(x => x.Accuracy).HasColumnType("real");
        builder.Property(x => x.Speed).HasColumnType("real");
        builder.Property(x => x.Heading).HasColumnType("real");

        builder.HasIndex(x => new { x.FleetId, x.RecordedAt });
        builder.HasIndex(x => new { x.CourierId, x.RecordedAt });

        builder.HasOne(x => x.WorkSession)
            .WithMany(x => x.LocationTracks)
            .HasForeignKey(x => x.WorkSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
