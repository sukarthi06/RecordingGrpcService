namespace RecordingService.Api.Contracts;

public sealed record ConsolidatedSpeakerSegmentsResponse(
    int SequenceNumber,
    IReadOnlyList<SpeakerSegmentResponse> SpeakerSegments);

public sealed record SpeakerSegmentResponse(
    string Speaker,
    double Start,
    double End,
    string Text);