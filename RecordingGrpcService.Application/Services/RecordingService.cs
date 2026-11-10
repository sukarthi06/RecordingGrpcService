using Microsoft.Extensions.Logging;
using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingGrpcService.Infrastructure.Interfaces;

namespace RecordingGrpcService.Application.Services;

public class RecordingService(
    ILogger<RecordingService> logger,
    IRecordingRepo recordingRepo) : IRecordingService
{
    public async Task<RecordingMetadata> GetRecordingMetadataAsync(RecordingId recordingId, CancellationToken cancellationToken)
    {
        var recordingSession = await recordingRepo.GetRecordingSessionAsync(recordingId, cancellationToken);
        return recordingSession?.RecordingMetadata ?? new RecordingMetadata(0, 0, 0, string.Empty);
    }

    public async Task<RecordingSession?> GetRecordingSessionAsync(RecordingId recordingId, CancellationToken cancellationToken)
    {
        if(RecordingId.Empty(recordingId.Value))
        {
            logger.LogWarning("GetRecordingSessionAsync called with empty recordingId:{RecordingId}", recordingId);
            return await Task.FromResult<RecordingSession?>(null);
        }
        return await recordingRepo.GetRecordingSessionAsync(recordingId, cancellationToken);
    }

    public async Task<RecordingSession> SaveRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken)
    {
        return await recordingRepo.SaveRecordingSessionAsync(recordingSession, cancellationToken);
    }

    public async Task<bool> UpdateRecordingMetadataAsync(
        RecordingId recordingId, RecordingMetadata metadata, CancellationToken cancellationToken)
    {
        return await recordingRepo.UpdateRecordingMetadataAsync(recordingId, metadata, cancellationToken);
    }

    public async Task<bool> UpdateRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken)
    {
        return await recordingRepo.UpdateRecordingSessionAsync(recordingSession, cancellationToken);
    }

    public async Task<bool> UpdateStoragePathAsync(RecordingId recordingId, string storagePath, CancellationToken cancellationToken)
    {
        return await recordingRepo.UpdateStoragePathAsync(recordingId, storagePath, cancellationToken);
    }

    public async Task<bool> UpdateTranscriptPathAsync(
        RecordingId recordingId, string transcriptPath, CancellationToken cancellationToken)
    {
        return await recordingRepo.UpdateTranscriptPathAsync(recordingId, transcriptPath, cancellationToken);
    }

    public async Task<bool> UpdateWavPathAsync(RecordingId recordingId, string wavPath, CancellationToken cancellationToken)
    {
        return await recordingRepo.UpdateWavPathAsync(recordingId, wavPath, cancellationToken);
    }

    public async Task<bool> UpdateDiarizationPathAsync(RecordingId recordingId, string diarizationPath, CancellationToken cancellationToken)
    {
        return await recordingRepo.UpdateDiarizationPathAsync(recordingId, diarizationPath, cancellationToken);
    }

    public async Task<bool> IsDiarizedAsync(RecordingId recordingId, CancellationToken cancellationToken)
    {
        return await recordingRepo.IsDiarizedAsync(recordingId, cancellationToken);
    }
}
