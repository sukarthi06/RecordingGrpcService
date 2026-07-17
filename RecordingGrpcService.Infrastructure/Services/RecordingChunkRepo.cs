using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingGrpcService.Infrastructure.Interfaces;

namespace RecordingGrpcService.Infrastructure.Services;

public class RecordingChunkRepo(
    ApplicationDbContext dbContext,
    ILogger<RecordingChunkRepo> logger) : IRecordingChunkRepo
{
    public async Task<RecordingChunk?> GetRecordingChunkAsync(ChunkId chunkId, CancellationToken cancellationToken)
    {
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
                TranscriptPath = rc.TranscriptPath ?? string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);
        return recordingChunks;
    }
    public async Task<List<RecordingChunk>> GetSubsequentRecordingChunksAsync(ChunkId chunkId, CancellationToken cancellationToken)
    {
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
                TranscriptPath = rc.TranscriptPath ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        return chunks;
    }
    public async Task<bool> SaveRecordingChunkAsync(RecordingChunk recordingChunk, CancellationToken cancellationToken)
    {
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
            logger.LogError("Failed to update storage path for recording chunk with ChunkId: {ChunkId}", chunkId);
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateWavPathAsync(ChunkId chunkId, string wavStoragePath, CancellationToken cancellationToken)
    {
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
            logger.LogError("Failed to update storage path for recording chunk with ChunkId: {ChunkId}", chunkId);
            return false;
        }

        return true;
    }
}
