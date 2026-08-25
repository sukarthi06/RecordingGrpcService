using Microsoft.Extensions.Logging;
using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingGrpcService.Infrastructure.Interfaces;

namespace RecordingGrpcService.Application.Services;

public class RecordingChunkService(
    IRecordingChunkRepo recordingChunkRepo,
    IRecordingRepo recordingRepo,
    ILogger<RecordingChunkService> logger) : IRecordingChunkService
{
    public async Task<RecordingChunk?> GetRecordingChunkAsync(ChunkId chunkId, CancellationToken cancellationToken)
    {
        if(ChunkId.Empty(chunkId.Value))
        {
            logger.LogWarning("Attempted to retrieve recording chunk with empty ID.");
            return await Task.FromResult<RecordingChunk?>(null);
        }

        return await recordingChunkRepo.GetRecordingChunkAsync(chunkId, cancellationToken);
    }

    public async Task<List<RecordingChunk>> GetRecordingChunksAsync(RecordingId recording, CancellationToken cancellationToken)
    {
        var chunks = await recordingChunkRepo.GetRecordingChunksAsync(recording, cancellationToken);
        return chunks is null ? [] : chunks;
    }

    public async Task<List<RecordingChunk>> GetSubsequentRecordingChunksAsync(ChunkId chunkId,
        CancellationToken cancellationToken = default)
    {
        return await recordingChunkRepo.GetSubsequentRecordingChunksAsync(chunkId, cancellationToken);
    }

    public async Task<bool> IsChunksReadyForConsolidationAsync(RecordingId recordingId, CancellationToken cancellationToken)
    {
        var result = await recordingRepo.IsRecordingCompletedAsync(recordingId, cancellationToken);
        if (result) 
            return await recordingChunkRepo.IsChunksTranscriptCompletedAsync(recordingId, cancellationToken);
        else 
            return false;
    }

    public async Task<bool> IsChunksReadyForConsolidateDiarizationAsync(RecordingId recordingId, CancellationToken cancellationToken)
    {
        var result = await recordingRepo.IsRecordingCompletedAsync(recordingId, cancellationToken);
        if (result)
            return await recordingChunkRepo.IsChunksDiarizationCompletedAsync(recordingId, cancellationToken);
        else
            return false;
    }

    public async Task<bool> SaveRecordingChunkAsync(RecordingChunk recordingChunk, CancellationToken cancellationToken)
    {
        return await recordingChunkRepo.SaveRecordingChunkAsync(recordingChunk, cancellationToken);
    }

    public async Task<bool> UpdateDiarizationPathAsync(
        ChunkId chunkId, string diarizationStoragePath, CancellationToken cancellationToken)
    {
        var response = await recordingChunkRepo.UpdateDiarizationPathAsync(chunkId, diarizationStoragePath, cancellationToken);
        if (response)
            logger.LogInformation("Diarization storage path updated for ChunkId: {ChunkId}", chunkId);
        else
            logger.LogInformation("Diarization storage path update failed for ChunkId: {ChunkId}", chunkId);
        return response;
    }

    public async Task<bool> UpdateStoragePathAsync(ChunkId chunkId, string storagePath, CancellationToken cancellationToken)
    {
        return await recordingChunkRepo.UpdateStoragePathAsync(chunkId, storagePath, cancellationToken);
    }

    public async Task<bool> UpdateTranscriptPathAsync(
        ChunkId chunkId, string transcriptStoragePath, CancellationToken cancellationToken)
    {
        var response = await recordingChunkRepo.UpdateTranscriptPathAsync(chunkId, transcriptStoragePath, cancellationToken);
        if (response)
            logger.LogInformation("Transcript storage path updated for ChunkId: {ChunkId}", chunkId);
        else
            logger.LogInformation("Transcript storage path update failed for ChunkId: {ChunkId}", chunkId);
        return response;
    }

    public async Task<bool> UpdateWavPathAsync(ChunkId chunkId, string wavStoragePath, CancellationToken cancellationToken)
    {
        var response = await recordingChunkRepo.UpdateWavPathAsync(chunkId, wavStoragePath, cancellationToken);
        if (response)
            logger.LogInformation("Wav storage path updated for ChunkId: {ChunkId}", chunkId);
        else
            logger.LogInformation("Wav storage path update failed for ChunkId: {ChunkId}", chunkId);
        return response;
    }
}
