using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Infrastructure.Interfaces;

public interface IRecordingChunkRepo
{
    Task<RecordingChunk?> GetRecordingChunkAsync(ChunkId chunkId, CancellationToken cancellationToken);
    Task<bool> SaveRecordingChunkAsync(RecordingChunk recordingChunk, CancellationToken cancellationToken);
    Task<bool> UpdateStoragePathAsync(ChunkId chunkId, string storagePath, CancellationToken cancellationToken);
    Task<List<RecordingChunk>> GetSubsequentRecordingChunksAsync(ChunkId chunkId, CancellationToken cancellationToken);
    Task<List<RecordingChunk>?> GetRecordingChunksAsync(RecordingId recording, CancellationToken cancellationToken);
    Task<bool> UpdateWavPathAsync(ChunkId chunkId, string wavStoragePath, CancellationToken cancellationToken);
    Task<bool> UpdateTranscriptPathAsync(ChunkId chunkId, string transcriptStoragePath, CancellationToken cancellationToken);
    Task<bool> UpdateDiarizationPathAsync(ChunkId chunkId, string diarizationStoragePath, CancellationToken cancellationToken);
    Task<bool> IsChunksTranscriptCompletedAsync(RecordingId recordingId, CancellationToken cancellationToken);
    Task<bool> IsChunksDiarizationCompletedAsync(RecordingId recordingId, CancellationToken cancellationToken);
}