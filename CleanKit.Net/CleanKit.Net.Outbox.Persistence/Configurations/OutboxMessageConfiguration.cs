using CleanKit.Net.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanKit.Net.Outbox.Persistence.Configurations;

public class ConsumedOutboxMessageConfiguration : IEntityTypeConfiguration<ConsumedOutboxMessage>
{
    public void Configure(EntityTypeBuilder<ConsumedOutboxMessage> builder)
    {
        builder.HasKey(e => new { e.EventId, e.Consumer });
    }
}