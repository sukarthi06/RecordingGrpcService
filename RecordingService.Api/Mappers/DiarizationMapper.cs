using RecordingGrpcService.Domain.Diarization;
using RecordingService.Api.Contracts;

namespace RecordingService.Api.Mappers;

public static class DiarizationMapper
{
    public static IReadOnlyList<ConsolidatedSpeakerSegmentsResponse> ToResponse(
        this IEnumerable<ConsolidatedSpeakerSegments> source) =>
        source
            .OrderBy(s => s.SequenceNumber)
            .Select(s => new ConsolidatedSpeakerSegmentsResponse(
                s.SequenceNumber,
                s.SpeakerSegments
                    .Select(seg => new SpeakerSegmentResponse(seg.Speaker, seg.Start, seg.End, seg.Text))
                    .ToList()))
            .ToList();
}