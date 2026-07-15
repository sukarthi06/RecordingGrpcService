using Grpc.Core;
using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Grpc.Mappers;
using RecordingGrpcService.Grpc.Protos;

namespace RecordingGrpcService.Grpc.Services;

public class RecordingsService(
    ILogger<RecordingsService> logger,
    RecordingMapper mapper,
    IRecordingService recordingService) : RecordingService.RecordingServiceBase
{
    public override async Task<RecordingSessionResponse> SaveRecordingSession(RecordingSessionRequest request, 
        ServerCallContext context)
    {
        logger.LogInformation("Received SaveRecordingSession request: {@Request}", request);
        var entity = mapper.ToDomain(request.RecordingSession);

        var result = await recordingService.SaveRecordingSessionAsync(entity, context.CancellationToken);

        return result is null
            ? new RecordingSessionResponse { IsSuccess = false }
            : new RecordingSessionResponse { IsSuccess = true, RecordingSession = mapper.ToDto(result) };
    }
    public override async Task<GetRecordingSessionResponse> GetRecordingSession(GetRecordingSessionRequest request, 
        ServerCallContext context)
    {        
        var recordingId = mapper.MapRecordingId(request.RecordingId);
        return await recordingService.GetRecordingSessionAsync(recordingId, context.CancellationToken) is { } result
            ? new GetRecordingSessionResponse { RecordingSession = mapper.ToDto(result) }
            : new GetRecordingSessionResponse();
    }
    public override async Task<UpdateRecordingSessionResponse> UpdateRecordingSession(UpdateRecordingSessionRequest request,
        ServerCallContext context)
    {
        var entity = mapper.ToDomain(request.RecordingSession);
        var result = await recordingService.UpdateRecordingSessionAsync(entity, context.CancellationToken);
        return new UpdateRecordingSessionResponse { IsSuccess = result };
    }
    public override async Task<UpdateRecordingMetadataResponse> UpdateRecordingMetadata(UpdateRecordingMetadataRequest request,
        ServerCallContext context)
    {
        var recordingId = mapper.MapRecordingId(request.RecordingId);
        var metadata = mapper.ToDomain(request.Metadata);
        var result = await recordingService.UpdateRecordingMetadataAsync(recordingId, metadata, context.CancellationToken);
        return new UpdateRecordingMetadataResponse { IsSuccess = result };
    }
    public override async Task<UpdateStoragePathResponse> UpdateStoragePath(UpdateStoragePathRequest request,
        ServerCallContext context)
    {
        var recordingId = mapper.MapRecordingId(request.RecordingId);
        var result = await recordingService.UpdateStoragePathAsync(recordingId, request.StoragePath, context.CancellationToken);
        return new UpdateStoragePathResponse { IsSuccess = result };
    }
    public override async Task<UpdateWavPathResponse> UpdateWavPath(UpdateWavPathRequest request,
        ServerCallContext context)
    {
        var recordingId = mapper.MapRecordingId(request.RecordingId);
        var result = await recordingService.UpdateWavPathAsync(recordingId, request.WavPath, context.CancellationToken);
        return new UpdateWavPathResponse { IsSuccess = result };
    }
    public override async Task<GetRecordingMetadataResponse> GetRecordingMetadata(GetRecordingMetadataRequest request,
        ServerCallContext context)
    {
        var recordingId = mapper.MapRecordingId(request.RecordingId);
        var result = await recordingService.GetRecordingMetadataAsync(recordingId, context.CancellationToken);
        return new GetRecordingMetadataResponse { Metadata = mapper.ToDto(result) };
    }
}
