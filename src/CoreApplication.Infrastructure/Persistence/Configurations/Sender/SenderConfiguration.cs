using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Sender;

public class SenderConfiguration : IEntityTypeConfiguration<Domain.Entities.Sender.Sender>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Sender.Sender> builder)
    {
        builder.ToTable("Sender");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("SenderId");
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NationalCode).HasMaxLength(20);
        builder.Property(x => x.EconomicCode).HasMaxLength(20);
        builder.Property(x => x.RegistrationNumber).HasMaxLength(50);
        builder.Property(x => x.ContractNumber).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
        builder.Property(x => x.Email).HasMaxLength(200);
        builder.Property(x => x.CreatedByIp).HasMaxLength(50);
        builder.Property(x => x.RowVersion).IsRowVersion();
    }
}
