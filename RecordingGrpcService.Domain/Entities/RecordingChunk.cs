using RecordingGrpcService.Domain.Abstraction;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Domain.Entities;

public class RecordingChunk : Entity
{
    public ChunkId ChunkId { get; set; } = default!;
    public RecordingId RecordingId { get; set; } = default!;
    public int SequenceNumber { get; set; } = default!;
    public string StoragePath { get; set; } = default!;
    public TimeSpan StartTime { get; set; } = TimeSpan.Zero;
    public TimeSpan EndTime { get; set; } = TimeSpan.Zero;
    public double ChunkDuration { get; set; } = 0;
    public string? WavPath { get; set; }
    public string? TranscriptPath { get; set; }
    public string? DiarizationPath { get; set; }
}
