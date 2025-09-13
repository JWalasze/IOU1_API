using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations;

public class TransactionStatusConfiguration : IEntityTypeConfiguration<TransactionStatus>
{
    public void Configure(EntityTypeBuilder<TransactionStatus> builder)
    {
        builder.ToTable("TransactionStatus");

        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.Name)
               .IsRequired();
    }
}
