using RecordingGrpcService.Domain.Diarization;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Application.RepoServices;

public interface IRecordingDiarizationService
{
    Task<List<ConsolidatedSpeakerSegments>?> GetConsolidatedDiarizationAsync(
        RecordingId recordingId, CancellationToken cancellationToken);
}