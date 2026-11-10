namespace RecordingGrpcService.Domain.Diarization;

public sealed class SpeakerSegment
{
    public string Speaker { get; init; } = string.Empty;
    public double Start { get; init; }
    public double End { get; init; }
    public string Text { get; init; } = string.Empty;
}