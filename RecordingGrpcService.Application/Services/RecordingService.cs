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
    public async Task<RecordingSession?> GetRecordingSessionAsync(RecordingId recordingId, CancellationToken cancellationToken = default)
    {
        if(RecordingId.Empty(recordingId.Value))
        {
            logger.LogWarning("GetRecordingSessionAsync called with empty recordingId:{RecordingId}", recordingId);
            return await Task.FromResult<RecordingSession?>(null);
        }
        return await recordingRepo.GetRecordingSessionAsync(recordingId, cancellationToken);
    }

    public async Task<RecordingSession> SaveRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken = default)
    {
        return await recordingRepo.SaveRecordingSessionAsync(recordingSession, cancellationToken);
    }

    public async Task<bool> UpdateRecordingMetadataAsync(RecordingId recordingId, RecordingMetadata metadata, CancellationToken cancellationToken = default)
    {
        return await recordingRepo.UpdateRecordingMetadataAsync(recordingId, metadata, cancellationToken);
    }

    public async Task<bool> UpdateRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken = default)
    {
        return await recordingRepo.UpdateRecordingSessionAsync(recordingSession, cancellationToken);
    }

    public async Task<bool> UpdateStoragePathAsync(RecordingId recordingId, string storagePath, CancellationToken cancellationToken = default)
    {
        return await recordingRepo.UpdateStoragePathAsync(recordingId, storagePath, cancellationToken);
    }
}
