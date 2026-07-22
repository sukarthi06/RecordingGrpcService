using RecordingGrpcService.Domain.Abstraction;
using RecordingGrpcService.Domain.Enums;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Domain.Entities;

public class RecordingSession : Entity
{
    public RecordingId RecordingId { get; set; } = RecordingId.Of(Guid.NewGuid());
    public RecordingStatus Status { get; set; } = RecordingStatus.Started;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StoppedAt { get; set; }
    public string? StoragePath { get; set; }
    public string? WavPath { get; set; }
    public string? TranscriptPath { get; set; }    
    public RecordingMetadata? RecordingMetadata { get; set; }
}
