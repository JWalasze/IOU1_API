using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currency");

            builder.HasKey(c => c.Key);

            builder.Property(x => x.Key)
                .HasColumnName("CurrencyKey");

            builder.Property(x => x.Version)
                   .IsRowVersion();
        }
    }
}
