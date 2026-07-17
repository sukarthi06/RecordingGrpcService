using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Infrastructure.Configuration;

public sealed class RecordingChunkConfiguration
    : IEntityTypeConfiguration<RecordingChunk>
{
    public void Configure(EntityTypeBuilder<RecordingChunk> builder)
    {
        builder.ToTable("RecordingChunks", "recording");

        builder.HasKey(x => x.ChunkId);

        builder.Property(x => x.ChunkId)
            .HasConversion(
                id => id.Value,
                value => ChunkId.Of(value));

        builder.Property(x => x.RecordingId)
            .HasConversion(
                id => id.Value,
                value => RecordingId.Of(value));

        builder.Property(x => x.SequenceNumber)
            .IsRequired();

        builder.Property(x => x.StoragePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.WavPath)
            .HasMaxLength(500);

        builder.Property(x => x.TranscriptPath)
            .HasMaxLength(500);

        // Inherited properties
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy);

        builder.Property(x => x.LastModifiedAt);

        builder.Property(x => x.LastModifiedBy);

        builder.Property(x => x.RecordStatus);
    }
}
