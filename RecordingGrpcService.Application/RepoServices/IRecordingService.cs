using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Application.RepoServices;

public interface IRecordingService
{
    Task<RecordingSession?> GetRecordingSessionAsync(RecordingId recordingId, CancellationToken cancellationToken = default);
    Task<RecordingSession> SaveRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken = default);
    Task<bool> UpdateStoragePathAsync(RecordingId recordingId, string storagePath, CancellationToken cancellationToken = default);
    Task<bool> UpdateRecordingMetadataAsync(RecordingId recordingId, RecordingMetadata metadata,
        CancellationToken cancellationToken = default);
    Task<bool> UpdateRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken = default);
    Task<bool> UpdateWavPathAsync(RecordingId recordingId, string wavPath, CancellationToken cancellationToken = default);
}
