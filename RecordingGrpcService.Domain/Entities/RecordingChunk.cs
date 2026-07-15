using RecordingGrpcService.Domain.Abstraction;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Domain.Entities;

public class RecordingChunk : Entity
{
    public ChunkId ChunkId { get; set; } = default!;
    public RecordingId RecordingId { get; set; } = default!;
    public int SequenceNumber { get; set; } = default!;
    public string StoragePath { get; set; } = default!;
    public string? WavPath { get; set; }
}
