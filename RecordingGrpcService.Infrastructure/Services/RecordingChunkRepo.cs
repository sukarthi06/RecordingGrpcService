using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingGrpcService.Infrastructure.Interfaces;
using System.Diagnostics;

namespace RecordingGrpcService.Infrastructure.Services;

public class RecordingChunkRepo(
    ApplicationDbContext dbContext,
    ILogger<RecordingChunkRepo> logger) : IRecordingChunkRepo
{
    private static readonly ActivitySource ActivitySource = new("RecordingGrpcService.Repository");

    public async Task<RecordingChunk?> GetRecordingChunkAsync(ChunkId chunkId, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(GetRecordingChunkAsync));

        var recordingChunks = await dbContext.RecordingChunks
            .AsNoTracking()
            .Where(rc => rc.ChunkId == chunkId)
            .Select(rc => new RecordingChunk
            {
                ChunkId = rc.ChunkId,
                RecordingId = rc.RecordingId,
                SequenceNumber = rc.SequenceNumber,
                StoragePath = rc.StoragePath,
                WavPath = rc.WavPath ?? string.Empty,
                TranscriptPath = rc.TranscriptPath ?? string.Empty,
                DiarizationPath = rc.DiarizationPath ?? string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);
        return recordingChunks;
    }

    public async Task<List<RecordingChunk>?> GetRecordingChunksAsync(RecordingId recordingId, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(GetRecordingChunksAsync));

        var recordingChunks = await dbContext.RecordingChunks
            .AsNoTracking()
            .Where(rc => rc.RecordingId == recordingId
                && rc.RecordStatus == 1)
            .Select(rc => new RecordingChunk
            {
                ChunkId = rc.ChunkId,
                RecordingId = rc.RecordingId,
                SequenceNumber = rc.SequenceNumber,
                StoragePath = rc.StoragePath,
                WavPath = rc.WavPath ?? string.Empty,
                TranscriptPath = rc.TranscriptPath ?? string.Empty,
                DiarizationPath = rc.DiarizationPath ?? string.Empty
            })
            .ToListAsync(cancellationToken);
        return recordingChunks;
    }

    public async Task<List<RecordingChunk>> GetSubsequentRecordingChunksAsync(ChunkId chunkId, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(GetSubsequentRecordingChunksAsync));

        var chunks = await dbContext.RecordingChunks
            .AsNoTracking()
            .Where(rc =>
                rc.ChunkId == chunkId ||
                dbContext.RecordingChunks.Any(current =>
                    current.ChunkId == chunkId &&
                    current.RecordingId == rc.RecordingId &&
                    current.SequenceNumber - 1 == rc.SequenceNumber))
            .OrderBy(rc => rc.SequenceNumber)
            .Select(rc => new RecordingChunk
            {
                ChunkId = rc.ChunkId,
                RecordingId = rc.RecordingId,
                SequenceNumber = rc.SequenceNumber,
                StoragePath = rc.StoragePath,
                WavPath = rc.WavPath ?? string.Empty,
                TranscriptPath = rc.TranscriptPath ?? string.Empty,
                DiarizationPath = rc.DiarizationPath ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        return chunks;
    }

    public async Task<bool> IsChunksTranscriptCompletedAsync(RecordingId recordingId, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(IsChunksTranscriptCompletedAsync));

        var result = await dbContext.RecordingChunks
            .AsNoTracking()
            .Where(rc => rc.RecordingId == recordingId
                && (rc.TranscriptPath == null || rc.TranscriptPath == string.Empty)
                && rc.RecordStatus == 1)
            .CountAsync(cancellationToken);                            
        return result == 0;
    }

    public async Task<bool> IsChunksDiarizationCompletedAsync(RecordingId recordingId, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(IsChunksDiarizationCompletedAsync));

        var result = await dbContext.RecordingChunks
            .AsNoTracking()
            .Where(rc => rc.RecordingId == recordingId
                && (rc.DiarizationPath == null || rc.DiarizationPath == string.Empty)
                && rc.RecordStatus == 1)
            .CountAsync(cancellationToken);
        return result == 0;
    }

    public async Task<bool> SaveRecordingChunkAsync(RecordingChunk recordingChunk, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(SaveRecordingChunkAsync));

        await dbContext.RecordingChunks.AddAsync(recordingChunk, cancellationToken);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if(result <= 0)
        {
            logger.LogError("Failed to save recording chunk with ChunkId: {ChunkId}", recordingChunk.ChunkId);
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateStoragePathAsync(ChunkId chunkId, string storagePath, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(UpdateStoragePathAsync));

        var existingChunk = await dbContext.RecordingChunks.FindAsync(chunkId, cancellationToken);
        if (existingChunk == null)
        {
            logger.LogWarning("Recording chunk with ChunkId: {ChunkId} not found", chunkId);
            return false;
        }

        existingChunk.StoragePath = storagePath;
        existingChunk.LastModifiedAt = DateTime.UtcNow;
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            logger.LogError("Failed to update storage path for recording chunk with ChunkId: {ChunkId}", chunkId);
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateTranscriptPathAsync(ChunkId chunkId, string transcriptStoragePath, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(UpdateTranscriptPathAsync));

        var existingChunk = await dbContext.RecordingChunks.FindAsync(chunkId, cancellationToken);
        if (existingChunk == null)
        {
            logger.LogWarning("Recording chunk with ChunkId: {ChunkId} not found", chunkId);
            return false;
        }

        existingChunk.TranscriptPath = transcriptStoragePath;
        existingChunk.LastModifiedAt = DateTime.UtcNow;
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            logger.LogError("Failed to update transcript path for recording chunk with ChunkId: {ChunkId}", chunkId);
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateWavPathAsync(ChunkId chunkId, string wavStoragePath, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(UpdateWavPathAsync));

        var existingChunk = await dbContext.RecordingChunks.FindAsync(chunkId, cancellationToken);
        if (existingChunk == null)
        {
            logger.LogWarning("Recording chunk with ChunkId: {ChunkId} not found", chunkId);
            return false;
        }

        existingChunk.WavPath = wavStoragePath;
        existingChunk.LastModifiedAt = DateTime.UtcNow;
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            logger.LogError("Failed to update wav path for recording chunk with ChunkId: {ChunkId}", chunkId);
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateDiarizationPathAsync(ChunkId chunkId, string diarizationStoragePath, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(nameof(UpdateDiarizationPathAsync));

        var existingChunk = await dbContext.RecordingChunks.FindAsync(chunkId, cancellationToken);
        if (existingChunk == null)
        {
            logger.LogWarning("Recording chunk with ChunkId: {ChunkId} not found", chunkId);
            return false;
        }

        existingChunk.DiarizationPath = diarizationStoragePath;
        existingChunk.LastModifiedAt = DateTime.UtcNow;
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            logger.LogError("Failed to update diarization path for recording chunk with ChunkId: {ChunkId}", chunkId);
            return false;
        }

        return true;
    }
}
