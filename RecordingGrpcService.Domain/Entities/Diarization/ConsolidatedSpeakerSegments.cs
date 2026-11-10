namespace RecordingGrpcService.Domain.Diarization;

public sealed class ConsolidatedSpeakerSegments
{
    public int SequenceNumber { get; init; }
    public List<SpeakerSegment> SpeakerSegments { get; init; } = [];
}