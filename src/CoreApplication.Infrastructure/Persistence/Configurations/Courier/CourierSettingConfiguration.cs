using CoreApplication.Domain.Entities.Courier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Courier;

public class CourierSettingConfiguration : IEntityTypeConfiguration<CourierSetting>
{
    public void Configure(EntityTypeBuilder<CourierSetting> builder)
    {
        builder.ToTable("CourierSettings");
        builder.HasKey(x => x.Id);
    }
}
