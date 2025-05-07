using CleanKit.Net.Idempotency.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanKit.Net.Idempotency.Persistence.Configurations;

public class IdempotentRequestConfiguration : IEntityTypeConfiguration<IdempotentRequest>
{
    public void Configure(EntityTypeBuilder<IdempotentRequest> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired();
    }
}