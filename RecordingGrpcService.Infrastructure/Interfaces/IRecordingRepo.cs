using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;

namespace RecordingGrpcService.Infrastructure.Interfaces;

public interface IRecordingRepo
{
    Task<RecordingSession?> GetRecordingSessionAsync(RecordingId recordingId, CancellationToken cancellationToken);
    Task<RecordingSession> SaveRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken);
    Task<bool> UpdateStoragePathAsync(RecordingId recordingId, string storagePath, CancellationToken cancellationToken);
    Task<bool> UpdateRecordingMetadataAsync(RecordingId recordingId, RecordingMetadata metadata,CancellationToken cancellationToken);
    Task<bool> UpdateRecordingSessionAsync(RecordingSession recordingSession, CancellationToken cancellationToken);
    Task<bool> UpdateWavPathAsync(RecordingId recordingId, string wavPath, CancellationToken cancellationToken);
}
