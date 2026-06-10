using CoreApplication.Domain.Entities.Courier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Courier;

public class BoxSizeConfiguration : IEntityTypeConfiguration<BoxSize>
{
    public void Configure(EntityTypeBuilder<BoxSize> builder)
    {
        builder.ToTable("BoxSize");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BoxSizeId");
        builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.BoxLength).HasColumnType("decimal(10,3)");
        builder.Property(x => x.BoxWidth).HasColumnType("decimal(10,3)");
        builder.Property(x => x.BoxHeight).HasColumnType("decimal(10,3)");
        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
