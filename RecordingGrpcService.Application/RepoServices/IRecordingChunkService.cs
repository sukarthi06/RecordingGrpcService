using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Application.RepoServices;

public interface IRecordingChunkService
{
    Task<RecordingChunk?> GetRecordingChunkAsync(ChunkId chunkId, CancellationToken cancellationToken);
    Task<bool> SaveRecordingChunkAsync(RecordingChunk recordingChunk, CancellationToken cancellationToken);
    Task<bool> UpdateStoragePathAsync(ChunkId chunkId, string storagePath, CancellationToken cancellationToken);
    Task<List<RecordingChunk>> GetSubsequentRecordingChunksAsync(ChunkId chunkId, CancellationToken cancellationToken);
    Task<bool> UpdateWavPathAsync(ChunkId chunkId, string wavStoragePath, CancellationToken cancellationToken);
    Task<bool> UpdateTranscriptPath(ChunkId chunkId, string transcriptStoragePath, CancellationToken cancellationToken);
}
