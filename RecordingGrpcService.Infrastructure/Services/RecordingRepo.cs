using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.Enums;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingGrpcService.Infrastructure.Interfaces;

namespace RecordingGrpcService.Infrastructure.Services;

public class RecordingRepo(
    ApplicationDbContext dbContext,
    ILogger<RecordingRepo> logger) : IRecordingRepo
{
    public Task<RecordingSession?> GetRecordingSessionAsync(RecordingId recordingId, CancellationToken cancellationToken = default)
    {
        var recordingSession = dbContext.RecordingSessions            
            .Include(rs => rs.RecordingMetadata)
            .AsNoTracking()
            .FirstOrDefaultAsync(rs => rs.RecordingId == recordingId, cancellationToken);
        return recordingSession;
    }

    public async Task<RecordingSession> SaveRecordingSessionAsync(RecordingSession recordingSession,
        CancellationToken cancellationToken = default)
    {
        await dbContext.RecordingSessions.AddAsync(recordingSession, cancellationToken);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
            logger.LogWarning("Failed to save recording session in database for RecordingId: {RecordingId}", recordingSession.RecordingId);
        else
            logger.LogInformation("Recording session saved in database for RecordingId: {RecordingId}", recordingSession.RecordingId);

        return recordingSession;
    }
    public async Task<bool> UpdateRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken = default)
    {
        var existingSession = await dbContext.RecordingSessions.FindAsync(recordingSession.RecordingId, cancellationToken);
        if (existingSession == null)
        {
            logger.LogWarning("Failed to update recording session for RecordingId: {RecordingId} in database",
                recordingSession.RecordingId);
            return false;
        }

        // Update the existing session with the new values
        existingSession.Status = recordingSession.Status;
        existingSession.StoppedAt = recordingSession.StoppedAt;
        existingSession.RecordingMetadata = recordingSession.RecordingMetadata;
        existingSession.LastModifiedAt = DateTime.UtcNow;

        var result = await dbContext.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            logger.LogWarning("Failed to update recording session for RecordingId: {RecordingId} in database",
                recordingSession.RecordingId);
            return false;
        }

        logger.LogInformation("Recording session for RecordingId: {RecordingId} updated in database", recordingSession.RecordingId);
        return true;
    }

    public async Task<bool> UpdateRecordingMetadataAsync(RecordingId recordingId, RecordingMetadata metadata,
        CancellationToken cancellationToken = default)
    {
        var recordingSession = await dbContext.RecordingSessions.FindAsync(recordingId, cancellationToken);
        if (recordingSession == null)
        {
            logger.LogWarning("Recording session not found for RecordingId: {RecordingId} in database", recordingId);
            return false;
        }

        recordingSession.RecordingMetadata = metadata;
        recordingSession.LastModifiedAt = DateTime.UtcNow;
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            logger.LogWarning("Failed to update recording metadata for RecordingId: {RecordingId} in database", recordingId);
            return false;
        }
        logger.LogInformation("Recording metadata updated for RecordingId: {RecordingId} in database",
            recordingSession.RecordingId);
        return true;
    }

    public async Task<bool> UpdateStoragePathAsync(RecordingId recordingId, string storagePath,
        CancellationToken cancellationToken = default)
    {
        var recordingSession = await dbContext.RecordingSessions
            .FirstOrDefaultAsync(rs => rs.RecordingId == recordingId, cancellationToken);
        if (recordingSession == null)
        {
            logger.LogWarning("Recording session not found for RecordingId: {RecordingId} in database while updating storage path",
                recordingId);
            return false;
        }

        recordingSession.StoragePath = storagePath;
        recordingSession.Status = RecordingStatus.Completed;
        recordingSession.LastModifiedAt = DateTime.UtcNow;
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            logger.LogWarning("Failed to update storage path for RecordingId: {RecordingId} in database", recordingId);
            return false;
        }
        logger.LogInformation("Storage path updated for RecordingId: {RecordingId} in database", recordingId);
        return true;
    }

    public async Task<bool> UpdateWavPathAsync(RecordingId recordingId, string wavPath, CancellationToken cancellationToken = default)
    {
        var recordingSession = await dbContext.RecordingSessions
            .FirstOrDefaultAsync(rs => rs.RecordingId == recordingId, cancellationToken);

        if (recordingSession == null)
        {
            logger.LogWarning("Recording session not found for RecordingId: {RecordingId} in database while updating wav path",
                recordingId);
            return false;
        }

        recordingSession.WavPath = wavPath;
        recordingSession.LastModifiedAt = DateTime.UtcNow;
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        if (result == 0)
        {
            logger.LogWarning("Failed to update wav path for RecordingId: {RecordingId} in database", recordingId);
            return false;
        }
        logger.LogInformation("Wav path updated for RecordingId: {RecordingId} in database", recordingId);
        return true;
    }
}

