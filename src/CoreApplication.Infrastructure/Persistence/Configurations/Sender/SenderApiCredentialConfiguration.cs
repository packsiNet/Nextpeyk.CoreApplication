using CoreApplication.Domain.Entities.Sender;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApplication.Infrastructure.Persistence.Configurations.Sender;

public class SenderApiCredentialConfiguration : IEntityTypeConfiguration<SenderApiCredential>
{
    public void Configure(EntityTypeBuilder<SenderApiCredential> builder)
    {
        builder.ToTable("SenderApiCredential");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("SenderApiCredentialId");
        builder.Property(x => x.ApiKey).HasMaxLength(64).IsRequired();
        builder.Property(x => x.ApiSecretHash).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.HasIndex(x => x.ApiKey).IsUnique();

        builder.HasOne(x => x.Sender)
            .WithMany(x => x.ApiCredentials)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
