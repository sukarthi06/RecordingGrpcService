using Microsoft.Extensions.Logging;
using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingGrpcService.Infrastructure.Interfaces;

namespace RecordingGrpcService.Application.Services;

public class RecordingChunkService(
    IRecordingChunkRepo recordingChunkRepo,
    ILogger<RecordingChunkService> logger) : IRecordingChunkService
{
    public async Task<RecordingChunk?> GetRecordingChunkAsync(ChunkId chunkId, CancellationToken cancellationToken = default)
    {
        if(ChunkId.Empty(chunkId.Value))
        {
            logger.LogWarning("Attempted to retrieve recording chunk with empty ID.");
            return await Task.FromResult<RecordingChunk?>(null);
        }

        return await recordingChunkRepo.GetRecordingChunkAsync(chunkId, cancellationToken);
    }

    public async Task<List<RecordingChunk>> GetSubsequentRecordingChunksAsync(ChunkId chunkId,
        CancellationToken cancellationToken = default)
    {
        return await recordingChunkRepo.GetSubsequentRecordingChunksAsync(chunkId, cancellationToken);
    }

    public async Task<bool> SaveRecordingChunkAsync(RecordingChunk recordingChunk, CancellationToken cancellationToken = default)
    {
        return await recordingChunkRepo.SaveRecordingChunkAsync(recordingChunk, cancellationToken);
    }

    public async Task<bool> UpdateStoragePathAsync(ChunkId chunkId, string storagePath, CancellationToken cancellationToken = default)
    {
        return await recordingChunkRepo.UpdateStoragePathAsync(chunkId, storagePath, cancellationToken);
    }

    public async Task<bool> UpdateWavPathAsync(ChunkId chunkId, string wavStoragePath, CancellationToken cancellationToken)
    {
        return await recordingChunkRepo.UpdateWavPathAsync(chunkId, wavStoragePath, cancellationToken);
    }
}
