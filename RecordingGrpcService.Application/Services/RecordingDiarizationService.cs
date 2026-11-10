using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Domain.Diarization;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingGrpcService.Infrastructure.Interfaces;
using RecordingGrpcService.Infrastructure.Options;

namespace RecordingGrpcService.Application.Services;

public sealed class RecordingDiarizationService(
    ILogger<RecordingDiarizationService> logger,
    IRecordingRepo recordingRepo,
    IBlobObjectStorage blobObjectStorage,
    IOptions<AzureBlobStorageOptions> storageOptions) : IRecordingDiarizationService
{
    public async Task<List<ConsolidatedSpeakerSegments>?> GetConsolidatedDiarizationAsync(
        RecordingId recordingId, CancellationToken cancellationToken)
    {
        var recordingSession = await recordingRepo.GetRecordingSessionAsync(recordingId, cancellationToken);
        if (recordingSession is null)
        {
            logger.LogWarning("Recording session not found for RecordingId: {RecordingId}", recordingId);
            return null;
        }

        if (string.IsNullOrWhiteSpace(recordingSession.DiarizationPath))
        {
            logger.LogInformation("Recording {RecordingId} has no diarization path yet", recordingId);
            return null;
        }

        return await blobObjectStorage.DownloadJsonAsync<List<ConsolidatedSpeakerSegments>>(
            storageOptions.Value.TranscriptsContainer,
            recordingSession.DiarizationPath,
            cancellationToken);
    }
}